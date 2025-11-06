using System.Collections;
using UnityEngine;
/// <summary>
/// ///// ÉTAT FINAL
/// </summary>  
public class BiomeEtatCultivable : BiomeEtatBase
{
    public override void InitEtat(BiomeEtatManager biome)
    {
        biome.StartCoroutine(Anime(biome));
    }
    public override void UpdateEtat(BiomeEtatManager biome)
    {

    }
    public override void TriggerEnterEtat(BiomeEtatManager biome, Collider other)
    {

    }

    private IEnumerator Anime(BiomeEtatManager biome)
    {
        var ps = Object.Instantiate(biome.ParticlesPrefab, biome.transform.position, Quaternion.identity);// instancie le prefab de particules
        ps.Play();// démarre les particules
        biome.activeCultivableParticles = ps;// garde la référence à l'instance
        yield return new WaitForSeconds(3f);// attend un court instant avant de changer d'état
        // biome.CurrentMaterial = biome.matCultivable;// définit le matériau final pour l'état cultivable




        biome.ChangerEtat(biome.Final);// change d'état vers "Final"
    }


     public override void ExitEtat(BiomeEtatManager biome)
    {
        biome.StopAllCoroutines();
    }
}
