using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using MessagePipe;

/// <summary>
/// �L�����N�^�[�����p�N���X
/// </summary>
public class CharacterInstantiateService:IInitializable
{
    #region �ϐ�
    //messagePipe
    [Inject]private ISubscriber<CharacterInstantiateRequestEvent> _instantiateReqest;
    [Inject]private IPublisher<CharacterAddEvent> _addEvent;
    [Inject]private IObjectResolver _container;
    //
    #endregion
    #region DI
    /// <summary>
    /// ���N�G�X�g��DI
    /// </summary>
    /// <param name="subscriber"></param>
    [Inject]
    public void CharacterInstantiateReqest(ISubscriber<CharacterInstantiateRequestEvent> subscriber)
    {
        _instantiateReqest = subscriber;
        _instantiateReqest.Subscribe(CharacterInsantiate);
       
    }
    /// <summary>
    /// �ǉ��ʒm��DI
    /// </summary>
    /// <param name="publisher"></param>
    [Inject]
    public void CharacterAdd(IPublisher<CharacterAddEvent> publisher)
    {
        _addEvent = publisher;
        
    }
    #endregion
    //�C���X�^���X�����邽��
    void IInitializable.Initialize()
    {

    }

    /// <summary>
    /// �L�����N�^�[�𐶐����āA�ʒm����
    /// </summary>
    /// <param name="reqest"></param>
    private void CharacterInsantiate(CharacterInstantiateRequestEvent reqest)
    {
        //instantiate
        GameObject instance = MonoBehaviour.Instantiate(reqest.characterPrefab, reqest.Position, Quaternion.identity,reqest.parent);
        
        //BaseCharacter�����Ă��Ȃ���Η�O��Ԃ�
        if(!instance.TryGetComponent<BaseCharacter>(out BaseCharacter character))
        {
            Debug.LogError("BaseCharacter�����Ă��Ȃ�");
            return;
        }
       //�R���e�i�ɓo�^(character����ʒm���΂���悤�ɂ��邽��)
        _container.Inject(character);
        //�L�����N�^�[�̒ǉ���ʒm
        _addEvent.Publish(new CharacterAddEvent(character));
        
    }
}
