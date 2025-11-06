using UnityEngine;
using System.Collections;

public class EnnemieEtatAttaque : EnnemyEtatBase
{
    public override void InitEtat(EnnemyEtatManager ennemy)
    {
        Debug.Log("Ennemi en état d'attaque");
        ennemy._animator.SetBool("enCourse", false);
        ennemy._animator.SetBool("enAttaque", true);

        ennemy.StartCoroutine(CouroutineAttaque(ennemy));
    }

    public override void ExitEtat(EnnemyEtatManager ennemy)
    {
        ennemy.StopAllCoroutines();
    }

    public override void UpdateEtat(EnnemyEtatManager ennemy) { }

    public override void TriggerEnterEtat(EnnemyEtatManager ennemy, Collider other)
    {
        // Rien ici pour l'instant
    }

    public IEnumerator CouroutineAttaque(EnnemyEtatManager ennemy)
    {
        while (true)
        {
            if (ennemy._personnage == null)
            {
                ennemy.ChangerEtat(ennemy.repos);
                yield break;
            }

            float distance = Vector3.Distance(ennemy.transform.position, ennemy._personnage.transform.position);

            if (distance > 3f)
            {
                ennemy.ChangerEtat(ennemy.chasse);
                yield break;
            }

            // Ici tu peux ajouter le code pour infliger des dégâts
            Debug.Log("Attaque le joueur !");
            yield return new WaitForSeconds(1.5f);
        }
    }
}
