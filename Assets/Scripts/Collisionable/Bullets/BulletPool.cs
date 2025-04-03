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
    //hash‚Ælist‚ÅƒyƒAƒŠƒ“ƒO‚µ‚Ä‚éDictionary
    private Dictionary<BulletPrefabValue, List<BaseBullet>> _bulletsDic = new();
    //hash‚Æ”­Ë‰Â”\‚È’e‚ÅƒyƒAƒŠƒ“ƒODictionary
    private Dictionary<BulletPrefabValue, Queue<BaseBullet>> _canFireBulletsQueues = new();
    //‰¼’u‚«êŠ
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
        //“n‚·‚½‚ß‚Ì’e
        BaseBullet bullet;
        //’e‚ª‚ ‚ê‚Î“n‚·
       if(TryGetShotableBullet(bulletPrefab,out bullet))
        {
            return bullet;
        }
       //‚È‚©‚Á‚½‚çXV
        UpdateShotableBullet(bulletPrefab);
        //’e‚ª‚ ‚ê‚Î“n‚·
        if(TryGetShotableBullet(bulletPrefab, out bullet))
        {
            return bullet;
        }
        //‚È‚©‚Á‚½‚ç’Ç‰Á
        _instantiateRequestEvent.Publish(new(bulletPrefab, _poolPosition));
        //’e‚ª‚ ‚ê‚Î“n‚·
        if (TryGetShotableBullet(bulletPrefab, out bullet))
        {
            return bullet;
        }
        Debug.LogError("’Ê’mæ‚ª‚È‚¢");
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
    /// ”­Ë‰Â”\‚È’e‚ÌQueue‚ğXV
    /// </summary>
    /// <param name="key"></param>
    private void UpdateShotableBullet(BulletPrefabValue key)
    {
        
        foreach (BaseBullet bullet in _bulletsDic[key])
        {
            //”­Ë‚³‚ê‚Ä‚¢‚È‚¯‚ê‚Î’Ç‰Á
            if (bullet.IsShot)
            {
                continue;
            }
            _canFireBulletsQueues[key].Enqueue(bullet);
        }

    }
    /// <summary>
    /// key‚ª“o˜^‚³‚ê‚Ä‚¢‚é‚©Šm”F
    /// </summary>
    /// <param name="prefabValue"></param>
    private void CheckExistKey(BulletPrefabValue prefabValue)
    {
        //key‚ª“o˜^‚³‚ê‚Ä‚¢‚é‚©
        if (!_bulletsDic.ContainsKey(prefabValue))
        {
            //‚È‚¯‚ê‚Î“o˜^
            _bulletsDic[prefabValue] = new();
            _canFireBulletsQueues[prefabValue] = new();
        }
        return;
    }
}
