using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnnemyEtatManager : MonoBehaviour
{
    private EnnemyEtatBase _etatActuel;

    public EnnemyEtatChasse chasse = new EnnemyEtatChasse(); //pourchasser le perso/joueur
    public EnnemieEtatActivable activable = new EnnemieEtatActivable(); //pourchasser le perso/joueur
    public EnnemieEtatAttaque attaque = new EnnemieEtatAttaque(); //promenade aléatoire ou vers ressources
    public EnnemyEtatRepos repos = new EnnemyEtatRepos(); // attendre (écoute si perso entre dans champ vision)
    public EnnemyEtatCuillette cuillette = new EnnemyEtatCuillette(); // vole des ressources

    public NavMeshAgent _agentNavigation;

    public GameObject _personnage;

    
    // public NavMeshSurface _agentNavigation {get; set;} //composant de navigation
    public Dictionary<string, dynamic> _infos  { get; set; } = new Dictionary<string, dynamic>();//dictionnaire
    public Animator _animator {get; set;}  //composant animator

    void Start()
    {
        _animator = GetComponent<Animator>();
        _agentNavigation = GetComponent<NavMeshAgent>();

        ChangerEtat(chasse);  
        // transform.rotation(new Vector3(0,Random.Range(0,360),0));
        _etatActuel = repos;
    }

    public void ChangerEtat(EnnemyEtatBase etat)
    {
        // _etatActuel.ExitEtat(this);
        _etatActuel = etat;
        _etatActuel.InitEtat(this);
    }

    void Update()
    {
        // _etatActuel.UpdateEtat(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        _etatActuel.TriggerEnterEtat(this, other);
    }
}
