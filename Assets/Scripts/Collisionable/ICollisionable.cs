using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ICollisionable
{
    
    /// <summary>
    /// 当たり判定用のデータ
    /// </summary>
    public IBaseCollisionData BaseData { get;  }
    /// <summary>
    /// 衝突の判定をとるかどうかのモード
    /// </summary>
    public CheckCollisionMode CheckCollisionMode { get; }
    /// <summary>
    /// 衝突するレイヤー
    /// </summary>
    public LayerMask CollisionableLayer { get; }
    /// <summary>
    /// OnCollision
    /// </summary>
    /// <param name="collisionable"></param>
    public void OnCollisionEvent(ICollisionable collisionable);
    
    public GameObject gameObject { get; }
    
}
public enum CheckCollisionMode
{
    collisionable,
    dontCollisionable
}

