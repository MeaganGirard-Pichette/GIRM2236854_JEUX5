using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnnemyEtatChasse : EnnemyEtatBase
{

    // public Transform perso;
    // public Transform bases;
    // public NavMeshAgent agent;
    // private Transform goal;


    public override void InitEtat(EnnemyEtatManager ennemy)
    {
        // goal = perso;
        // InvokeRepeating("EnChasse", 3f, 2f);
        // Initialisation de l'état de chasse
        // ennemy.StartCoroutine(EnChasse(ennemy));
    }

    public override void UpdateEtat(EnnemyEtatManager ennemy)
    {
        // Logique de mise à jour de l'état de chasse
    }

    public override void TriggerEnterEtat(EnnemyEtatManager ennemy, Collider other)
    {
        // Logique lors de l'entrée dans le trigger en état de chasse
    }

    private IEnumerator EnChasse(EnnemyEtatManager ennemy){

        ennemy._animator.SetBool("enCourse", true);
        ennemy._agentNavigation.SetDestination(ennemy._personnage.transform.position);
        if (ennemy._agentNavigation.remainingDistance < 1f)
        {
            ennemy._animator.SetBool("enCourse", false);
        }
        else
        {
            ennemy._animator.SetBool("enCourse", true);
        }


        return null;
    }

   
}

