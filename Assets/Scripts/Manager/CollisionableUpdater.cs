using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using MessagePipe;

public class CollisionableUpdater
{
    [Inject] private CharacterUpdater _characterUpdater;
    [Inject] private BulletsUpdater _bulletUpdater;
    [Inject] private ControllerUpdater _controllerUpdater;
    
    [Inject] private CollisionChecker _checker;
    public void CollisionableUpdate()
    {
        _controllerUpdater.ControllersUpdate();
        _characterUpdater.CharactersUpdate();
        _bulletUpdater.BulletsUpdate();
        _checker.CheckCollisions();
    }
}
