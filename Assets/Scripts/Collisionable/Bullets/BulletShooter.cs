using System;
using VContainer;
using MessagePipe;
using UnityEngine;
using VContainer.Unity;

public class BulletShooter:IInitializable
{
    #region　変数
    private ISubscriber<BulletShotRequestEvent> _shotRequest;
    [Inject] BulletPool _bulletPool;
    #endregion
    #region DI
    [Inject]
    private void BulletShotRequest(ISubscriber<BulletShotRequestEvent> subscriber)
    {
        _shotRequest = subscriber;
        _shotRequest.Subscribe(OnShotRequest);
    }
    #endregion
    #region Shot
    private void OnShotRequest(BulletShotRequestEvent requestEvent)
    {
        BaseBullet bullet= _bulletPool.GetShotableBullet(requestEvent.prefabValue);
        Shot(bullet, requestEvent);
    }
    private void Shot(BaseBullet bullet,BulletShotRequestEvent requestEvent)
    {
        bullet.ShotBullet(requestEvent.position, requestEvent.shotPlayer, requestEvent.bulletSpeed, requestEvent.damage);
    }
    #endregion
    void IInitializable.Initialize()
    {

    }
}