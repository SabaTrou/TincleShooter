using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;
using VContainer;
using MessagePipe;

public class ScoreProvider :IInitializable
{
    [Inject] private IPublisher<ScoreAddEvent> _scorePublisher;
    private ISubscriber<EnemyDeadEvent> _enemyDeadEvent;
    [Inject]
    private void EnemyDead(ISubscriber<EnemyDeadEvent> subscriber)
    {
        _enemyDeadEvent = subscriber;
        _enemyDeadEvent.Subscribe(OnEneyDead);
    }
    private void OnEneyDead(EnemyDeadEvent deadEvent)
    {
       
        _scorePublisher.Publish(new ScoreAddEvent(deadEvent.player,deadEvent.enemyCharacter.EnemyScore));
    }
    /// <summary>
    /// instance‰»—p
    /// </summary>
   void IInitializable.Initialize()
    {

    }
}
