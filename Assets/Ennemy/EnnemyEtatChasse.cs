using UnityEngine;
using System.Collections;

public class EnnemyEtatChasse : EnnemyEtatBase
{
    public override void InitEtat(EnnemyEtatManager ennemy)
    {
        Debug.Log("Ennemi en état de chasse");
        ennemy._animator.SetBool("enCourse", true);
        ennemy._animator.SetBool("enAttaque", false);

        ennemy.StartCoroutine(CouroutineChasse(ennemy));
    }

    public override void ExitEtat(EnnemyEtatManager ennemy)
    {
        ennemy.StopAllCoroutines();
    }

    public override void UpdateEtat(EnnemyEtatManager ennemy) { }

    public override void TriggerEnterEtat(EnnemyEtatManager ennemy, Collider other)
    {
        if (other.gameObject == ennemy._personnage)
        {
            ennemy.ChangerEtat(ennemy.attaque);
        }
    }

    public IEnumerator CouroutineChasse(EnnemyEtatManager ennemy)
    {
        while (true)
        {
            if (ennemy._personnage == null)
            {
                ennemy.ChangerEtat(ennemy.repos);
                yield break;
            }

            Vector3 target = ennemy._personnage.transform.position;
            ennemy._agentNavigation.SetDestination(target);

            float distance = Vector3.Distance(ennemy.transform.position, target);

            if (distance < 2f)
            {
                ennemy.ChangerEtat(ennemy.attaque);
                yield break;
            }

            if (distance > 15f)
            {
                ennemy.ChangerEtat(ennemy.repos);
                yield break;
            }

            yield return null;
        }
    }
}
