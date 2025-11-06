// using System.Collections.Generic;
// using UnityEngine;

// public class GenerateurArchipel : MonoBehaviour
// {
//     public GameObject ilePrefab; // Le prefab d'île contenant le script de génération de l'île et la méthode InitIle()
//     public List<int> lesDiametres = new List<int>(); // une liste des diamètres désirés pour toutes les îles de l'archipel

//     // void Start()
//     // {
//     //     // 1. Calcule des positions avec la méthode CalculPositionsIles qui reçoit la liste des diamètres et retourne une liste de position pour chaque centre des îles.
//     //     List<Vector3> lesPositions = CalculPositionsIles(lesDiametres);
//     //     // 2. Instanciation des îles aux positions calculées
//     //     for (int i = 0; i < lesPositions.Count; i++)
//     //     {
//     //         // Prend la position de la liste de position
//     //         Vector3 position = lesPositions[i];
//     //         // Prend le diamètre de la liste de diamètre (qui a servi à calculer la position)
//     //         int diametre = lesDiametres[i];
//     //         // Instancie l'île à la position de l'archipel
//     //         GameObject uneIle = Instantiate(ilePrefab, position, Quaternion.identity);
//     //         // Initialise l'île avec le script et profondeur à la valeur diamètre :
//     //         uneIle.GetComponent<GenerateurÎle>().InitIsLand(position, diametre, diametre, 10, 15f, 85f, 15f, true);
//     //         // Donne un nom à l'île pour plus de clarté dans la hierarchie
//     //         uneIle.name = "Ile_" + i + "_D" + diametre;
//     //         // uneIle.transform.parent = transform;
//     //     }
//     // }

//     // Calcule le rayon de l'archipel et les coordonnées (x, z) pour les centres de toutes les îles.
//     // Retourne une liste de Vector3 représentant les positions (x, 0, z) des centres d'îles.
//     public List<Vector3> CalculPositionsIles(List<int> D_I)
//     {
//         // --- Etape 1 : Calcule de la longueur de la circonférence totale et du rayon de l'Archipel ---
//         float c_archipel = 0f;
//         for (int i = 0; i < D_I.Count; i++)
//         {
//             c_archipel += D_I[i];
//         }
//         float r_archipel = c_archipel / (2 * Mathf.PI);
//         // Mathf.PI est une constante pratique dans Unity
//         float R = r_archipel + Mathf.Max(D_I.ToArray()) / 2f;
//         // --- Etape 2 : Détermination des Angles et Coordonnées pour chaque Ile ---
//         List<Vector3> listeCentresIles = new List<Vector3>();
//         float theta_total = 0f;
//         for (int i = 0; i < D_I.Count; i++)
//         {
//             // Calcule l'angle theta occupé par cette ile (Theta = D_I / r_archipel).
//             float theta = (float)D_I[i] / r_archipel;
//             // Calcule l'angle du centre de l'île par rapport au centre de l'archipel.
//             float angleCentre = theta_total + theta / 2f;
//             // Calcule la position (x, z) du centre de l'île.
//             float x = R * Mathf.Cos(angleCentre);
//             float z = R * Mathf.Sin(angleCentre);
//             Vector3 centre = new Vector3(x, 0f, z);
//             listeCentresIles.Add(centre);
//             // Debug.Log($"Ile {i} centre: ({centre.x:F2}, {centre.z:F2}) Angle={angleCentre + Mathf.Rad2Deg:F2}");
//             theta_total += theta; // Ajoute l'angle complet de l'île que nous venons de traiter.
//         }
//         // Remarque : À la fin de la boucle, 'theta_total' devrait être très proche de 2*PI (environ 6.283...).
//         return listeCentresIles;
//     }
// }
