using System.Collections;
using UnityEngine;

public class BiomeEtatCultivable : BiomeEtatBase
{
    public override void InitEtat(BiomeEtatManager biome)
    {
        // Charger le prefab spécifique à ce biome
        GameObject itemPrefab = Resources.Load<GameObject>("Items/b1_1");
        if (itemPrefab != null)
        {
            // Instancier le prefab à la position du biome avec un léger décalage
            GameObject item = Object.Instantiate(itemPrefab, biome.transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);

            // Scale aléatoire
            float randomScale = Random.Range(0.5f, 3f);
            item.transform.localScale = Vector3.one * randomScale;

            // Parent pour garder l'objet dans le biome
            item.transform.SetParent(biome.transform);
        }
        else
        {
            Debug.LogWarning("Prefab introuvable : b1_1");
        }

        // Particules cultivables
        var ps = Object.Instantiate(biome.ParticlesPrefab, biome.transform.position, Quaternion.identity);
        ps.Play();
        biome.activeCultivableParticles = ps;

        // Lancer la coroutine pour changer d'état après un délai
        biome.StartCoroutine(Anime(biome));
    }

    private IEnumerator Anime(BiomeEtatManager biome)
    {
        yield return new WaitForSeconds(3f);
        biome.ChangerEtat(biome.Final);
    }

    public override void UpdateEtat(BiomeEtatManager biome) { }

    public override void TriggerEnterEtat(BiomeEtatManager biome, Collider other) { }

    public override void ExitEtat(BiomeEtatManager biome)
    {
        biome.StopAllCoroutines();
    }
}
