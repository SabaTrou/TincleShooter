using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using MessagePipe;

/// <summary>
/// キャラクター生成用クラス
/// </summary>
public class CharacterInstantiateService:IInitializable
{
    #region 変数
    //messagePipe
    [Inject]private ISubscriber<CharacterInstantiateRequestEvent> _instantiateReqest;
    [Inject]private IPublisher<CharacterAddEvent> _addEvent;
    [Inject]private IObjectResolver _container;
    //
    #endregion
    #region DI
    /// <summary>
    /// リクエストをDI
    /// </summary>
    /// <param name="subscriber"></param>
    [Inject]
    public void CharacterInstantiateReqest(ISubscriber<CharacterInstantiateRequestEvent> subscriber)
    {
        _instantiateReqest = subscriber;
        _instantiateReqest.Subscribe(CharacterInsantiate);
       
    }
    /// <summary>
    /// 追加通知をDI
    /// </summary>
    /// <param name="publisher"></param>
    [Inject]
    public void CharacterAdd(IPublisher<CharacterAddEvent> publisher)
    {
        _addEvent = publisher;
        
    }
    #endregion
    //インスタンス化するため
    void IInitializable.Initialize()
    {

    }

    /// <summary>
    /// キャラクターを生成して、通知する
    /// </summary>
    /// <param name="reqest"></param>
    private void CharacterInsantiate(CharacterInstantiateRequestEvent reqest)
    {
        //instantiate
        GameObject instance = MonoBehaviour.Instantiate(reqest.characterPrefab, reqest.Position, Quaternion.identity,reqest.parent);
        
        //BaseCharacterがついていなければ例外を返す
        if(!instance.TryGetComponent<BaseCharacter>(out BaseCharacter character))
        {
            Debug.LogError("BaseCharacterがついていない");
            return;
        }
       //コンテナに登録(characterから通知を飛ばせるようにするため)
        _container.Inject(character);
        //キャラクターの追加を通知
        _addEvent.Publish(new CharacterAddEvent(character));
        
    }
}
