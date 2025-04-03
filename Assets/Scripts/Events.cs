using System;
using UnityEngine;


//通知用クラスをまとめるためのファイル

#region Character
/// <summary>
/// キャラクターを追加したときの通知用
/// </summary>
public class CharacterAddEvent
{
    readonly public BaseCharacter character;
    public CharacterAddEvent(BaseCharacter addedCharacter)
    {
        this.character = addedCharacter;
    }
}
public class PlayerAddEvent
{
    readonly public PlayerCharacter player;

    public PlayerAddEvent(PlayerCharacter player)
    {
        this.player = player;
    }
}
public class PlayerDeadEvent
{
    readonly public PlayerCharacter player;
    public PlayerDeadEvent(PlayerCharacter player)
    {
        this.player = player;
    }
}

/// <summary>
/// キャラクターの追加をリクエストする通知用
/// </summary>
public class CharacterInstantiateRequestEvent
{
    readonly public Vector3 Position;
    readonly public GameObject characterPrefab;
    readonly public Transform parent;
    public CharacterInstantiateRequestEvent(GameObject characterPrefab, Vector3 Position, Transform parent)
    {
        this.characterPrefab = characterPrefab;
        this.Position = Position;
        this.parent = parent;
    }
}
/// <summary>
/// キャラクターの位置の変更要求
/// </summary>
public class CharacterMoveRequetEvent
{
    readonly public BaseCharacter character;
    readonly public Vector3 movePosition;
    public CharacterMoveRequetEvent(BaseCharacter character, Vector3 movePosition)
    {
        this.character = character;
        this.movePosition = movePosition;
    }

}
#region player
//プレイヤーキャラクターがダメージを食らった通知
public class PlayerTakeDamageEvent
{
    readonly public int damage;
    readonly public PlayerCharacter playerCharacter;
    public PlayerTakeDamageEvent(int damage, PlayerCharacter playerCharacter)
    {
        this.damage = damage;
        this.playerCharacter = playerCharacter;
    }
}
#endregion
#region enemy
/// <summary>
/// 敵のスポーンを要求通知用
/// </summary>
public class EnemySpawnRequestEvent
{
    readonly public Type enemyCharacterType;
    readonly public Vector3 spawnPosition;
    readonly public Transform parent;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="enemyNameHash">.name.getHashCode()</param>
    /// <param name="spawnPosition"></param>
    /// <param name="parent"></param>
    public EnemySpawnRequestEvent(Type enemyCharacterType, Vector3 spawnPosition, Transform parent)
    {
        this.enemyCharacterType = enemyCharacterType;
        this.spawnPosition = spawnPosition;
        this.parent = parent;
    }
}

public class EnemyDeadEvent
{
    readonly public PlayerCharacter player;
    readonly public EnemyCharacter enemyCharacter;

    public EnemyDeadEvent(EnemyCharacter enemyCharacter, PlayerCharacter player)
    {
        this.enemyCharacter = enemyCharacter;
        this.player = player;
    }

}
public class EnemyTakeDamageEvent
{
    readonly public EnemyCharacter character;
    public EnemyTakeDamageEvent(EnemyCharacter character)
    {
        this.character = character;
    }
}
public class EnemyReSycleEvent
{
    readonly public EnemyCharacter enemy;
    readonly public Vector3 position;
    public EnemyReSycleEvent(EnemyCharacter enemy,Vector3 position)
    {
        this.enemy = enemy;
        this.position = position;
    }
}

#endregion
#endregion
#region Controller
/// <summary>
/// コントローラーを追加したときの通知用
/// </summary>
public class ControllerAddEvent
{
    readonly public CharacterController controller;
    public ControllerAddEvent(CharacterController controller)
    {
        this.controller = controller;
    }


}
#endregion
#region ai
public class AiLogicAddEvent
{
    readonly public AiCharacterLogic logic;
    public AiLogicAddEvent(AiCharacterLogic logic)
    {
        this.logic = logic;
    }
}
public class AiLogicRequestEvent
{


}


#endregion
#region Bullet
/// <summary>
/// 球追加要求時の通知用
/// </summary>
public class BulletInstantiateRequestEvent
{
    readonly public Vector3 Position;
    readonly public BulletPrefabValue prefabValue;
    //readonly public Transform parent;
    public BulletInstantiateRequestEvent(BulletPrefabValue prefabValue, Vector3 Position)
    {
        this.prefabValue = prefabValue;
        this.Position = Position;

    }

}

/// <summary>
/// 球が追加されたときの通知用
/// </summary>
public class BulletAddEvent
{
    readonly public BaseBullet baseBullet;
    readonly public BulletPrefabValue prefabValue;
    public BulletAddEvent(BaseBullet baseBullet, BulletPrefabValue prefabValue)
    {
        this.baseBullet = baseBullet;
        this.prefabValue = prefabValue;
    }

}

/// <summary>
/// 発射要求時の通知用
/// </summary>
public class BulletShotRequestEvent
{
    readonly public BulletPrefabValue prefabValue;
    readonly public Vector3 position;
    readonly public float bulletSpeed;
    readonly public PlayerCharacter shotPlayer;
    readonly public int damage;
    public BulletShotRequestEvent(BulletPrefabValue prefabValue, Vector3 position, float bulletSpeed, int damage, PlayerCharacter shotPlayer)
    {
        this.prefabValue = prefabValue;
        this.position = position;
        this.bulletSpeed = bulletSpeed;
        this.damage = damage;
        this.shotPlayer = shotPlayer;
    }

}
#endregion
#region Collisionable
/// <summary>
/// 衝突可能オブジェクトの追加時の通知用
/// </summary>
public class CollisionableAddEvent
{
    readonly public ICollisionable collisionable;
    public CollisionableAddEvent(ICollisionable collisionable)
    {
        this.collisionable = collisionable;
    }


}
#endregion
#region Difficulty

/// <summary>
/// 難易度適用イベント
/// </summary>
public class StageDifficultySetEvent
{
    readonly public int spawnSpanLevel;
    readonly public PlayerCharacter player;

    public StageDifficultySetEvent(int difficultyLevel, PlayerCharacter player)
    {
        this.spawnSpanLevel = difficultyLevel;
        this.player = player;
    }
}
public class LogicDifficultySetEvent
{
    readonly public int logicLevel;
    public LogicDifficultySetEvent(int logicLevel)
    {
        this.logicLevel = logicLevel;
    }
}
#endregion
#region UI
public class ScoreAddEvent
{
    readonly public PlayerCharacter player;
    readonly public int score;
    public ScoreAddEvent(PlayerCharacter player,int score)
    {
        this.player = player;
        this.score = score;
    }


}

#endregion




