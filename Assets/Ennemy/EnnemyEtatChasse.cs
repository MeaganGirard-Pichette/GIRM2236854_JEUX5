using UnityEngine;
using System.Collections;

public class EnnemyEtatChasse : EnnemyEtatBase
{
    public override void InitEtat(EnnemyEtatManager ennemy)
    {
        ennemy._animator.SetBool("enCourse", true);
        Debug.Log("Ennemy en état Chasse");
        ennemy.StartCoroutine(CoroutineChasse(ennemy));
    }

    public override void ExitEtat(EnnemyEtatManager ennemy)
    {
        ennemy._agentNavigation.ResetPath();
        ennemy._animator.SetBool("enCourse", false);
    }

    public override void UpdateEtat(EnnemyEtatManager ennemy)
    {
    }

    public override void TriggerEnterEtat(EnnemyEtatManager ennemy, Collider other)
    {
    }

    public IEnumerator CoroutineChasse(EnnemyEtatManager ennemy)
    {
        float loseDistance = 15f;
        float attackDistance = 2f;

        while (ennemy._etatActuel == this && ennemy._personnage != null)
        {
            float distance = Vector3.Distance(ennemy.transform.position, ennemy._personnage.transform.position);

            // Si joueur trop loin, retourne à repos
            if (distance > loseDistance)
            {
                ennemy.ChangerEtat(ennemy.repos);
                yield break;
            }
            // Si joueur proche, attaque
            else if (distance <= attackDistance)
            {
                ennemy.ChangerEtat(ennemy.attaque);
                yield break;
            }
            else
            {
                // Se déplacer vers le joueur
                if (ennemy._agentNavigation != null)
                    ennemy._agentNavigation.SetDestination(ennemy._personnage.transform.position);
            }

            yield return new WaitForSeconds(0.2f);
        }
    }
}
