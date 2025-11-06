using UnityEngine;

public class EnnemieEtatActivable : EnnemyEtatBase
{

    public override void InitEtat(EnnemyEtatManager ennemy)
    {
        Debug.Log("Ennemy en état activable");
    }

    public override void UpdateEtat(EnnemyEtatManager ennemy)
    {
        // Logique de mise à jour de l'état activable
    }

    public override void TriggerEnterEtat(EnnemyEtatManager ennemy, Collider other)
    {
        // Logique lors de l'entrée dans le trigger en état activable
    } 
}
