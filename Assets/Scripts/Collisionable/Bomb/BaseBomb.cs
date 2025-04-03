using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public abstract class BaseBomb:MonoBehaviour,ICollisionable
{
    [SerializeField]
    private int _bombDamage = 10;//‚Æ‚è‚ ‚¦‚¸10
    public IBaseCollisionData BaseData {  get=>_capsuleData; }

    public CheckCollisionMode CheckCollisionMode {  get=>CheckCollisionMode.collisionable;  }
    protected CapsuleData _capsuleData;
    CapsuleCollider _capsuleCollider;

    public LayerMask CollisionableLayer {  get; private set; }
    private void Start()
    {
        InitBomb();
    }
    public virtual void CharacterBomb()
    {

    }
    public void InitBomb()
    {
        _capsuleCollider = GetComponent<CapsuleCollider>();
        Vector2 origin = new Vector2(transform.position.x, transform.position.z+(transform.lossyScale.y*_capsuleCollider.height/2));
        Vector2 end = new Vector2(transform.position.x, transform.position.z - (transform.lossyScale.y * _capsuleCollider.height / 2));
        _capsuleData = new CapsuleData(origin,end,_capsuleCollider.radius);
    }
    public void OnCollisionEvent(ICollisionable collisionable)
    {
        switch (collisionable)
        {
            case EnemyCharacter enemy:
                {
                    enemy.Status.CalclateHp(_bombDamage);
                    return;
                }
        
        }

    }
}

