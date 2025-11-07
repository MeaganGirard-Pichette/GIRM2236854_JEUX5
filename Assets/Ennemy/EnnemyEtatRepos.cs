using UnityEngine;
using System.Collections;

public class EnnemyEtatRepos : EnnemyEtatBase
{
    public override void InitEtat(EnnemyEtatManager ennemy)
    {
        ennemy._animator.SetBool("enCourse", false);
        ennemy._animator.SetBool("enAttaque", false);
        Debug.Log("Ennemy en état Repos");
        ennemy.StartCoroutine(CoroutineRepos(ennemy));
    }

    public override void ExitEtat(EnnemyEtatManager ennemy)
    {
    }

    public override void UpdateEtat(EnnemyEtatManager ennemy)
    {
    }

    public override void TriggerEnterEtat(EnnemyEtatManager ennemy, Collider other)
    {
    }

    public IEnumerator CoroutineRepos(EnnemyEtatManager ennemy)
    {
        float detectionDistance = 10f;

        while (ennemy._etatActuel == this)
        {
            if (ennemy._personnage != null)
            {
                float distance = Vector3.Distance(ennemy.transform.position, ennemy._personnage.transform.position);
                if (distance <= detectionDistance)
                {
                    ennemy.ChangerEtat(ennemy.chasse);
                    yield break;
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
