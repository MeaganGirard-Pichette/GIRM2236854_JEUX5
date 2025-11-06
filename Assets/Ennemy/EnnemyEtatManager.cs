using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnnemyEtatManager : MonoBehaviour
{
    private EnnemyEtatBase _etatActuel;

    public EnnemyEtatChasse chasse = new EnnemyEtatChasse(); //pourchasser le perso/joueur
    public EnnemieEtatAttaque attaque = new EnnemieEtatAttaque(); //promenade aléatoire ou vers ressources
    public EnnemyEtatRepos repos = new EnnemyEtatRepos(); // attendre (écoute si perso entre dans champ vision)
    public EnnemieEtatActivable activable = new EnnemieEtatActivable(); //pourchasser le perso/joueur

    public NavMeshAgent _agentNavigation;
    public Animator _animator { get; set; }  //composant animator

    public GameObject _personnage;


    // public NavMeshSurface _agentNavigation {get; set;} //composant de navigation
    public Dictionary<string, dynamic> _infos { get; set; } = new Dictionary<string, dynamic>();//dictionnaire

    void Start()
    {
        _animator = GetComponent<Animator>();
        _agentNavigation = GetComponent<NavMeshAgent>();

        _etatActuel = activable;
        _etatActuel.InitEtat(this);
    }

    public void ChangerEtat(EnnemyEtatBase nouvelEtat)
    {
        if (_etatActuel != null) _etatActuel.ExitEtat(this); // Nettoyer l'état actuel
        _etatActuel = nouvelEtat;
        if (_etatActuel != null)  _etatActuel.InitEtat(this); // Initialiser le nouvel état
    }

    void Update()
    {
        // _etatActuel.UpdateEtat(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_etatActuel != null) _etatActuel.TriggerEnterEtat(this, other);
    }
}
