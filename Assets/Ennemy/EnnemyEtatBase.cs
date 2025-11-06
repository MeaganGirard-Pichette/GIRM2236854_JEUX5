using UnityEngine;

public abstract class EnnemyEtatBase
{
    public abstract void InitEtat(EnnemyEtatManager ennemy);
    // public abstract void ExitEtat(EnnemyEtatManager ennemy);
    public abstract void UpdateEtat(EnnemyEtatManager ennemy);
    public abstract void TriggerEnterEtat(EnnemyEtatManager ennemy, Collider other);
}
