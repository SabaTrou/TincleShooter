using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using MessagePipe;
using VContainer.Unity;

/// <summary>
/// キャラクター管理クラス
/// </summary>
public class CharacterUpdater
{
    #region 変数
    //ペアリングされたキャスターのリスト
    private List<PlayerCharacter> _pairedCharacters = new();
    

    //キャラクターのリスト
    private List<BaseCharacter> _characters = new();

    
    private List<AiCharacterLogic> _logics = new();
    #region プロパティ
    public List<BaseCharacter> Charaters { get => _characters; }//プロパティ
    #endregion
    #region messagePipe
    #region Subscriber
    private ISubscriber<CharacterAddEvent> _characterSubscriber;
    private ISubscriber<AiLogicAddEvent> _logicAddEvent;
    #endregion
   
    #endregion
    #endregion
    #region DI
    [Inject]//Characterが追加された時のイベント
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
    #region 管理系統
    /// <summary>
    /// ペアリングされたキャラクターとコントローラの状態更新
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
    #region 追加系イベント
    /// <summary>
    /// キャラクター追加時
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
    /// キャラクターを追加
    /// </summary>
    /// <param name="character"></param>
    private void AddObserveCharacter(BaseCharacter character)
    {
        //リストに追加
        _characters.Add(character);
       
       
        
    }
    private void AddObserveAiLogic(AiCharacterLogic logic)
    {
        _logics.Add(logic);
    }
   
    
    

    #endregion
}
