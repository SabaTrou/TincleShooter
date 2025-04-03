using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePool
{
    //test
    ParticleInstanceCreator _creator = new();
    #region Dic
    private Dictionary<ParticlePrefabValue, List<ParticleSystem>> _allParticles = new();
    private Dictionary<ParticlePrefabValue, Queue<ParticleSystem>> _usableParticles = new();
    #endregion


    private bool TryGetUsableParticle(ParticlePrefabValue prefabValue, out ParticleSystem particle)
    {
        particle = null;
        if (_usableParticles[prefabValue].Count <= 0)
        {
            return false;
        }
        particle = _usableParticles[prefabValue].Dequeue();
        return true;
    }

    private void UpdateUsableParticles(ParticlePrefabValue prefabValue)
    {

        foreach (ParticleSystem particle in _allParticles[prefabValue])
        {
            if(particle==null)
            {
                Debug.Log("particle is missing");
            }
            if (particle.isPlaying)
            {
                continue;
            }
            _usableParticles[prefabValue].Enqueue(particle);
        }

    }

    public ParticleSystem GetParticle(ParticlePrefabValue prefabValue)
    {
        if(!_allParticles.ContainsKey(prefabValue))
        {
            RegisterParticle(prefabValue);
        }
        ParticleSystem particle = null;
        if (TryGetUsableParticle(prefabValue, out particle))
        {
            return particle;
        }
        UpdateUsableParticles(prefabValue);

        if (TryGetUsableParticle(prefabValue, out particle))
        {
            return particle;
        }
        particle=_creator.CreateParticle(prefabValue);
        _allParticles[prefabValue].Add(particle);
        return particle;
    }
    private void RegisterParticle(ParticlePrefabValue prefabValue)
    {
        _allParticles[prefabValue] = new();
        _usableParticles[prefabValue]= new();
    }
}
