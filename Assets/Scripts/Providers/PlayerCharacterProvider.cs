using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePipe;
using VContainer;
using VContainer.Unity;

public class PlayerCharacterProvider:IInitializable
{
    private ISubscriber<CharacterAddEvent> _subscriber;
    [Inject] private IPublisher<PlayerAddEvent> _publisher;
    
    [Inject]
    private void CharacterAdd(ISubscriber<CharacterAddEvent> subscriber)
    {
        _subscriber = subscriber;
        _subscriber.Subscribe(OnCharacterAdded);
    }
    void IInitializable.Initialize()
    {

    }
    private void OnCharacterAdded(CharacterAddEvent addEvent)
    {
        if(!(addEvent.character is PlayerCharacter player))
        {
            return;
        }
        
        _publisher.Publish(new PlayerAddEvent(player));
    }
}
