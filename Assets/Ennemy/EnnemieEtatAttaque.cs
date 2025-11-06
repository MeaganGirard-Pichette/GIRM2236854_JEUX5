using UnityEngine;
using System.Collections;

public class EnnemieEtatAttaque : EnnemyEtatBase
{
    public override void InitEtat(EnnemyEtatManager ennemy)
    {
        // Initialisation de l'état d'attaque
    }

    public override void UpdateEtat(EnnemyEtatManager ennemy)
    {
        // Logique de mise à jour de l'état d'attaque
    }

    public override void TriggerEnterEtat(EnnemyEtatManager ennemy, Collider other)
    {
        // Logique lors de l'entrée dans le trigger en état d'attaque
    }
}

