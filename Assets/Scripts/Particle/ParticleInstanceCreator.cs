using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ParticleInstanceCreator
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ParticleSystem CreateParticle(ParticlePrefabValue particlePrefabValue)
    {
        if (particlePrefabValue.particle == null)
        {
            return null;
        }
        return MonoBehaviour.Instantiate(particlePrefabValue.particle);
    }
}
