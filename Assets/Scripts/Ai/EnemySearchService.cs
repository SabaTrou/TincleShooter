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
    #region �ϐ�
    private ISubscriber<CharacterAddEvent> _addEvent;

    private Dictionary<Type, List<EnemyCharacter>> _enemyCharacterListsDic = new();
    #endregion
    #region DI
    /// <summary>
    /// �L�����N�^�[���ǉ����ꂽ�Ƃ�
    /// </summary>
    /// <param name="subscriber"></param>
    [Inject]
    private void CharacterAdd(ISubscriber<CharacterAddEvent> subscriber)
    {
        _addEvent = subscriber;
        _addEvent.Subscribe(RegisterCharacter);
    }
    #endregion
    //�C���X�^���X���p
    void IInitializable.Initialize()
    {

    }

    #region EnemyList
    /// <summary>
    /// character�����X�g�ɒǉ�
    /// </summary>
    /// <param name="addEvent"></param>
    private void RegisterCharacter(CharacterAddEvent addEvent)
    {

        //addEvent�̃L�����N�^�[��EnemyCharacter�ł͂Ȃ����return
        if (!(addEvent.character is EnemyCharacter enemy))
        {
           
            return;
        }
        //�L�����N�^�[��Type���擾
        Type characterType = addEvent.character.GetType();
        //�o�^����Ă��Ȃ����new()
        if (!_enemyCharacterListsDic.ContainsKey(characterType))
        {
            _enemyCharacterListsDic[characterType] = new();
        }
        //���X�g�ɒǉ�
        _enemyCharacterListsDic[characterType].Add(enemy);
    }
    #endregion
    #region EnemySearch
    #region All
    /// <summary>
    /// �S������
    /// </summary>
    /// <param name="targetCharacter">�Ώۂ̃L�����N�^�[</param>
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
    /// �G�̃^�C�v���ƂɌ���
    /// </summary>
    /// <typeparam name="T">EnemyCharacter����</typeparam>
    /// <param name="targetCharacter">�Ώۂ̃L�����N�^�[</param>
    /// <returns></returns>
    protected EnemyCharacter SearchNearEnemy<T>(BaseCharacter targetCharacter) where T : EnemyCharacter
    {
        //Dictionary�ɓo�^����Ă��Ȃ����return
        if (!_enemyCharacterListsDic.TryGetValue(typeof(T), out List<EnemyCharacter> enemies))
        {
            Debug.Log("�w�肳�ꂽType�͓o�^����Ă��܂���");
            return null;
        }
        return GetNearestCharacter(enemies,targetCharacter);
    }
    #endregion
    /// <summary>
    /// ���ʏ���
    /// </summary>
    /// <param name="enemies">�G�̃��X�g</param>
    /// <param name="targetCharacter">�Ώۂ̃L�����N�^�[</param>
    /// <returns></returns>
    private EnemyCharacter GetNearestCharacter(List<EnemyCharacter> enemies,BaseCharacter targetCharacter)
    {
        //-1�͂Ƃ肦�Ȃ��l
        float nearEnemyDistance = -1;
        EnemyCharacter nearEnemy = default;
        foreach (EnemyCharacter enemy in enemies)
        {
            float distance = GetDistance(targetCharacter.transform.position, enemy.transform.position);
            //enemy�Ƃ̋�����nearEnemyDistance���Z�����AnearEnemyDistace��-1�̎�
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
    /// ���ʏ���
    /// </summary>
    /// <param name="enemies">�G�̃��X�g</param>
    /// <param name="targetCharacter">�Ώۂ̃L�����N�^�[</param>
    /// <returns></returns>
    private EnemyCharacter GetNearestCharacterDistance(List<EnemyCharacter> enemies,BaseCharacter targetCharacter)
    {
        //-1�͂Ƃ肦�Ȃ��l
        float nearEnemyDistance = -1;
        EnemyCharacter nearEnemy = default;
        foreach (EnemyCharacter enemy in enemies)
        {
            float distance = GetDistance(targetCharacter.transform.position, enemy.transform.position);
            //enemy�Ƃ̋�����nearEnemyDistance���Z�����AnearEnemyDistace��-1�̎�
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
