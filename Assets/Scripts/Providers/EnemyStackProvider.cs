using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePipe;
using VContainer;
using VContainer.Unity;

public class EnemyStackProvider:IInitializable
{
    private ISubscriber<EnemyDeadEvent> _deadEvent;
    [Inject]
    private IPublisher<EnemyReSycleEvent> _ReSycleEvent;

    [Inject]
    private void EnemyDead(ISubscriber<EnemyDeadEvent> subscriber)
    {
        _deadEvent = subscriber;
        _deadEvent.Subscribe(ConvertAndPublish);
    }
    private void ConvertAndPublish(EnemyDeadEvent enemyDead)
    {
        _ReSycleEvent.Publish(new EnemyReSycleEvent(enemyDead.enemyCharacter,new Vector3(0,100,0)));
    }
    
    void IInitializable.Initialize()
    {

    }

}
