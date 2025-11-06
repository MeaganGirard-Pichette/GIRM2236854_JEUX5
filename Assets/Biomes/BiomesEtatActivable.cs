using System.Collections;
using UnityEngine;
/// <summary>
/// ///// Classe représentant l'état "Activable" d'un biome.
/// État "Activable" : passage à "Cultivable" via trigger
///  ÉTAT DE DÉPART
/// </summary>
public class BiomeEtatActivable : BiomeEtatBase
{
    public override void InitEtat(BiomeEtatManager biome)
    {
        // Ne change plus immédiatement d'état : on attend un trigger
        biome.GetComponent<Renderer>().material = (Material)Resources.Load("mats/b" + biome.infos["quelBiomes"] + "_" + biome.infos["quelVariante"]);

    }
    public override void UpdateEtat(BiomeEtatManager biome)
    {
        // Comportement périodique de l'état activable (si besoin)
    }
    public override void TriggerEnterEtat(BiomeEtatManager biome, Collider other)
    {
        if (other != null && other.CompareTag("perso")) { biome.StartCoroutine(Anime(biome)); }

    }

    private IEnumerator Anime(BiomeEtatManager biome)
    {
        // t += Time.deltaTime;
        // float rotation = Mathf.Lerp(startRotation, endRotation, t / duration)





        Quaternion start = biome.initialRotation;// utilise la rotation initiale sauvegardée depuis le manager
        // biome.CurrentMaterial = biome.matActivable;// définit le matériau temporaire pour l'état activable

        float duration = Mathf.Max(0.0001f, biome.Delay);// durée de l'animation
        float elapsed = 0f;// temps écoulé
        while (elapsed < duration)// animation en cours
        {
            elapsed += Time.deltaTime;// incrémente le temps écoulé
            float t = Mathf.Clamp01(elapsed / duration);// calcule le ratio de progression
            float angle = Mathf.Lerp(0f, 360f, t);// calcule l'angle de rotation
            yield return null; // attendre la frame suivante pour ne pas bloquer
        }

        biome.transform.rotation = start;// réinitialise la rotation finale
        // biome.CurrentMaterial = biome.matCultivable;// définit le matériau final pour l'état cultivable

        biome.ChangerEtat(biome.cultivable);// change d'état vers "Cultivable"

        yield return new WaitForSeconds(1.5f);// attend un court instant avant de changer d'état
    }

    public override void ExitEtat(BiomeEtatManager biome)
    {
        biome.StopAllCoroutines();
    }
}
