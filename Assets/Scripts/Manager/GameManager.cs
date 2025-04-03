using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using MessagePipe;

public class GameManager:IStartable,ITickable,IFixedTickable
{

    [Inject] private CollisionableUpdater _collisionableManager;
    [Inject] private CharacterUpdater characterManager;
    
    void IStartable.Start()
    {

    }
    void ITickable.Tick()
    {
        
        _collisionableManager.CollisionableUpdate();
    }
    void IFixedTickable.FixedTick()
    {

    }

}
