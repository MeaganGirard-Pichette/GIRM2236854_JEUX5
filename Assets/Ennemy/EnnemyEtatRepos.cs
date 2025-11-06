using UnityEngine;
using System.Collections;

public class EnnemyEtatRepos : EnnemyEtatBase
{
  public override void InitEtat(EnnemyEtatManager ennemy)
    {
        // ennemy._animator.SetBool("enCourse", false);
        // ennemy._animator.SetBool("enAttaque", false);
        Debug.Log("Ennemy en état de repos");
    }

  

    public override void UpdateEtat(EnnemyEtatManager ennemy)
    {
        // Logique de mise à jour de l'état de cuillette
    }

    public override void TriggerEnterEtat(EnnemyEtatManager ennemy, Collider other)
    {
        // Logique lors de l'entrée dans le trigger en état de cuillette
    }  
}

