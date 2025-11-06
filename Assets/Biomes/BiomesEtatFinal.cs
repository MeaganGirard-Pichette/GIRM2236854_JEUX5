using System.Collections;
using UnityEngine;
/// <summary>
/// ///// ÉTAT FINAL
/// </summary>  
public class BiomeEtatFinal : BiomeEtatBase
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
        // if (other != null && other.CompareTag("perso"))  biome.StartCoroutine(Anime(biome));
        //Pour un éventuelle changement de matérielle ou d'état futur
    }

    private IEnumerator Anime(BiomeEtatManager biome)
    {
        yield return new WaitForSeconds(3f);// attend un court instant avant de changer d'état


        // biome.CurrentMaterial = biome.matFinal;// définit le matériau final pour l'état final


    

        yield return new WaitForSeconds(1f);// attend un court instant avant de nettoyer les particules
        if (biome.activeCultivableParticles != null)// vérifie si un système de particules est actif
        {
            GameObject.Destroy(biome.activeCultivableParticles.gameObject);// détruit le système de particules actif
            biome.activeCultivableParticles = null;// nettoie la référence
        }

    }



     public override void ExitEtat(BiomeEtatManager biome)
    {
        biome.StopAllCoroutines();
    }
}
