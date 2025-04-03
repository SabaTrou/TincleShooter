using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MessagePipe;
using VContainer;

public class UIMessenger : MonoBehaviour
{
    private Dictionary<PlayerCharacter, HpBar> _hpUiDic = new();
    private Dictionary<PlayerCharacter, ScoreBoard> _scoreBoadDic = new();
    private List<PlayerCharacter> _players = new();
    [Inject] private IScorePropertyHolder _scorePropertyHolder;
    #region UI
    [SerializeField]
    private HpBar[] _playerHpBars = default;
    [SerializeField]
    private ScoreBoard[] _playerScoreBoards = default;

    private int _playerAddCount=0;
    #endregion
    #region Pipe
    private ISubscriber<PlayerAddEvent> _characterAddEvent;
    private ISubscriber<PlayerTakeDamageEvent> _takeDamageEvent;
    #endregion
    #region DIMethod
    [Inject]
    private void CharacterAdd(ISubscriber<PlayerAddEvent> subscriber)
    {
        _characterAddEvent = subscriber;
        _characterAddEvent.Subscribe(OnCharacterAdded);
    }
    [Inject]
    private void PlayerTakeDamage(ISubscriber<PlayerTakeDamageEvent> subscriber)
    {
        _takeDamageEvent = subscriber;
        _takeDamageEvent.Subscribe(OnPlayerTakeDamage);
    }

   
    #endregion
    private void OnCharacterAdded(PlayerAddEvent addEvent)
    {
        
        
        if (_playerHpBars[_playerAddCount]==null||_playerHpBars[_playerAddCount]==null)
        {
            return;
        }
        
        _hpUiDic[addEvent.player] = _playerHpBars[_playerAddCount];
        _scoreBoadDic[addEvent.player] = _playerScoreBoards[_playerAddCount];
        _playerHpBars[_playerAddCount].SetCharacter(addEvent.player);
        _players.Add(addEvent.player);
        _scorePropertyHolder.GetScoreProperty(addEvent.player).Subscribe(_scoreBoadDic[addEvent.player].SetScore);
        _playerAddCount++;
        
    }
    private void OnPlayerTakeDamage(PlayerTakeDamageEvent damageEvent)
    {
        _hpUiDic[damageEvent.playerCharacter].SetHpValue(damageEvent.playerCharacter.Status.Hp);
    }
    
}
