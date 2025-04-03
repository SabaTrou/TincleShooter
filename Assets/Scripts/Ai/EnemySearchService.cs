using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using MessagePipe;
using VContainer.Unity;
using UnityEngine.TextCore.Text;
using System;
using VContainer;



public class EnemySearchService:IInitializable
{
    #region 変数
    private ISubscriber<CharacterAddEvent> _addEvent;

    private Dictionary<Type, List<EnemyCharacter>> _enemyCharacterListsDic = new();
    #endregion
    #region DI
    /// <summary>
    /// キャラクターが追加されたとき
    /// </summary>
    /// <param name="subscriber"></param>
    [Inject]
    private void CharacterAdd(ISubscriber<CharacterAddEvent> subscriber)
    {
        _addEvent = subscriber;
        _addEvent.Subscribe(RegisterCharacter);
    }
    #endregion
    //インスタンス化用
    void IInitializable.Initialize()
    {

    }

    #region EnemyList
    /// <summary>
    /// characterをリストに追加
    /// </summary>
    /// <param name="addEvent"></param>
    private void RegisterCharacter(CharacterAddEvent addEvent)
    {

        //addEventのキャラクターがEnemyCharacterではなければreturn
        if (!(addEvent.character is EnemyCharacter enemy))
        {
           
            return;
        }
        //キャラクターのTypeを取得
        Type characterType = addEvent.character.GetType();
        //登録されていなければnew()
        if (!_enemyCharacterListsDic.ContainsKey(characterType))
        {
            _enemyCharacterListsDic[characterType] = new();
        }
        //リストに追加
        _enemyCharacterListsDic[characterType].Add(enemy);
    }
    #endregion
    #region EnemySearch
    #region All
    /// <summary>
    /// 全件検索
    /// </summary>
    /// <param name="targetCharacter">対象のキャラクター</param>
    /// <returns></returns>
    public bool TrySearchNearestEnemy(BaseCharacter targetCharacter,out EnemyCharacter enemy)
    {
        enemy = null;
        if(_enemyCharacterListsDic.Values.Count<=0)
        {
            return false;
        }

        List<EnemyCharacter> enemyCharacters = new List<EnemyCharacter>();
        foreach (List<EnemyCharacter> enemies in _enemyCharacterListsDic.Values)
        {
            EnemyCharacter character = GetNearestCharacter(enemies, targetCharacter);
            if (character != null)
            {
                enemyCharacters.Add(character);
            }

        }
        enemy= GetNearestCharacter(enemyCharacters, targetCharacter); 
        return true;
    } 
    public Vector3 SearchNearEnemyPosition(BaseCharacter targetCharacter)
    {


        List<EnemyCharacter> enemyCharacters = new List<EnemyCharacter>();
        foreach (List<EnemyCharacter> enemies in _enemyCharacterListsDic.Values)
        {
            EnemyCharacter character = GetNearestCharacter(enemies, targetCharacter);
            if (character != null)
            {
                enemyCharacters.Add(character);
            }

        }
        return GetNearestCharacter(enemyCharacters, targetCharacter).transform.position;
    }
    #endregion
    #region Generic
    /// <summary>
    /// 敵のタイプごとに検索
    /// </summary>
    /// <typeparam name="T">EnemyCharacter限定</typeparam>
    /// <param name="targetCharacter">対象のキャラクター</param>
    /// <returns></returns>
    protected EnemyCharacter SearchNearEnemy<T>(BaseCharacter targetCharacter) where T : EnemyCharacter
    {
        //Dictionaryに登録されていなければreturn
        if (!_enemyCharacterListsDic.TryGetValue(typeof(T), out List<EnemyCharacter> enemies))
        {
            Debug.Log("指定されたTypeは登録されていません");
            return null;
        }
        return GetNearestCharacter(enemies,targetCharacter);
    }
    #endregion
    /// <summary>
    /// 共通処理
    /// </summary>
    /// <param name="enemies">敵のリスト</param>
    /// <param name="targetCharacter">対象のキャラクター</param>
    /// <returns></returns>
    private EnemyCharacter GetNearestCharacter(List<EnemyCharacter> enemies,BaseCharacter targetCharacter)
    {
        //-1はとりえない値
        float nearEnemyDistance = -1;
        EnemyCharacter nearEnemy = default;
        foreach (EnemyCharacter enemy in enemies)
        {
            float distance = GetDistance(targetCharacter.transform.position, enemy.transform.position);
            //enemyとの距離がnearEnemyDistanceより短いか、nearEnemyDistaceが-1の時
            if ((distance < nearEnemyDistance) || nearEnemyDistance == -1)
            {
                nearEnemy = enemy;
                nearEnemyDistance = distance;
                continue;
            }
        }
        return nearEnemy;
    }
    /// <summary>
    /// 共通処理
    /// </summary>
    /// <param name="enemies">敵のリスト</param>
    /// <param name="targetCharacter">対象のキャラクター</param>
    /// <returns></returns>
    private EnemyCharacter GetNearestCharacterDistance(List<EnemyCharacter> enemies,BaseCharacter targetCharacter)
    {
        //-1はとりえない値
        float nearEnemyDistance = -1;
        EnemyCharacter nearEnemy = default;
        foreach (EnemyCharacter enemy in enemies)
        {
            float distance = GetDistance(targetCharacter.transform.position, enemy.transform.position);
            //enemyとの距離がnearEnemyDistanceより短いか、nearEnemyDistaceが-1の時
            if ((distance < nearEnemyDistance) || nearEnemyDistance == -1)
            {
                nearEnemy = enemy;
                nearEnemyDistance = distance;
                continue;
            }
        }
        return nearEnemy;
    }

    private float GetDistance(Vector3 point1, Vector3 point2)
    {
        Vector3 distanceVec = point1 - point2;
        float distance = Mathf.Sqrt(
            Mathf.Pow(Mathf.Abs(distanceVec.x), 2) +
            Mathf.Pow(Mathf.Abs(distanceVec.z), 2)
            );
        return distance;
    }
    #endregion
}
