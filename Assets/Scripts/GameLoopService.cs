using MessagePipe;
using VContainer;
using VContainer.Unity;

public class GameLoopService:IInitializable
{
    #region Values
    [Inject]private SceneChanger _sceneChanger;

    private ISubscriber<PlayerDeadEvent> _playerDeadEvent;
    #endregion
    #region DIMethod
    [Inject]
    private void PlayerDead(ISubscriber<PlayerDeadEvent> subscriber)
    {
        _playerDeadEvent = subscriber;
        _playerDeadEvent.Subscribe(OnPlayerDead);
    }
    #endregion

    
    private void OnPlayerDead(PlayerDeadEvent deadEvent)
    {
        if(deadEvent.player.IsPlayer)
        {
            _sceneChanger.LoadGameOverScene();
            return;
        }

        _sceneChanger.LoadClearScene();
        return;
    }
    void IInitializable.Initialize()
    {

    }

    
}
