using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;
using MessagePipe;


public class ControllerProvider:IStartable
{
    private IPublisher<ControllerAddEvent> _addEvent;
    [Inject]
    private void Construct(IPublisher<ControllerAddEvent> publisher)
    {
        _addEvent = publisher;
    }
    void IStartable.Start()
    {
        
        if(TryGetConnectedGamePads(out Gamepad[] gamepads))
        {
            foreach(Gamepad gamepad in gamepads)
            {
                RegisterController(gamepad);
            }
        }
        else
        {
            RegisterController();
        }
        InputSystem.onDeviceChange += OnDeviceChange;
    }
    
    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        
        // 新しいゲームパッドが接続されたとき
        if (change == InputDeviceChange.Added && device is Gamepad gamepad)
        {
            
            RegisterController(gamepad);
        }
    }

    private void RegisterController(Gamepad gamepad)
    {
        CharacterController controller = new();
        controller.SetGamePad(gamepad);
        _addEvent.Publish(new ControllerAddEvent(controller));
    }
    private void RegisterController()
    {
        _addEvent.Publish(new ControllerAddEvent(new CharacterController()));
       
    }
    private bool TryGetConnectedGamePads(out Gamepad[] gamepads)
    {
        gamepads=Gamepad.all.ToArray();
        if (gamepads.Length>0)
        {
            return true;
        }
            return false; 
    }
}
