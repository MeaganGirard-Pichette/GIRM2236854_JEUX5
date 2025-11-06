using UnityEngine;

public abstract class BiomeEtatBase
{
    public abstract void InitEtat(BiomeEtatManager biome);
    public abstract void ExitEtat(BiomeEtatManager biome);
    public abstract void UpdateEtat(BiomeEtatManager biome);
    public abstract void TriggerEnterEtat(BiomeEtatManager biome, Collider other);
}
