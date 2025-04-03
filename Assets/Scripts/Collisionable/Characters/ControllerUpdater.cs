using System;
using VContainer;
using MessagePipe;
using System.Collections.Generic;

public class ControllerUpdater
{
    #region　変数
    
    //ペアリングされたキャラクターコントローラのリスト
    private List<CharacterController> _controllers = new();

    private ISubscriber<ControllerAddEvent> _controllerSubscriber;
    #endregion
    #region DI
    [Inject]//Controllerが追加された時のイベント
    private void ControllerAdd(ISubscriber<ControllerAddEvent> subscriber)
    {
        _controllerSubscriber = subscriber;
        _controllerSubscriber.Subscribe(OnControllerAdded);

    }
    #endregion
    /// <summary>
    /// コントローラ追加時
    /// </summary>
    /// <param name="addEvent"></param>
    private void OnControllerAdded(ControllerAddEvent addEvent)
    {
        AddObserveController(addEvent.controller);


    }
    /// <summary>
    /// コントローラーを追加
    /// </summary>
    /// <param name="controller"></param>
    private void AddObserveController(CharacterController controller)
    {
        _controllers.Add(controller);
    }
    public void ControllersUpdate()
    {
        foreach (CharacterController controller in _controllers)
        {
            controller.ControllerUpdate();


        }
    }
}