using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;
using MessagePipe;
using VContainer;


public class ScoreHolder:IScorePropertyHolder
{
    #region Pipe
    private ISubscriber<ScoreAddEvent> _scoreAddEvent;
    private ISubscriber<PlayerAddEvent> _playerAddEvent;
    
    #endregion
    private Dictionary<PlayerCharacter, ReactiveProperty<int>> _playerScoreDic = new();
    
    #region DI
    [Inject]
    private void ScoreAdd(ISubscriber<ScoreAddEvent> subscriber)
    {
        _scoreAddEvent = subscriber;
        _scoreAddEvent.Subscribe(OnScoreAdded);
    }
    [Inject]
    private void PlayerAdd(ISubscriber<PlayerAddEvent> subscriber)
    {
        _playerAddEvent = subscriber;
        _playerAddEvent.Subscribe(OnPlayerAdded);
    }
    #endregion


    /// <summary>
    /// Score���Z
    /// </summary>
    /// <param name="addEvent"></param>
    private void OnScoreAdded(ScoreAddEvent addEvent)
    {
    
        _playerScoreDic[addEvent.player].Value += addEvent.score;
    }
    /// <summary>
    /// player�ǉ�
    /// </summary>
    /// <param name="addEvent"></param>
    private void OnPlayerAdded(PlayerAddEvent addEvent)
    {
        //score�̏����l��0
        _playerScoreDic[addEvent.player] = new(0);
    }

    public ReactiveProperty<int> GetScoreProperty(PlayerCharacter player)
    {
        return _playerScoreDic[player];
    }

    
}
public interface IScorePropertyHolder
{
    /// <summary>
    /// Socre�̃v���p�e�B�����炤
    /// </summary>
    /// <param name="player">�擾�������v���C���[</param>
    /// <returns>�v���p�e�B</returns>
    public ReactiveProperty<int> GetScoreProperty(PlayerCharacter player);

}
