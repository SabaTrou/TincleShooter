using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;
using VContainer;
using MessagePipe;
using System;

public class EnemySpawnService : IInitializable
{
    #region 変数
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
        //EnemyCharacterのついたプレハブを取得
        GameObject[] enemys = PrefabLoader.LoadPreafabWithComponent<EnemyCharacter>();
        //登録
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
    /// 再利用可能な敵がいるか確認
    /// </summary>
    /// <param name="characterType">確認する敵</param>
    /// <returns></returns>
    private bool CheckSpawnableEnemy(Type characterType)
    {

        //keyが存在すれば
        if (_spawnableEnemyQueuesDic.ContainsKey(characterType))
        {
            //queueの中身が0より多ければ
            if (_spawnableEnemyQueuesDic[characterType].Count > 0)
            {
                return true;
            }
            
            //そうでなければ
            return false;
        }
        //keyがなければnewする
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
    /// enemyCharacter,gameObjectのDictionaryに登録
    /// </summary>
    /// <param name="enemyObjects">登録するgameObject群</param>
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
