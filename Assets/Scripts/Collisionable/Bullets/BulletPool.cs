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
    //hash��list�Ńy�A�����O���Ă�Dictionary
    private Dictionary<BulletPrefabValue, List<BaseBullet>> _bulletsDic = new();
    //hash�Ɣ��ˉ\�Ȓe�Ńy�A�����ODictionary
    private Dictionary<BulletPrefabValue, Queue<BaseBullet>> _canFireBulletsQueues = new();
    //���u���ꏊ
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
        //�n�����߂̒e
        BaseBullet bullet;
        //�e������Γn��
       if(TryGetShotableBullet(bulletPrefab,out bullet))
        {
            return bullet;
        }
       //�Ȃ�������X�V
        UpdateShotableBullet(bulletPrefab);
        //�e������Γn��
        if(TryGetShotableBullet(bulletPrefab, out bullet))
        {
            return bullet;
        }
        //�Ȃ�������ǉ�
        _instantiateRequestEvent.Publish(new(bulletPrefab, _poolPosition));
        //�e������Γn��
        if (TryGetShotableBullet(bulletPrefab, out bullet))
        {
            return bullet;
        }
        Debug.LogError("�ʒm�悪�Ȃ�");
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
    /// ���ˉ\�Ȓe��Queue���X�V
    /// </summary>
    /// <param name="key"></param>
    private void UpdateShotableBullet(BulletPrefabValue key)
    {
        
        foreach (BaseBullet bullet in _bulletsDic[key])
        {
            //���˂���Ă��Ȃ���Βǉ�
            if (bullet.IsShot)
            {
                continue;
            }
            _canFireBulletsQueues[key].Enqueue(bullet);
        }

    }
    /// <summary>
    /// key���o�^����Ă��邩�m�F
    /// </summary>
    /// <param name="prefabValue"></param>
    private void CheckExistKey(BulletPrefabValue prefabValue)
    {
        //key���o�^����Ă��邩
        if (!_bulletsDic.ContainsKey(prefabValue))
        {
            //�Ȃ���Γo�^
            _bulletsDic[prefabValue] = new();
            _canFireBulletsQueues[prefabValue] = new();
        }
        return;
    }
}
