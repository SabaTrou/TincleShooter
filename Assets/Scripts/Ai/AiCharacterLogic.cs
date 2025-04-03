using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePipe;
using VContainer.Unity;
using VContainer;
using System;
/// <summary>
/// 敵のAiのロジック部分
/// 基底にしてもいいかも
/// </summary>
public class AiCharacterLogic
{
    #region　変数
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
    
    public　void UpdateLogic()
    {
        //ペアリングされていなければreturn
       if(!_isPair)
        {
            return;
        }
        
        //一番近い敵
        EnemyCharacter nearestEnemy;
        if(!_enemySearcher.TrySearchNearestEnemy(_character,out nearestEnemy))
        {
            return;
        }
        //敵の位置
        Vector3 enemyPosition = nearestEnemy.transform.position;
        //自分の位置
        Vector3 playerPosition=_character.transform.position;
        // 距離の計算
        float distanceX = GetDistanceX(playerPosition, enemyPosition); // X軸距離
        float distanceZ = GetDistanceZ(playerPosition, enemyPosition); // Y軸距離
        
        float collisionDistance = _character.CircleRadius + nearestEnemy.CircleRadius; // 衝突距離
        
        
        // 時間の計算
        float timetoHit=0; // 衝突までの時間
        float dodgeTime=0; // 回避に必要な時間
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
    /// 距離を取得
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
    /// 距離を取得x
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
    /// 距離を取得y
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
        // 敵からプレイヤーへの方向ベクトルを計算
        Vector3 direction = enemyPos - playerPos;

        // ベクトルを正規化して単位ベクトルを作成
        Vector3 normalizedDir = direction.normalized;

        // 敵の方向と逆方向のベクトルを計算
        Vector3 moveDir = -normalizedDir;

        // Y軸成分をゼロにして、平面上の回避方向に限定
        moveDir.y = 0;
        
        return moveDir;
    }
    private Vector3 GetApproachDirection(Vector3 playerPos,Vector3 enemyPos)
    {
        // 敵からプレイヤーへの方向ベクトルを計算
        Vector3 direction = enemyPos - playerPos;

        // ベクトルを正規化して単位ベクトルを作成
        Vector3 normalizedDir = direction.normalized;

       
        

        // Y軸成分をゼロにして、平面上の回避方向に限定
        normalizedDir.y = 0;

        return normalizedDir;
    }
    
    #endregion
    /// <summary>
    /// キャラクターをペアリング
    /// </summary>
    /// <param name="character"></param>
    public void SetCharacter(PlayerCharacter character)
    {
      
        _character = character;
        _isPair = true;
        _spawnPosition = character.transform.position;
    }
    /// <summary>
    /// ペアリングを解除
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
