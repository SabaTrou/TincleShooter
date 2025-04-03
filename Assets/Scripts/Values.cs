using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BulletPrefabValue
{

    readonly public GameObject prefabObject;
    public BulletPrefabValue(GameObject prefabObject)
    {
        this.prefabObject = prefabObject;
    }
    public override bool Equals(object obj)
    {
        if(obj is BulletPrefabValue value)
        {
            return prefabObject == value.prefabObject;
        }
        return false;
    }
    public override int GetHashCode()
    {
        return prefabObject.GetHashCode();
    }

}
public class ParticlePrefabValue
{
    readonly public ParticleSystem particle;
    public ParticlePrefabValue(ParticleSystem particle)
    {
        this.particle = particle;
    }
    public override bool Equals(object obj)
    {
        if(obj is ParticlePrefabValue value)
        {
            return particle == value.particle;
        }
        return false;
    }
    public override int GetHashCode()
    {
        return particle.GetHashCode();
    }
}


