using MessagePipe;
using VContainer;
using VContainer.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Փx�Ǘ��p�N���X
/// </summary>
public class DifficultyManager:IStartable,ITickable
{
    #region �萔 �d�݂Â�
    //�_���[�W��H��������Փx�������邽�߃}�C�i�X
    private const float DAMAGE_WHEIGHT = -0.5f;
    private const float KILL_ENEMY_WEIGHT = 1f;
    #endregion
    #region �ϐ� 
    private Dictionary<PlayerCharacter, int> _playertakeDamageCountDic = new();
    private Dictionary<PlayerCharacter,int> _playerKillEnemyCountDic = new();
    private Dictionary<PlayerCharacter, int> _playerStageLevelDic=new();
    
    //�^�C�}�[�̊J�n�^�C�~���O
    private DateTime _startTime = default;
    //�I���̃^�C�~���O
    private DateTime _endTime = default;
    //��Փx�K�p�܂ł̎���(5s)
    private TimeSpan _difficultyUpdateTime = new(0,0,0,5,0);
    private int _logicDifficultyLevel = default;
    #region messagePipe
    private ISubscriber<EnemyDeadEvent> _enemyDeadEvent;
    private ISubscriber<PlayerTakeDamageEvent> _playerTakeDamageEvent;
    private ISubscriber<CharacterAddEvent> _characterAddEvent;
    [Inject] private IPublisher<StageDifficultySetEvent> _stageSetEvent;
    [Inject] private IPublisher<LogicDifficultySetEvent> _logicSetEvent;
    #endregion
    #endregion
    #region DI
    [Inject]
    private void EnemyDead(ISubscriber<EnemyDeadEvent> subscriber)
    {
        _enemyDeadEvent = subscriber;
        _enemyDeadEvent.Subscribe(OnEnemyDead);

    }
    [Inject]
    private void PlayerTakeDamage(ISubscriber<PlayerTakeDamageEvent> subscriber)
    {
        _playerTakeDamageEvent = subscriber;
        _playerTakeDamageEvent.Subscribe(OnPlayerTakeDamage);
    }
    [Inject]
    private void CharacterAdd(ISubscriber<CharacterAddEvent> subscriber)
    {
        _characterAddEvent = subscriber;
        _characterAddEvent.Subscribe(OnCharacterAdded);
    }
    private void OnCharacterAdded(CharacterAddEvent addEvent)
    {
        if(!(addEvent.character is PlayerCharacter player))
        {
            return;
        }
        _playerStageLevelDic[player] = 0;
    }
    #endregion
    //�C���X�^���X��
    void IStartable.Start()
    {
        _startTime = DateTime.Now;
         
    }
    void ITickable.Tick()
    {
        _endTime = DateTime.Now;
        if(_endTime-_startTime<_difficultyUpdateTime)
        {
            return;
        }
        //�v�Z
        foreach(PlayerCharacter player in _playerStageLevelDic.Keys)
        {
            int difficulty=CalculateCharacterDifficulty(player);
            _stageSetEvent.Publish(new StageDifficultySetEvent(difficulty,player));
        }
        _logicDifficultyLevel++;
        
        _startTime = DateTime.Now;

    }
    #region weight
    private void OnEnemyDead(EnemyDeadEvent deadEvent)
    {
        if(!_playerKillEnemyCountDic.ContainsKey(deadEvent.player))
        {
            _playerKillEnemyCountDic[deadEvent.player] = new();          
        }
        _playerKillEnemyCountDic[deadEvent.player]++;
    }
    private void OnPlayerTakeDamage(PlayerTakeDamageEvent damageEvent)
    {
        if(!_playertakeDamageCountDic.ContainsKey(damageEvent.playerCharacter))
        {
            _playertakeDamageCountDic[damageEvent.playerCharacter]=new();
        }
        _playertakeDamageCountDic[damageEvent.playerCharacter] ++ ;
    }
    #endregion
    #region ��Փx�v�Z
    private int CalculateCharacterDifficulty(PlayerCharacter player)
    {
        float difficulty = 0;
        difficulty = KILL_ENEMY_WEIGHT * _playerKillEnemyCountDic[player]+DAMAGE_WHEIGHT*_playertakeDamageCountDic[player];
        return Mathf.FloorToInt(difficulty);
    }
   
    #endregion


}

