using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using MessagePipe;
using VContainer.Unity;

/// <summary>
/// �L�����N�^�[�Ǘ��N���X
/// </summary>
public class CharacterUpdater
{
    #region �ϐ�
    //�y�A�����O���ꂽ�L���X�^�[�̃��X�g
    private List<PlayerCharacter> _pairedCharacters = new();
    

    //�L�����N�^�[�̃��X�g
    private List<BaseCharacter> _characters = new();

    
    private List<AiCharacterLogic> _logics = new();
    #region �v���p�e�B
    public List<BaseCharacter> Charaters { get => _characters; }//�v���p�e�B
    #endregion
    #region messagePipe
    #region Subscriber
    private ISubscriber<CharacterAddEvent> _characterSubscriber;
    private ISubscriber<AiLogicAddEvent> _logicAddEvent;
    #endregion
   
    #endregion
    #endregion
    #region DI
    [Inject]//Character���ǉ����ꂽ���̃C�x���g
    private void CharacterAdd(ISubscriber<CharacterAddEvent> subscriber)
    {
        _characterSubscriber = subscriber;
        _characterSubscriber.Subscribe(OnCharacterAdded);


    }
    [Inject]
    private void AiLogicAdd(ISubscriber<AiLogicAddEvent> subscriber)
    {
        _logicAddEvent = subscriber;
        _logicAddEvent.Subscribe(OnAddAiLogic);
    }
    

   
    #endregion
    #region �Ǘ��n��
    /// <summary>
    /// �y�A�����O���ꂽ�L�����N�^�[�ƃR���g���[���̏�ԍX�V
    /// </summary>
    public void CharactersUpdate()
    {


       
        foreach (BaseCharacter character in _characters)
        {
            character.CharacterUpdate();
        }
        foreach(AiCharacterLogic logic in _logics)
        {
            logic.UpdateLogic();
        }




    }
    #region �ǉ��n�C�x���g
    /// <summary>
    /// �L�����N�^�[�ǉ���
    /// </summary>
    /// <param name="addEvent"></param>
    private void OnCharacterAdded(CharacterAddEvent addEvent)
    {
        AddObserveCharacter(addEvent.character);

    }
    private void OnAddAiLogic(AiLogicAddEvent addEvent)
    {
        AddObserveAiLogic(addEvent.logic);
    }
   
    #endregion
    /// <summary>
    /// �L�����N�^�[��ǉ�
    /// </summary>
    /// <param name="character"></param>
    private void AddObserveCharacter(BaseCharacter character)
    {
        //���X�g�ɒǉ�
        _characters.Add(character);
       
       
        
    }
    private void AddObserveAiLogic(AiCharacterLogic logic)
    {
        _logics.Add(logic);
    }
   
    
    

    #endregion
}
