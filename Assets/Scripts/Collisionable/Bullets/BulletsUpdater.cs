using MessagePipe;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;
using VContainer;
using System.Linq;

/// <summary>
/// 弾管理クラス
/// </summary>
public class BulletsUpdater
{
    #region　変数
    public List<BaseBullet> baseBullets { get => _baseBullets; }//プロパティ
    //弾のリスト
    private List<BaseBullet> _baseBullets = new();
    //messagePipe
    private ISubscriber<BulletAddEvent> _bulletAddEvent;
    //
    #endregion
    #region DI       
    //DI
    [Inject]
    private void BulletAdd(ISubscriber<BulletAddEvent> subscriber)
    {
        _bulletAddEvent = subscriber;
        _bulletAddEvent.Subscribe(OnAddedBullet);

    }
    #endregion
    #region Update
    public void BulletsUpdate()
    {
        foreach (BaseBullet bullet in _baseBullets)
        {
            bullet.BulletUpdate();
        }
    }
    #endregion
    /// <summary>
    /// 弾の追加通知が来たタイミングで呼ばれる
    /// </summary>
    /// <param name="addEvent"></param>
    private void OnAddedBullet(BulletAddEvent addEvent)
    {
        //管理対象に追加
        addObserveBullet(addEvent.baseBullet);
        
    }
    //管理する弾を追加
    private void addObserveBullet(BaseBullet bullet)
    {
        _baseBullets.Add(bullet);
    }
    
   

}
