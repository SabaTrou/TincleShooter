using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePipe;
using VContainer.Unity;
using VContainer;
using System;
/// <summary>
/// �G��Ai�̃��W�b�N����
/// ���ɂ��Ă���������
/// </summary>
public class AiCharacterLogic
{
    #region�@�ϐ�
    //messgePipe
    [Inject]private EnemySearchService _enemySearcher;
    private ISubscriber<LogicDifficultySetEvent> _difficultySetEvent;
    //
    private PlayerCharacter _character;
    private bool _isPair = false;
    private int _logicLevel;
    private Vector3 _spawnPosition=default;
    #endregion
    #region DI
    [Inject]
    private void DifficultySet(ISubscriber<LogicDifficultySetEvent> subscriber)
    {
        _difficultySetEvent = subscriber;
        _difficultySetEvent.Subscribe(OnDifficultySet);

    }
    #endregion
    
    public�@void UpdateLogic()
    {
        //�y�A�����O����Ă��Ȃ����return
       if(!_isPair)
        {
            return;
        }
        
        //��ԋ߂��G
        EnemyCharacter nearestEnemy;
        if(!_enemySearcher.TrySearchNearestEnemy(_character,out nearestEnemy))
        {
            return;
        }
        //�G�̈ʒu
        Vector3 enemyPosition = nearestEnemy.transform.position;
        //�����̈ʒu
        Vector3 playerPosition=_character.transform.position;
        // �����̌v�Z
        float distanceX = GetDistanceX(playerPosition, enemyPosition); // X������
        float distanceZ = GetDistanceZ(playerPosition, enemyPosition); // Y������
        
        float collisionDistance = _character.CircleRadius + nearestEnemy.CircleRadius; // �Փˋ���
        
        
        // ���Ԃ̌v�Z
        float timetoHit=0; // �Փ˂܂ł̎���
        float dodgeTime=0; // ����ɕK�v�Ȏ���
        //
        if (collisionDistance>= distanceX && collisionDistance <distanceZ)
        {
            timetoHit = (distanceZ - collisionDistance) / nearestEnemy.Status.MoveSpeed;
            dodgeTime = (collisionDistance - distanceX) / _character.Status.MoveSpeed;
        }
        
        if (collisionDistance>distanceX)
        {
            timetoHit += nearestEnemy.Status.Hp * _character.ShotCoolTime;
            
            if (timetoHit < dodgeTime)
            {
                //_character.Move(GetApproachDirection(playerPosition,enemyPosition)) ;
                _character.Attack();

            }
            else
            {
                _character.Attack();
                _character.Move(GetDodgeDirection(playerPosition, enemyPosition));
            }
        }
        else if(collisionDistance<distanceZ)
        {
            _character.Move(GetApproachDirection(playerPosition,_spawnPosition));
        }
    }
    #region Distance
    /// <summary>
    /// �������擾
    /// </summary>
    /// <param name="point1"></param>
    /// <param name="point2"></param>
    /// <returns></returns>
    private float GetDistance(Vector3 point1, Vector3 point2)
    {
        Vector3 distanceVec = point1 - point2;
        float distance = Mathf.Sqrt(
            Mathf.Pow(Mathf.Abs(distanceVec.x), 2) +
            Mathf.Pow(Mathf.Abs(distanceVec.z), 2)
            );
        return distance;
    } 
    /// <summary>
    /// �������擾x
    /// </summary>
    /// <param name="point1"></param>
    /// <param name="point2"></param>
    /// <returns></returns>
    private float GetDistanceX(Vector3 point1, Vector3 point2)
    {
        Vector3 distanceVec = point1 - point2;
        float distance = MathF.Abs(distanceVec.x);
        return distance;
    }
    /// <summary>
    /// �������擾y
    /// </summary>
    /// <param name="point1"></param>
    /// <param name="point2"></param>
    /// <returns></returns>
    private float GetDistanceZ(Vector3 point1, Vector3 point2)
    {
        Vector3 distanceVec = point1 - point2;
        
        float distance = MathF.Abs(distanceVec.z);
        return distance;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="point1"></param>
    /// <param name="point2"></param>
    #endregion
    #region Direction
    private Vector3 GetDodgeDirection(Vector3 playerPos, Vector3 enemyPos)
    {
        // �G����v���C���[�ւ̕����x�N�g�����v�Z
        Vector3 direction = enemyPos - playerPos;

        // �x�N�g���𐳋K�����ĒP�ʃx�N�g�����쐬
        Vector3 normalizedDir = direction.normalized;

        // �G�̕����Ƌt�����̃x�N�g�����v�Z
        Vector3 moveDir = -normalizedDir;

        // Y���������[���ɂ��āA���ʏ�̉������Ɍ���
        moveDir.y = 0;
        
        return moveDir;
    }
    private Vector3 GetApproachDirection(Vector3 playerPos,Vector3 enemyPos)
    {
        // �G����v���C���[�ւ̕����x�N�g�����v�Z
        Vector3 direction = enemyPos - playerPos;

        // �x�N�g���𐳋K�����ĒP�ʃx�N�g�����쐬
        Vector3 normalizedDir = direction.normalized;

       
        

        // Y���������[���ɂ��āA���ʏ�̉������Ɍ���
        normalizedDir.y = 0;

        return normalizedDir;
    }
    
    #endregion
    /// <summary>
    /// �L�����N�^�[���y�A�����O
    /// </summary>
    /// <param name="character"></param>
    public void SetCharacter(PlayerCharacter character)
    {
      
        _character = character;
        _isPair = true;
        _spawnPosition = character.transform.position;
    }
    /// <summary>
    /// �y�A�����O������
    /// </summary>
    public void DisposePairCharacter()
    {
        _character = null;
        _isPair = false;
    }
    private void OnDifficultySet(LogicDifficultySetEvent setEvent)
    {
        _logicLevel = setEvent.logicLevel;
    }
}
