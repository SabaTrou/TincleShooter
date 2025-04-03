using MessagePipe;
using VContainer.Unity;
using VContainer;
using UnityEngine;


public class AiLogicCreater : IInitializable
{
    #region 変数
    #region messagePipe
    private ISubscriber<AiLogicRequestEvent> _requestEvent;
    [Inject]private IPublisher<AiLogicAddEvent> _addEvent;
    [Inject] private IObjectResolver _container;
    #endregion
    #endregion
    #region DI
    [Inject]
    private void AiLogicRequest(ISubscriber<AiLogicRequestEvent> subscriber)
    {
        _requestEvent = subscriber;
        _requestEvent.Subscribe(InstantiateAi);
    }
    #endregion
    #region instantiate
    #endregion
    //インスタンス化
    void IInitializable.Initialize()
    {

    }
    
    private void InstantiateAi(AiLogicRequestEvent requestEvent)
    {
        
        AiCharacterLogic instance = new AiCharacterLogic();
        _container.Inject(instance);
        _addEvent.Publish(new AiLogicAddEvent(instance));
    }
}

