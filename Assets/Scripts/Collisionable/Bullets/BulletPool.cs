using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using MessagePipe;

public class BulletPool
{
    #region 
    private ISubscriber<BulletAddEvent> _addEvent;
    [Inject] private IPublisher<BulletInstantiateRequestEvent> _instantiateRequestEvent;
    //hashとlistでペアリングしてるDictionary
    private Dictionary<BulletPrefabValue, List<BaseBullet>> _bulletsDic = new();
    //hashと発射可能な弾でペアリングDictionary
    private Dictionary<BulletPrefabValue, Queue<BaseBullet>> _canFireBulletsQueues = new();
    //仮置き場所
    private Vector3 _poolPosition = new(0,1000,0);
    #endregion
    #region DI
    [Inject]
    private void BulletAdd(ISubscriber<BulletAddEvent> subscriber)
    {
        _addEvent = subscriber;
        _addEvent.Subscribe(OnBulletAdded);
    }
    #endregion
    #region 
    /// <summary>
    /// 
    /// </summary>
    /// <param name="addEvent"></param>
    private void OnBulletAdded(BulletAddEvent addEvent)
    {
        _bulletsDic[addEvent.prefabValue].Add(addEvent.baseBullet) ;
        _canFireBulletsQueues[addEvent.prefabValue].Enqueue(addEvent.baseBullet);
    }
    #endregion
    public BaseBullet GetShotableBullet(BulletPrefabValue bulletPrefab)
    {
        
        CheckExistKey(bulletPrefab);
        //渡すための弾
        BaseBullet bullet;
        //弾があれば渡す
       if(TryGetShotableBullet(bulletPrefab,out bullet))
        {
            return bullet;
        }
       //なかったら更新
        UpdateShotableBullet(bulletPrefab);
        //弾があれば渡す
        if(TryGetShotableBullet(bulletPrefab, out bullet))
        {
            return bullet;
        }
        //なかったら追加
        _instantiateRequestEvent.Publish(new(bulletPrefab, _poolPosition));
        //弾があれば渡す
        if (TryGetShotableBullet(bulletPrefab, out bullet))
        {
            return bullet;
        }
        Debug.LogError("通知先がない");
        return null;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bulletPrefab"></param>
    /// <param name="bullet"></param>
    /// <returns></returns>
    private bool TryGetShotableBullet(BulletPrefabValue bulletPrefab,out BaseBullet bullet)
    {
        bullet = null;
        if (_canFireBulletsQueues[bulletPrefab].Count <= 0)
        {
            return false;
        }
        bullet = _canFireBulletsQueues[bulletPrefab].Dequeue(); ;
        return true;
    }

    /// <summary>
    /// 発射可能な弾のQueueを更新
    /// </summary>
    /// <param name="key"></param>
    private void UpdateShotableBullet(BulletPrefabValue key)
    {
        
        foreach (BaseBullet bullet in _bulletsDic[key])
        {
            //発射されていなければ追加
            if (bullet.IsShot)
            {
                continue;
            }
            _canFireBulletsQueues[key].Enqueue(bullet);
        }

    }
    /// <summary>
    /// keyが登録されているか確認
    /// </summary>
    /// <param name="prefabValue"></param>
    private void CheckExistKey(BulletPrefabValue prefabValue)
    {
        //keyが登録されているか
        if (!_bulletsDic.ContainsKey(prefabValue))
        {
            //なければ登録
            _bulletsDic[prefabValue] = new();
            _canFireBulletsQueues[prefabValue] = new();
        }
        return;
    }
}
