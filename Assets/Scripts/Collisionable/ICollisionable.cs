using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ICollisionable
{
    
    /// <summary>
    /// �����蔻��p�̃f�[�^
    /// </summary>
    public IBaseCollisionData BaseData { get;  }
    /// <summary>
    /// �Փ˂̔�����Ƃ邩�ǂ����̃��[�h
    /// </summary>
    public CheckCollisionMode CheckCollisionMode { get; }
    /// <summary>
    /// �Փ˂��郌�C���[
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

