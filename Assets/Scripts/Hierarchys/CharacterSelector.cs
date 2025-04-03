using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using MessagePipe;

public class CharacterSelector : MonoBehaviour
{
    [SerializeField]
    private Transform _character1SpawnPosition=default;
    [SerializeField]
    private Transform _character2SpawnPosition=default;

    [SerializeField]
    private GameObject _playerPrefab;
    [SerializeField]
    private GameObject _player2Prefab;
    [SerializeField]
    private Transform _parent;
    private IPublisher<CharacterInstantiateRequestEvent> _reqestEvent;
    [Inject]
    public void Constuct(IPublisher<CharacterInstantiateRequestEvent> publisher)
    {
        _reqestEvent = publisher;
    }
    // Start is called before the first frame update
    void Start()
    {
        RegisterCharacter();

    }
    private void RegisterCharacter()
    {
        _reqestEvent.Publish(new CharacterInstantiateRequestEvent(_playerPrefab,_character1SpawnPosition.position,_parent));
        _reqestEvent.Publish(new CharacterInstantiateRequestEvent(_player2Prefab,_character2SpawnPosition.position,_parent));
    }
    
}
