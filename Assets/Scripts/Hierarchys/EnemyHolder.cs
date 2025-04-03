using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePipe;
using VContainer;

public class EnemyHolder : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _enemyPrefabs;
    private List<EnemyCharacter> _enemyCharacterList=new();
    [SerializeField]
    private Transform _point1origin;
    [SerializeField]
    private Transform _point1end;
    [SerializeField]
    private Transform _point2origin;
    [SerializeField]
    private Transform _point2end;
    [SerializeField]
    private float _spawnTime = 1f;
    private float _conutTime = 0;
    //èCê≥Ç∑ÇÈ
    [SerializeField]
    private float _paceUpTime = 20f;
    private float _paveUpCount = 0;
    [Inject]IPublisher<EnemySpawnRequestEvent> _spawnRequest;
    private void Start()
    {
        foreach(GameObject enemy in _enemyPrefabs)
        {
            if(enemy.TryGetComponent<EnemyCharacter>(out EnemyCharacter component))
            {
                _enemyCharacterList.Add(component);
            }
        }
    }

    private void Update()
    {
        _conutTime += Time.deltaTime;
        _paveUpCount += Time.deltaTime;
        if(_paveUpCount>=_paceUpTime)
        {
            _spawnTime /= 2;
            _paveUpCount = 0;
        }
        if(_conutTime<_spawnTime)
        {
            return;
        }
        _conutTime = 0f;
        Vector3 point = new Vector3(Random.Range(_point1origin.position.x, _point1end.position.x), 0, _point1origin.position.z);
        _spawnRequest.Publish(new EnemySpawnRequestEvent(_enemyCharacterList[Random.Range(0, _enemyCharacterList.Count)].GetType(), point, null));
        point = new Vector3(Random.Range(_point2origin.position.x, _point2end.position.x), 0, _point2origin.position.z);
        _spawnRequest.Publish(new EnemySpawnRequestEvent(_enemyCharacterList[Random.Range(0, _enemyCharacterList.Count)].GetType(), point, null));
    }
}
