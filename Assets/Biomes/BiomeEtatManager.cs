using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;


public class BiomeEtatManager : MonoBehaviour
{
    private BiomeEtatBase etatActuel;
    public BiomeEtatActivable activable = new BiomeEtatActivable();
    public BiomeEtatCultivable cultivable = new BiomeEtatCultivable();
    public BiomeEtatFinal Final = new BiomeEtatFinal();
    public Dictionary<String, dynamic> infos { get; set; } = new Dictionary<string, dynamic>();

    public float Delay = 1f;
    public ParticleSystem ParticlesPrefab;// assigné dans l'inspector
    public ParticleSystem activeCultivableParticles;

    public int matBiomes { get; set; }
    public int matVariante { get; set; }
    public Quaternion initialRotation;
    // reference to the currently spawned cultivable particle system (if any)

    void Start()
    {
        // CurrentMaterial = matInitial;
        initialRotation = transform.rotation;
        etatActuel = activable;
        etatActuel.InitEtat(this);
        
    }
    public void ChangerEtat(BiomeEtatBase etat)// changer l'état actuel
    {
        etatActuel.ExitEtat(this);
        
        infos["etat"] = etatActuel.GetType().Name.Replace("BiomesEtat", "");

        etatActuel = etat;// assigner le nouvel état


        etatActuel.InitEtat(this);// initialiser le nouvel état
    }

    private void OnTriggerEnter(Collider other)
    {
        etatActuel.TriggerEnterEtat(this, other);// déléguer au state actuel
    }
}
