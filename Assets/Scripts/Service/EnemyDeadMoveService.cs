using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;
using MessagePipe;
using VContainer;

public class EnemyDeadMoveService:IInitializable
{
    private ISubscriber<EnemyDeadEvent> _deadEvent;
    //vector3ÇÕíËêîÇ…Ç≈Ç´Ç»Ç¢ÇÃÇ≈readonly private
   readonly private Vector3 _deadPosition = new Vector3(0,100,0);
    [Inject]
    private void EnemyDead(ISubscriber<EnemyDeadEvent> subscriber)
    {
        _deadEvent = subscriber;
        _deadEvent.Subscribe(MoveDeadPosition);
    }
    //instance
    void IInitializable.Initialize()
    {

    }
    private void MoveDeadPosition(EnemyDeadEvent deadEvent)
    {
        deadEvent.enemyCharacter.transform.position = _deadPosition;
        
    }
}
