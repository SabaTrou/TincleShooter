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
    /// Score加算
    /// </summary>
    /// <param name="addEvent"></param>
    private void OnScoreAdded(ScoreAddEvent addEvent)
    {
    
        _playerScoreDic[addEvent.player].Value += addEvent.score;
    }
    /// <summary>
    /// player追加
    /// </summary>
    /// <param name="addEvent"></param>
    private void OnPlayerAdded(PlayerAddEvent addEvent)
    {
        //scoreの初期値は0
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
    /// Socreのプロパティをもらう
    /// </summary>
    /// <param name="player">取得したいプレイヤー</param>
    /// <returns>プロパティ</returns>
    public ReactiveProperty<int> GetScoreProperty(PlayerCharacter player);

}
