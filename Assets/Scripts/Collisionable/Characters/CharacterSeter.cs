using MessagePipe;
using System.Collections.Generic;
using VContainer;
using VContainer.Unity;
using UnityEngine;

public class CharacterSeter:IInitializable
{
    #region
    //�y�A�����O����Ă��Ȃ��L�����N�^�[�R���g���[����Queue
    private Queue<CharacterController> _unpairedControllers = new();
    private int _pairedControllerConut = default;
    private int _pairedCharacterCount = default;
    //�y�A�����O����Ă��Ȃ��L�����N�^�[��Queue
    private Queue<PlayerCharacter> _unPairedCharacters = new();

    private ISubscriber<ControllerAddEvent> _controllerSubscriber;

    private ISubscriber<AiLogicAddEvent> _aiAddEvent;

    private ISubscriber<CharacterAddEvent> _characterSubscriber;
    #region Publisher
    [Inject] private IPublisher<AiLogicRequestEvent> _aiRequestEvent;
    #endregion
    #endregion
    #region DI
    [Inject]//Controller���ǉ����ꂽ���̃C�x���g
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

    [Inject]//Character���ǉ����ꂽ���̃C�x���g
    private void CharacterAdd(ISubscriber<CharacterAddEvent> subscriber)
    {
        _characterSubscriber = subscriber;
        _characterSubscriber.Subscribe(OnCharacterAdded);


    }
    /// <summary>
    /// �L�����N�^�[�ǉ���
    /// </summary>
    /// <param name="addEvent"></param>
    private void OnCharacterAdded(CharacterAddEvent addEvent)
    {
        AddObserveCharacter(addEvent.character);

    }
    /// <summary>
    /// �L�����N�^�[��ǉ�
    /// </summary>
    /// <param name="character"></param>
    private void AddObserveCharacter(BaseCharacter character)
    {

        //�󂯎�����L�����N�^�[��PlayerCharacter�łȂ����
        if (!(character is PlayerCharacter player))
        {
            return;
        }
        
        //�y�A�����O�ł��邩
        if (!TryPairUpCharacters(out CharacterController controller))
        {
            
            //�ł��Ȃ���Ζ��ڑ��̃L�����N�^�[��Queue�ɓ����
            _unPairedCharacters.Enqueue(player);
            //�y�A�����O���Ă���R���g���[����0��葽�����
            if (_pairedControllerConut > 0)
            {
                
                //Ai�̐����v��
                _aiRequestEvent.Publish(new AiLogicRequestEvent());
            }
            return;
        }
        
        //�o������y�A�����O
        controller.SetCharacter(player);
        player.SetPlayerFlag();
        _pairedCharacterCount++;
        _pairedControllerConut++;
    }
    #endregion
    #region
    /// <summary>
    /// �R���g���[���ǉ���
    /// </summary>
    /// <param name="addEvent"></param>
    private void OnControllerAdded(ControllerAddEvent addEvent)
    {
        AddObserveController(addEvent.controller);


    }
    /// <summary>
    /// �R���g���[���[��ǉ�
    /// </summary>
    /// <param name="controller"></param>
    private void AddObserveController(CharacterController controller)
    {

        
        //�y�A�����O�ł��邩
        if (!TryPairUpCharacter(out PlayerCharacter character))
        {
            _unpairedControllers.Enqueue(controller);
            return;
        }
        //�o������y�A�����O
        controller.SetCharacter(character);
        character.SetPlayerFlag();
        _pairedControllerConut++;
        _pairedCharacterCount++;
    }

    /// <summary>
    /// Ai�ǉ���
    /// </summary>
    /// <param name="addEvent"></param>
    private void OnAiLogicAdded(AiLogicAddEvent addEvent)
    {
        AddObserveAiLogic(addEvent.logic);
    }

    /// <summary>
    /// �����������ق�������
    /// </summary>
    /// <param name="logic"></param>
    private void AddObserveAiLogic(AiCharacterLogic logic)
    {
        if (!TryPairUpCharacter(out PlayerCharacter character))
        {
            return;
        }
        //�y�A�����O���Ă���R���g���[����0��葽�����
        if (_pairedControllerConut > 0)
        {
            logic.SetCharacter(character);
        }
        return;

    }
    #endregion
    #region TryPairUp �قڋ��ʏ����Ȃ̂ł܂Ƃ߂Ă�������
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
