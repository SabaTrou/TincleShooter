using MessagePipe;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;


public class CharacterMoveService:ITickable
{
    private float _moveSpeed = 8f;
    private float _errorValue = 0.2f;
    private List<CharacterMoveRequetEvent> _moveCharacters = new();
    private int _listLength = 0;
    private ISubscriber<CharacterMoveRequetEvent> _moveService;

    [Inject]
    private void MoveRequest(ISubscriber<CharacterMoveRequetEvent> subscriber)
    {
        _moveService = subscriber;
        _moveService.Subscribe(OnMoveCharacter);
    }
    
    void ITickable.Tick()
    {
       
        MoveCharacters(_moveCharacters);
    }
    private void OnMoveCharacter(CharacterMoveRequetEvent requetEvent)
    {
        _moveCharacters.Add(requetEvent);
        _listLength++;
        //requetEvent.character.transform.position = requetEvent.movePosition;
    }
    private void MoveCharacters(List<CharacterMoveRequetEvent> moveList)
    {
        int i = 0;
        while(i<_listLength)
        {
            CharacterMoveRequetEvent characterMoveReq = _moveCharacters[i];
            characterMoveReq.character.transform.position += GetMoveDirection(characterMoveReq.character.transform.position, characterMoveReq.movePosition) * _moveSpeed * Time.deltaTime;
            if (GetDistance(characterMoveReq.character.transform.position, characterMoveReq.movePosition) <= _errorValue)
            {
                _moveCharacters.Remove(characterMoveReq);
                _listLength--;
            }
            i++;
        }
    }
    private Vector3 GetMoveDirection(Vector3 characterPos,Vector3 targetPos)
    {
       
        Vector3 direction = targetPos - characterPos;
        Vector3 normalizedDir = direction.normalized;

        normalizedDir.y = 0;

        return normalizedDir;
    }
    private float GetDistance(Vector3 point1,Vector3 point2)
    {
        Vector3 distanceVec = point1 - point2;
        float distance = Mathf.Sqrt(
            Mathf.Pow(Mathf.Abs(distanceVec.x), 2) +
            Mathf.Pow(Mathf.Abs(distanceVec.z), 2)
            );
        return distance;
    }
}
