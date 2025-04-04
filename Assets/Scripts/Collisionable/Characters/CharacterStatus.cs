using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class CharacterStatus
{
    [SerializeField,Tooltip("体力"),Range(0,99999)]
    private int _hp;
    [SerializeField, Tooltip("最大体力"), Range(0, 99999)]
    private int _maxhp;
    public int Hp { get=>_hp; }

    public int MaxHp { get =>_maxhp; }
    [SerializeField,Tooltip("攻撃力")]
    private int _attackDamage;
    public int AttackDamage { get=>_attackDamage; }
    [SerializeField,Tooltip("移動速度")]
    private float _moveSpeed;
    public float MoveSpeed { get=>_moveSpeed; }
    public CharacterStatus(int hp,int attackDamage,int moveSpeed)
    {
        _hp = hp;
        _maxhp = hp;
        _attackDamage = attackDamage;
        _moveSpeed = moveSpeed;
    }
    public void CalclateHp(int damage)
    {
        _hp -= damage;
    }

    public void ResetStatus()
    {
        _hp = _maxhp;
    }
    

}
