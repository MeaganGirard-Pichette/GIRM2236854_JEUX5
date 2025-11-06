using UnityEngine;
using System.Collections;

public class EnnemyEtatRepos : EnnemyEtatBase
{
    public override void InitEtat(EnnemyEtatManager ennemy)
    {
        ennemy._animator.SetBool("enCourse", false);
        ennemy._animator.SetBool("enAttaque", false);


        Debug.Log("Ennemy en état de repos");
        ennemy.StartCoroutine(CouroutineRepos(ennemy));
    }

    public override void ExitEtat(EnnemyEtatManager ennemy)
    {
        ennemy.StopAllCoroutines();
    }

    public override void UpdateEtat(EnnemyEtatManager ennemy)
    {
        // Rien de spécial ici, tout est dans la coroutine
    }

    public override void TriggerEnterEtat(EnnemyEtatManager ennemy, Collider other)
    {
        // Si le joueur entre dans un trigger, on peut décider d’attaquer directement ici
    }

    public IEnumerator CouroutineRepos(EnnemyEtatManager ennemy)
    {

            while (true)
        {
            if (ennemy._personnage != null)
            {
                float distance = Vector3.Distance(ennemy.transform.position, ennemy._personnage.transform.position);

                // Si le joueur s'approche trop, passer à la chasse
                if (distance < 10f)
                {
                    ennemy.ChangerEtat(ennemy.chasse);
                    yield break;
                }
            }

            yield return null;
        }
    }
}

