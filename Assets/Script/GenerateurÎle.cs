using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine.AI;



/// <summary>
/// Générateur procédural d'îles en 3D utilisant le bruit de Perlin et des techniques d'érosion
/// Ce script génère une île constituée de cubes colorés selon l'altitude
/// </summary>
public class GenerateurÎle : MonoBehaviour
{
    [Header("IsLand Colors")]
    public List<Material> _biomeMats = new List<Material>(); // Liste de matériaux pour différents biomes (couleurs selon l'altitude)


    [Header("Matériaux et Effets")]
    public Material shaderGraphMaterial;      // Matériau avec shader graph pour les effets visuels
    public GameObject generationEffect;       // Effet de particules lors de la génération

    [Header("Dimensions de l'Île")]
    private int _ileProfondeur;      // Nombre de cubes en profondeur (axe Z)
    private int _ileLargeur;         // Nombre de cubes en largeur (axe X)
    private Vector3 _positionOrigin;

    [Header("Préfabs et Rendu")]
    [SerializeField] GameObject _cube;        // Préfab du cube à instancier pour chaque cellule de terrain
    [SerializeField] Renderer textureRender; // Renderer pour afficher la texture de l'île (vue de dessus)

    [Header("Génération")]
    public float generateDelay = 0.1f; // délai entre les lignes lors de l'instanciation

    [Header("Paramètres de Terrain")]
    private float _erosionIle;      // Intensité de l'érosion des bords (0-100, plus élevé = érosion plus douce)
    private float _pourcentageHorsEau; // Pourcentage du terrain qui reste hors de l'eau (0-100)
    private float _coefY;           // Coefficient multiplicateur pour la hauteur des cubes (plus grand = île plus haute (axe Y))

    [Header("Génération Procédurale")]
    [Range(1f, 35f)]
    private float _attenuation = 10f;          // Facteur d'atténuation pour le zoom du bruit de Perlin (plus petit = plus de détails)

    // [Header("Couleurs par Altitude")]
    // public Color[] _couleursCubes;           // Palette de couleurs selon l'altitude (index 0 = plus bas, dernier index = plus haut)

    [Header("Personnages")]
    [SerializeField] GameObject _prefabPersonnage; // Prefab du personnage (capsule + cube)
    [SerializeField] int _nombrePersonnages;  // Nombre de personnages à placer sur l'île

    // Variables privées pour la gestion des cubes et personnages
    private List<GameObject> _cubesInstancies; // Liste des cubes créés sur l'île

    private List<BiomeEtatManager> biomesListe = new List<BiomeEtatManager>();


    void Start()
    {
        // île circulaire  escarpé coef hauteur 10 unité
        InitIsLand(Vector3.zero, 250, 250, 10, 15, 99, 15, true);//en ordre de ligne 43//InitIsLand
        // StartCoroutine(InfosMonde());

    }



    public List<BiomeEtatManager> ChercherBiomes(string info, dynamic valeur)
    {
        List<BiomeEtatManager> tempList = new List<BiomeEtatManager>();
        for (int i = 0; i < biomesListe.Count; i++)
        {
            if (biomesListe[i].infos.ContainsKey(info))
            {
                if (biomesListe[i].infos[info].Equals(valeur))
                {
                    tempList.Add(biomesListe[i]);
                }
            }
        }
        return tempList;
        // return biomesList.Where(biome => biome.infos.ContainsKey(cleInfo) && biome.infos[cleInfo]...) pas fini
    }
    private float PourcentageBiome(string typeBiome)
    {
        return Mathf.Round((float)ChercherBiomes("etat", typeBiome).Count / (float)biomesListe.Count * 1000) / 10;
    }

    private IEnumerator InfosMonde()
    {
        while (true)
        {
            Debug.Log("Activales" + PourcentageBiome("Activable")
            + "%\nCultivable:" + PourcentageBiome("Cultivable")
            + "%\nCultivable:" + PourcentageBiome("Final"));
            yield return new WaitForSeconds(3);
        }
    }


    private List<List<Material>> _biomesMats = new List<List<Material>>();//fait une liste dans une liste des matériels
    private void LoadRessources()
    {
        int nbBiomes = 1;
        int nbVariant = 1;
        bool resteDesMats = true;
        List<Material> tpBiome = new List<Material>();
        do
        {
            Material mats = (Material)Resources.Load("Items/b" + nbBiomes + "_" + nbVariant);
            if (mats)
            {
                tpBiome.Add((Material)mats);
                nbVariant++;
            }
            else
            {
                if (nbVariant == 1)
                {
                    resteDesMats = false;
                }
                else
                {
                    _biomesMats.Add(tpBiome);
                    tpBiome = new List<Material>();
                    nbBiomes++;
                    nbVariant = 1;
                }
            }
        } while (resteDesMats);
    }
    public void InitIsLand(Vector3 originIsLand, int ileLargeurInit, int ileProfondeurInit, int coefYInit, float AttenuationInit, float pourcentageHorsEauIni, float erosionIleInit, bool circular)
    {
        _positionOrigin = originIsLand;
        _ileLargeur = ileLargeurInit;
        _ileProfondeur = ileProfondeurInit;
        _coefY = coefYInit;
        _attenuation = AttenuationInit;
        _pourcentageHorsEau = pourcentageHorsEauIni;
        _erosionIle = erosionIleInit;
        LoadRessources();
        CreateIsLand(circular);
    }

    private void CreateIsLand(bool circulaire)
    {
        transform.position = _positionOrigin;
        // 1. Génération du terrain de base avec bruit de Perlin
        float[,] unTerrain = Terraformation(_ileLargeur, _ileProfondeur, _attenuation);
        float[,] oneBiomeMap = Terraformation(_ileLargeur, _ileProfondeur, _attenuation * 5f);
        float[,] onVariantMap = Terraformation(_ileLargeur, _ileProfondeur, _attenuation * 2f);
        float[,] onIsLand = circulaire ? Aquaformation(unTerrain) : AquaformationCirculaire(unTerrain);

        // 2. Application de l'érosion circulaire pour créer la forme d'île
        unTerrain = AquaformationCirculaire(unTerrain);
        // Note: AquaformationCarrée disponible en alternative (commentée)
        // unTerrain = Aquaformation(unTerrain);

        // 3. Instanciation des cubes de manière asynchrone pour éviter les blocages
        // StartCoroutine(InstantierCubes(onIsLand));
        InstantierCubes(onIsLand, oneBiomeMap, onVariantMap);
        // PlacerPersos(_nombrePersonnages);
    }





    /// <summary>
    /// Génère un terrain de base en utilisant le bruit de Perlin
    /// </summary>
    /// <param name="largeur">Largeur du terrain en nombre de cellules</param>
    /// <param name="profondeur">Profondeur du terrain en nombre de cellules</param>
    /// <param name="zoomPerlin">Facteur de zoom pour le bruit de Perlin (plus petit = plus de détails)</param>
    /// <returns>Tableau 2D contenant les hauteurs normalisées (0-1) de chaque point du terrain</returns>
    float[,] Terraformation(int largeur, int profondeur, float zoomPerlin)
    {
        // Initialisation du tableau 2D pour stocker les hauteurs du terrain
        float[,] terrain = new float[largeur, profondeur];

        // Génération d'une graine aléatoire pour assurer la variation entre les générations
        int bruitAleatoire = UnityEngine.Random.Range(0, 10000);

        // Parcours de chaque point du terrain
        for (int z = 0; z < profondeur; z++)
        {
            for (int x = 0; x < largeur; x++)
            {
                // Calcul des coordonnées pour le bruit de Perlin
                // Addition de la graine aléatoire pour décaler l'origine du bruit
                float tpX = bruitAleatoire + (x / zoomPerlin);
                float tpZ = bruitAleatoire + (z / zoomPerlin);

                // Génération de la hauteur avec Perlin Noise (valeur entre 0 et 1)
                terrain[x, z] = Mathf.PerlinNoise(tpX, tpZ);
            }
        }
        return terrain;
    }

    /// <summary>
    /// Applique une érosion carrée au terrain pour créer une forme d'île rectangulaire
    /// Les bords sont progressivement érodés selon un masque carré
    /// </summary>
    /// <param name="terrain">Tableau 2D des hauteurs du terrain à modifier</param>
    /// <returns>Terrain modifié avec érosion carrée appliquée</returns>
    float[,] Aquaformation(float[,] terrain)
    {
        int largeur = terrain.GetLength(0);
        int profondeur = terrain.GetLength(1);

        for (int z = 0; z < profondeur; z++)
        {
            for (int x = 0; x < largeur; x++)
            {
                // Normalisation des coordonnées entre -1 et 1 (centre = 0, bords = ±1)
                float tpX = Mathf.Abs(x / (float)largeur * 2 - 1);
                float tpZ = Mathf.Abs(z / (float)profondeur * 2 - 1);

                // Utilisation de la distance Manhattan (distance carrée)
                // Max donne la plus grande des deux distances pour créer un masque carré
                float tpVal = Mathf.Max(tpX, tpZ);

                // Application de la fonction sigmoïde pour un fondu progressif
                tpVal = Sigmoid(tpVal);

                // Soustraction du masque d'érosion à la hauteur du terrain
                // Clamp01 assure que les valeurs restent entre 0 et 1
                terrain[x, z] = Mathf.Clamp01(terrain[x, z] - tpVal);
            }
        }
        return terrain;
    }
    /// <summary>
    /// Applique une érosion circulaire au terrain pour créer une forme d'île ronde
    /// Utilise la distance euclidienne depuis le centre pour créer un masque circulaire
    /// </summary>
    /// <param name="terrain">Tableau 2D des hauteurs du terrain à modifier</param>
    /// <returns>Terrain modifié avec érosion circulaire appliquée</returns>
    float[,] AquaformationCirculaire(float[,] terrain)
    {
        int largeur = terrain.GetLength(0);
        int profondeur = terrain.GetLength(1);

        // Calcul du centre géométrique de l'île
        float centreX = (largeur - 1) / 2f;
        float centreZ = (profondeur - 1) / 2f;

        // Définition du rayon maximum (distance du centre au bord)
        float rayon = Mathf.Min(largeur, profondeur) / 2f;

        for (int z = 0; z < profondeur; z++)
        {
            for (int x = 0; x < largeur; x++)
            {
                // Calcul des composantes du vecteur distance depuis le centre
                float dx = x - centreX;  // Distance horizontale (côté adjacent)
                float dz = z - centreZ;  // Distance verticale (côté opposé)

                // Distance euclidienne normalisée (théorème de Pythagore)
                // 0 = centre exact, 1 = bord du cercle
                float distance = Mathf.Sqrt(dx * dx + dz * dz) / rayon;

                // Sécurité : s'assurer que la distance reste dans [0,1]
                distance = Mathf.Clamp01(distance);

                // Application de la fonction sigmoïde pour une transition douce
                float tpVal = Sigmoid(distance);

                // Soustraction du masque d'érosion à la hauteur du terrain
                terrain[x, z] = Mathf.Clamp01(terrain[x, z] - tpVal);
            }
        }
        return terrain;
    }

    /// <summary>
    /// Coroutine qui instancie les cubes du terrain de manière asynchrone
    /// Évite les blocages de performance en générant ligne par ligne avec des pauses
    /// </summary>
    /// <param name="terrain">Tableau 2D contenant les hauteurs normalisées du terrain</param>
    /// <returns>IEnumerator pour la coroutine</returns>
    private void InstantierCubes(float[,] terrain, float[,] mapBiome, float[,] mapVariant)
    {
        int largeur = terrain.GetLength(0);
        int profondeur = terrain.GetLength(1);

        Vector3 posIsLand = transform.position;
        // largeur /2 et profomdeur /2

        Vector3 offsetIle = new Vector3(largeur / 2, 0f, profondeur / 2f);// offsetIle = Vector3 avec largeur /2 pour x et profondeur /2 pour z

        // Initialisation de la liste des cubes instanciés (accessible depuis d'autres méthodes)
        _cubesInstancies = new List<GameObject>();

        // Génération ligne par ligne pour permettre l'affichage progressif
        for (int z = 0; z < profondeur; z++)
        {
            for (int x = 0; x < largeur; x++)
            {
                // Ne créer un cube que si la hauteur est positive (au-dessus du niveau de la mer)
                if (terrain[x, z] > 0)
                {
                    // Instanciation du cube à la position (x, position île actuel+ hauteur*coeff, z)
                    GameObject unCube = Instantiate(_cube, posIsLand + new Vector3(x, terrain[x, z] * _coefY, z), Quaternion.identity);
                    _cubesInstancies.Add(unCube);// Ajout à la liste pour traitement ultérieur
                    unCube.name = $"Cube_{x}_{z}";// Nommer le cube pour identification
                    // Ensure the cube is active (prefab might be inactive) and log its state for debugging
                    unCube.SetActive(true);

                    int quelBiomes = Mathf.RoundToInt(mapBiome[x, z] * (_biomesMats.Count - 1));
                    int quelVariante = Mathf.RoundToInt(mapVariant[x, z] * (_biomesMats[quelBiomes].Count - 1));//Random.Range(0, _biomesMats[quelBiomes].Count); -> ancient quel variant

                    // unCube.GetComponent<Renderer>().material = _biomesMats[quelBiomes][quelleVariante];    Va permettre de savoir uelle matérielle prendre
                    unCube.GetComponent<BiomeEtatManager>().infos.Add("quelBiomes", quelBiomes + 1);
                    unCube.GetComponent<BiomeEtatManager>().infos.Add("quelVariante", +quelVariante + 1);

                    // Debug.Log("Biome:" + (quelBiomes + 1) + " Variante:" + (quelVariante + 1));
                    unCube.transform.parent = this.transform;// Attachment du cube comme enfant de ce GameObject pour l'organisation

                    GetComponent<NavMeshSurface>().BuildNavMesh();

                }
            }
            // Pause configurable entre chaque ligne pour permettre l'affichage progressif
            // yield return new WaitForSeconds(generateDelay);
        }

        // Phase de finalisation : reconfirmation des couleurs de tous les cubes
        // Utile si des modifications de matériau ont eu lieu pendant la génération
        for (int i = 0; i < _cubesInstancies.Count; i++)
        {
            var rend = _cubesInstancies[i].GetComponent<Renderer>();

            // Récupération des coordonnées originales du cube
            int x = Mathf.RoundToInt(_cubesInstancies[i].transform.position.x);
            int z = Mathf.RoundToInt(_cubesInstancies[i].transform.position.z);

            // Recalcul et réapplication de la couleur finale
            int quelleCouleur = Mathf.FloorToInt(terrain[x, z] * (_biomeMats.Count - 1));
            rend.material = _biomeMats[quelleCouleur];

        }

        // 4. Placement automatique des personnages après génération des cubes

    }

    /// <summary>
    /// Génère et affiche une texture 2D représentant l'île vue de dessus
    /// Utile pour le debug et la visualisation du terrain généré
    /// </summary>
    /// <param name="terrain">Tableau 2D contenant les hauteurs du terrain</param>
    void AfficherIle(float[,] terrain)
    {
        int largeur = terrain.GetLength(0);
        int profondeur = terrain.GetLength(1);

        // Création d'une texture 2D aux dimensions du terrain
        Texture2D texture = new Texture2D(largeur, profondeur);

        // Tableau de couleurs pour chaque pixel de la texture
        Color[] couleursTabl = new Color[largeur * profondeur];

        for (int z = 0; z < profondeur; z++)
        {
            for (int x = 0; x < largeur; x++)
            {
                // Conversion des coordonnées 2D en index 1D pour le tableau de couleurs
                int index = z * largeur + x;

                // Génération d'une couleur en niveaux de gris basée sur l'altitude
                // Altitude 0 = noir (eau), Altitude 1 = blanc (sommet)
                Color tpCol = new Color(1, 1, 1) * terrain[x, z];

                // Stockage de la couleur dans le tableau
                couleursTabl[index] = tpCol;
            }

            // Application des couleurs à la texture
            texture.SetPixels(couleursTabl);
            texture.Apply(); // Finalisation des changements sur la texture

            // Affectation de la texture au matériau du renderer pour affichage
            textureRender.sharedMaterial.mainTexture = texture;
        }
    }

    /// <summary>
    /// Fonction sigmoïde personnalisée pour contrôler l'érosion des bords de l'île
    /// Crée une transition douce entre les zones émergées et submergées
    /// </summary>
    /// <param name="value">Valeur d'entrée normalisée (généralement distance du centre)</param>
    /// <returns>Facteur d'érosion entre 0 (pas d'érosion) et 1 (érosion complète)</returns>
    private float Sigmoid(float value)
    {
        // k = intensité de la pente de transition
        // Plus _erosionIle est faible, plus k est grand et la transition est abrupte
        // Plus _erosionIle est élevé, plus k est petit et la transition est douce
        float k = (100f - _erosionIle) / 10f;

        // c = point central de la transition (seuil)
        // Contrôle à quelle distance du bord commence l'érosion
        // _pourcentageHorsEau détermine quelle proportion du terrain reste émergée
        float c = _pourcentageHorsEau / 100f;

        // Formule mathématique de la fonction sigmoïde
        // Retourne une valeur entre 0 et 1 avec une courbe en S
        // f(x) = 1 / (1 + e^(-k*(x-c)))
        return 1 / (1 + Mathf.Exp(-k * (value - c)));
    }

    /// <summary>
    /// Place des personnages aléatoirement sur les cubes visibles de l'île
    /// </summary>
    /// <param name="nbP">Nombre de personnages à placer (limité à la moitié des cubes instanciés)</param>
    public void PlacerPersos(int nbP)
    {
        // Validation du nombre de personnages avec opérateur ternaire
        // nbP ne peut dépasser la moitié du nombre de cubes instanciés
        int nbMaxPersos = _cubesInstancies?.Count / 2 ?? 0; // Si la liste est nulle, nbMaxPersos = 0
        int nbPersonnagesValide = (nbP <= nbMaxPersos) ? nbP : nbMaxPersos;// Si nbP est valide, on l'utilise, sinon on prend nbMaxPersos

        // Vérification qu'on a des cubes et un prefab (expression ternaire)
        var cubesDisponibles = (_cubesInstancies?.Count > 0 && _prefabPersonnage != null) ?
        new List<GameObject>(_cubesInstancies) : new List<GameObject>();// Liste vide si pas de cubes ou prefab

        // Mélange de la liste des cubes disponibles pour un placement aléatoire
        cubesDisponibles = MelangerListeCubes(cubesDisponibles);

        // Placement des personnages sur les cubes mélangés
        for (int i = 0; i < nbPersonnagesValide && i < cubesDisponibles.Count; i++)
        {
            GameObject cubeChoisi = cubesDisponibles[i];

            // Position du personnage : au-dessus du cube choisi
            Vector3 positionPersonnage = cubeChoisi.transform.position + Vector3.up * 1.5f;// Décalage vertical pour être au-dessus du cube

            // Instanciation du personnage
            GameObject nouveauPersonnage = Instantiate(_prefabPersonnage, positionPersonnage, Quaternion.identity);
            nouveauPersonnage.name = $"Personnage_{i + 1}";// Nommer le personnage pour identification
            nouveauPersonnage.transform.parent = this.transform;// Parentage pour organisation dans la hiérarchie

        }
    }

    /// <summary>
    /// Mélange une liste de GameObjects (algorithme Fisher-Yates)
    /// </summary>
    /// <param name="uneListe">Liste à mélanger</param>
    /// <returns>Liste mélangée</returns>
    List<GameObject> MelangerListeCubes(List<GameObject> uneListe)
    {
        for (int i = 0; i < uneListe.Count; i++)
        {
            GameObject temp = uneListe[i];
            int randomIndex = UnityEngine.Random.Range(i, uneListe.Count);
            uneListe[i] = uneListe[randomIndex];
            uneListe[randomIndex] = temp;
        }
        return uneListe;
    }
}
