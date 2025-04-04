using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using MessagePipe;
using UnityEngine.AddressableAssets;

/// <summary>
/// 弾生成用クラス
/// </summary>
public class BulletInstantiateService :IInitializable
{
    
    //messagePipe
    IPublisher<BulletAddEvent> _addEvent;
    ISubscriber<BulletInstantiateRequestEvent> _reqestEvent;
    //
   

   
    void IInitializable.Initialize()
    {
        
        Debug.Log("Init");
    }
    #region DI
    [Inject]
    private void BulletAdd(IPublisher<BulletAddEvent> publisher)
    {
        _addEvent = publisher;
    }
    [Inject]
    private void BulletInstantiateReqest(ISubscriber<BulletInstantiateRequestEvent> subscriber)
    {
        _reqestEvent = subscriber;
        _reqestEvent.Subscribe(BulletInstantiate);

    }
    #endregion
   
    /// <summary>
    /// 弾生成
    /// </summary>
    /// <param name="reqestevent"></param>
    private void BulletInstantiate(BulletInstantiateRequestEvent reqestevent)
    {
        
        //instantiate
        GameObject instance= MonoBehaviour.Instantiate(
           reqestevent.prefabValue.prefabObject,
            reqestevent.Position,
            Quaternion.identity          
            );

        //BaseBulletがついていれば弾の追加を通知
        if(instance.TryGetComponent<BaseBullet>(out BaseBullet bullet))
        {
            _addEvent.Publish(new BulletAddEvent(bullet,reqestevent.prefabValue));
        }
        else//例外処理
        {
            Debug.LogError("BaseBulletがついていない");
            return;
        }
    }

   
}
