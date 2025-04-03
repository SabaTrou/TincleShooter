using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;
using VContainer;
using MessagePipe;
using System;

public class EnemySpawnService : IInitializable
{
    #region �ϐ�
    //messagePipe
    private ISubscriber<EnemySpawnRequestEvent> _spawnRequest;
    private ISubscriber<EnemyReSycleEvent> _reSycleEvent;
    [Inject] private IPublisher<CharacterInstantiateRequestEvent> _instantiateRequest;
    //
    //nameHash,Queue
    private Dictionary<Type, Queue<EnemyCharacter>> _spawnableEnemyQueuesDic = new();
    //nameHash,prefab
    private Dictionary<Type, GameObject> _enemyGameObjectDic = new ();
    #endregion
    #region DI
    [Inject]
    private void EnemySpawnRequest(ISubscriber<EnemySpawnRequestEvent> subscriber)
    {
        _spawnRequest = subscriber;
        _spawnRequest.Subscribe(SpawnEnemy);
    }
    [Inject]
    private void EnemyReSycle(ISubscriber<EnemyReSycleEvent> subscriber)
    {
        _reSycleEvent = subscriber;
        _reSycleEvent.Subscribe(RegisterSpawnableDictionary);
        _reSycleEvent.Subscribe(MoveEnemy);
    }
    
    #endregion

    void IInitializable.Initialize()
    {
        //EnemyCharacter�̂����v���n�u���擾
        GameObject[] enemys = PrefabLoader.LoadPreafabWithComponent<EnemyCharacter>();
        //�o�^
        RegisterEnemyDictinary(enemys);
    }
    #region spawn
    private void SpawnEnemy(EnemySpawnRequestEvent requestEvent)
    {
        if (!_enemyGameObjectDic.ContainsKey(requestEvent.enemyCharacterType))
        {
            Debug.LogError("enemyCharacter is missing");
            return;
        }
       
        if (CheckSpawnableEnemy(requestEvent.enemyCharacterType))
        {
            //
            ReSpawnEnemy(requestEvent);
            
        }
        else
        {
            //
            GameObject enemyObj = _enemyGameObjectDic[requestEvent.enemyCharacterType];
            _instantiateRequest.Publish(new CharacterInstantiateRequestEvent(enemyObj,requestEvent.spawnPosition,requestEvent.parent));
        }
    }
    /// <summary>
    /// �ė��p�\�ȓG�����邩�m�F
    /// </summary>
    /// <param name="characterType">�m�F����G</param>
    /// <returns></returns>
    private bool CheckSpawnableEnemy(Type characterType)
    {

        //key�����݂����
        if (_spawnableEnemyQueuesDic.ContainsKey(characterType))
        {
            //queue�̒��g��0��葽�����
            if (_spawnableEnemyQueuesDic[characterType].Count > 0)
            {
                return true;
            }
            
            //�����łȂ����
            return false;
        }
        //key���Ȃ����new����
        _spawnableEnemyQueuesDic[characterType] = new();
        return false;

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="requestEvent"></param>
    private void ReSpawnEnemy(EnemySpawnRequestEvent requestEvent)
    {
        EnemyCharacter enemy = _spawnableEnemyQueuesDic[requestEvent.enemyCharacterType].Dequeue();
        enemy.transform.position = requestEvent.spawnPosition;
        enemy.transform.parent = requestEvent.parent;
        enemy.ResetEnemyStatus();
    }
    #endregion
    private void MoveEnemy(EnemyReSycleEvent reSycleEvent)
    {
        reSycleEvent.enemy.transform.position = reSycleEvent.position;
    }
    #region register
    /// <summary>
    /// 
    /// </summary>
    /// <param name="reSycleEvent"></param>
    private void RegisterSpawnableDictionary(EnemyReSycleEvent reSycleEvent)
    {
        Type enemyType=reSycleEvent.enemy.GetType();
        if(!_spawnableEnemyQueuesDic.ContainsKey(enemyType))
        {
            _spawnableEnemyQueuesDic[enemyType] = new();
        }
        _spawnableEnemyQueuesDic[enemyType].Enqueue(reSycleEvent.enemy);
       
    }
    /// <summary>
    /// enemyCharacter,gameObject��Dictionary�ɓo�^
    /// </summary>
    /// <param name="enemyObjects">�o�^����gameObject�Q</param>
    private void RegisterEnemyDictinary(GameObject[] enemyObjects)
    {
        foreach(GameObject enemy in enemyObjects)
        {
            if(!enemy.TryGetComponent<EnemyCharacter>(out EnemyCharacter character))
            {
                Debug.LogError("EnemyCharacter is missing");
               continue;
            }
            _enemyGameObjectDic[character.GetType()] = enemy;
        }
    }
    #endregion
}
