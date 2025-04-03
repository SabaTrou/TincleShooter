using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePipe;
using VContainer.Unity;
using VContainer;

/// <summary>
/// 中継クラス
/// </summary>
public class CollisionableProvider:IInitializable
{

    private ISubscriber<BulletAddEvent> _bulletAddEvent;
    private ISubscriber<CharacterAddEvent> _characterAddEvent;
    [Inject]private IPublisher<CollisionableAddEvent> _collisionableAddEvent;
    //インスタンス化するため
    void IInitializable.Initialize()
    {

    }
    [Inject]
    private void BulletAdd(ISubscriber<BulletAddEvent> subscriber)
    {
        _bulletAddEvent = subscriber;
        _bulletAddEvent.Subscribe(OnBulletAdded);
    }
    [Inject]
    private void CharacterAdd(ISubscriber<CharacterAddEvent> subscriber)
    {
        _characterAddEvent = subscriber;
        _characterAddEvent.Subscribe(OnCharacterAdded);
    }
    //
    private void OnCharacterAdded(CharacterAddEvent addEvent)
    {
        OnCollisionableAdded(addEvent.character);
    }
    //
    private void OnBulletAdded(BulletAddEvent addEvent)
    {
        OnCollisionableAdded(addEvent.baseBullet);
    }
    //
    private void OnCollisionableAdded(ICollisionable collisionable)
    {
        _collisionableAddEvent.Publish(new CollisionableAddEvent(collisionable));
    }

}
