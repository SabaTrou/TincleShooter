using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem;
using VContainer;
using MessagePipe;
using VContainer.Unity;

public class CharacterController
{
   
    private Dictionary<ControllerEnum,ButtonControl > _keyConfigDic = new();
    private bool _isCharacterSet = false;
    private BaseCharacter _controllCharacter;
    private Gamepad _gamepad;
   
    
   
    //コントローラーのアップデート
    public void ControllerUpdate()
    {
        if(!_isCharacterSet)
        {
            Debug.LogError("Character is Null");
            return;
        }
        
        if(_gamepad!=null)
        {
            GamePadControll();
        }
        else
        {
            KeyBordControll();
        }

        
        
    }
    //ゲームパッドが接続されているとき
    private void GamePadControll()
    {
        _controllCharacter.Move(_gamepad.leftStick.value);
        if(_gamepad.bButton.isPressed)
        {
            _controllCharacter.Attack();
        }
    }
    //ゲームパッドが接続されていないとき
    private void KeyBordControll()
    {
       
        Vector2 keybordMoveValue = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _controllCharacter.Move((keybordMoveValue).normalized);
        if (Input.GetKey(KeyCode.Space))
        {
            _controllCharacter.Attack();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            _controllCharacter.Bomb();
        }
    }
    //キャラクターを受け取る
    public void SetCharacter(BaseCharacter character)
    {
        _isCharacterSet = true;
        _controllCharacter = character;
        
    }
    public void SetGamePad(Gamepad gamepad)
    {
        _gamepad = gamepad;
    }

    private void SetKeyConfig(ControllerEnum key)
    {
        
        switch (key)
        {
            case ControllerEnum.DpadTop:
                {
                    _keyConfigDic[key] = ControllerInput.Gamepad.dpad.up;
                    return;
                }
            case ControllerEnum.DpadLeft:
                {
                    _keyConfigDic[key] = ControllerInput.Gamepad.dpad.left;
                    return;
                }
            case ControllerEnum.DpadRight:
                {
                    _keyConfigDic[key] = ControllerInput.Gamepad.dpad.right;
                    return;
                }
            case ControllerEnum.DpadButtom:
                {
                    _keyConfigDic[key] = ControllerInput.Gamepad.dpad.down;
                    return;
                }
            case ControllerEnum.TggleRightStick:
                {
                    _keyConfigDic[key] = ControllerInput.Gamepad.rightStickButton;
                    return;
                }
            case ControllerEnum.ToggleLeftStick:
                {
                    _keyConfigDic[key] = ControllerInput.Gamepad.leftStickButton;
                    return;
                }
            case ControllerEnum.ShoulderRiht:
                {
                    _keyConfigDic[key] = ControllerInput.Gamepad.rightShoulder;
                    return;
                }
            case ControllerEnum.ShoulderLeft:
                {
                    _keyConfigDic[key] = ControllerInput.Gamepad.leftShoulder;
                    return;
                }

        }

    }
}

public enum ControllerEnum
{
    DpadTop,
    DpadLeft,
    DpadRight,
    DpadButtom,
    ToggleLeftStick,
    TggleRightStick,
    ShoulderLeft,
    ShoulderRiht,
    TriggerLeft,
    TriggerRight
}

