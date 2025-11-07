using UnityEngine;
using System.Collections;

public class EnnemieEtatAttaque : EnnemyEtatBase
{
    public override void InitEtat(EnnemyEtatManager ennemy)
    {
        ennemy._animator.SetBool("enAttaque", true);
        Debug.Log("Ennemy en état Attaque");
        ennemy.StartCoroutine(CoroutineAttaque(ennemy));
    }

    public override void ExitEtat(EnnemyEtatManager ennemy)
    {
        ennemy._animator.SetBool("enAttaque", false);
    }

    public override void UpdateEtat(EnnemyEtatManager ennemy)
    {
    }

    public override void TriggerEnterEtat(EnnemyEtatManager ennemy, Collider other)
    {
    }

    public IEnumerator CoroutineAttaque(EnnemyEtatManager ennemy)
    {
        float attackDistance = 2f;
        float chaseDistance = 10f;

        while (ennemy._etatActuel == this && ennemy._personnage != null)
        {
            float distance = Vector3.Distance(ennemy.transform.position, ennemy._personnage.transform.position);

            if (distance > chaseDistance)
            {
                // Joueur trop loin, retour à repos
                ennemy.ChangerEtat(ennemy.repos);
                yield break;
            }
            else if (distance > attackDistance)
            {
                // Joueur un peu loin, retour en chasse
                ennemy.ChangerEtat(ennemy.chasse);
                yield break;
            }
            else
            {
                // Ici tu peux ajouter la logique d'attaque (ex: infliger des dégâts)
                Debug.Log("Ennemy attaque le joueur");
            }

            yield return new WaitForSeconds(0.3f);
        }
    }
}
