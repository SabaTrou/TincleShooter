using MessagePipe;
using System.Collections.Generic;
using VContainer;
using VContainer.Unity;
using UnityEngine;

public class CharacterSeter:IInitializable
{
    #region
    //ペアリングされていないキャラクターコントローラのQueue
    private Queue<CharacterController> _unpairedControllers = new();
    private int _pairedControllerConut = default;
    private int _pairedCharacterCount = default;
    //ペアリングされていないキャラクターのQueue
    private Queue<PlayerCharacter> _unPairedCharacters = new();

    private ISubscriber<ControllerAddEvent> _controllerSubscriber;

    private ISubscriber<AiLogicAddEvent> _aiAddEvent;

    private ISubscriber<CharacterAddEvent> _characterSubscriber;
    #region Publisher
    [Inject] private IPublisher<AiLogicRequestEvent> _aiRequestEvent;
    #endregion
    #endregion
    #region DI
    [Inject]//Controllerが追加された時のイベント
    private void ControllerAdd(ISubscriber<ControllerAddEvent> subscriber)
    {
        _controllerSubscriber = subscriber;
        _controllerSubscriber.Subscribe(OnControllerAdded);

    }

    [Inject]
    private void AiLogicAdd(ISubscriber<AiLogicAddEvent> subscriber)
    {
        _aiAddEvent = subscriber;
        _aiAddEvent.Subscribe(OnAiLogicAdded);
    }

    [Inject]//Characterが追加された時のイベント
    private void CharacterAdd(ISubscriber<CharacterAddEvent> subscriber)
    {
        _characterSubscriber = subscriber;
        _characterSubscriber.Subscribe(OnCharacterAdded);


    }
    /// <summary>
    /// キャラクター追加時
    /// </summary>
    /// <param name="addEvent"></param>
    private void OnCharacterAdded(CharacterAddEvent addEvent)
    {
        AddObserveCharacter(addEvent.character);

    }
    /// <summary>
    /// キャラクターを追加
    /// </summary>
    /// <param name="character"></param>
    private void AddObserveCharacter(BaseCharacter character)
    {

        //受け取ったキャラクターがPlayerCharacterでなければ
        if (!(character is PlayerCharacter player))
        {
            return;
        }
        
        //ペアリングできるか
        if (!TryPairUpCharacters(out CharacterController controller))
        {
            
            //できなければ未接続のキャラクターのQueueに入れる
            _unPairedCharacters.Enqueue(player);
            //ペアリングしているコントローラが0より多ければ
            if (_pairedControllerConut > 0)
            {
                
                //Aiの生成要求
                _aiRequestEvent.Publish(new AiLogicRequestEvent());
            }
            return;
        }
        
        //出来たらペアリング
        controller.SetCharacter(player);
        player.SetPlayerFlag();
        _pairedCharacterCount++;
        _pairedControllerConut++;
    }
    #endregion
    #region
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

        
        //ペアリングできるか
        if (!TryPairUpCharacter(out PlayerCharacter character))
        {
            _unpairedControllers.Enqueue(controller);
            return;
        }
        //出来たらペアリング
        controller.SetCharacter(character);
        character.SetPlayerFlag();
        _pairedControllerConut++;
        _pairedCharacterCount++;
    }

    /// <summary>
    /// Ai追加時
    /// </summary>
    /// <param name="addEvent"></param>
    private void OnAiLogicAdded(AiLogicAddEvent addEvent)
    {
        AddObserveAiLogic(addEvent.logic);
    }

    /// <summary>
    /// 書き直したほうがいい
    /// </summary>
    /// <param name="logic"></param>
    private void AddObserveAiLogic(AiCharacterLogic logic)
    {
        if (!TryPairUpCharacter(out PlayerCharacter character))
        {
            return;
        }
        //ペアリングしているコントローラが0より多ければ
        if (_pairedControllerConut > 0)
        {
            logic.SetCharacter(character);
        }
        return;

    }
    #endregion
    #region TryPairUp ほぼ共通処理なのでまとめていいかも
    private bool TryPairUpCharacter(out PlayerCharacter character)
    {
        bool isGet = false;
        if (_unPairedCharacters.Count > 0)
        {
            isGet = true;
            character = _unPairedCharacters.Dequeue();
            return isGet;
        }
        character = null;
        return isGet;
    }

    private bool TryPairUpCharacters(out CharacterController controller)
    {
        bool isGet = false;
        if (_unpairedControllers.Count > 0)
        {
            isGet = true;
            controller = _unpairedControllers.Dequeue();
            return isGet;
        }
        controller = null;
        return isGet;
    }
    #endregion
    void IInitializable.Initialize()
    {

    }
}
