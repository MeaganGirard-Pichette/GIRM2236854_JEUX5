using UnityEngine;

public abstract class EnnemyEtatBase
{
    public abstract void InitEtat(EnnemyEtatManager ennemy);
    // public abstract void ExitEtat(EnnemyEtatManager ennemy);
    public abstract void UpdateEtat(EnnemyEtatManager ennemy);
        public abstract void ExitEtat(EnnemyEtatManager ennemi);      // Nettoyage quand on change d'état

    public abstract void TriggerEnterEtat(EnnemyEtatManager ennemy, Collider other);
}
