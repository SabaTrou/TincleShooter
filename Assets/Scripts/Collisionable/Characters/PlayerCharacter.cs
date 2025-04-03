using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using MessagePipe;
[RequireComponent(typeof(Move))]
[RequireComponent(typeof(Attack))]
[RequireComponent(typeof(Bomb))]
public class PlayerCharacter : BaseCharacter
{
    [SerializeField]
    private GameObject _bulletPrefab;
    [SerializeField]
    private float _bulletSpeed = default;
    private int _bulletNameHash = default;
    [SerializeField]
    private Transform _shotPoint;
    public GameObject BulletPrefab { get => _bulletPrefab; }
    protected Vector3 _prevPosition = default;
    private bool _isWallHit = default;
    public bool IsWallHit { get => _isWallHit; }
    [Inject]private IPublisher<BulletShotRequestEvent> _shotRequest;
    [Inject] private IPublisher<PlayerTakeDamageEvent> _takeDamageEvent;
    [Inject] private IPublisher<PlayerDeadEvent> _deadEvent;
    #region –³“G
    private bool _isIncible = default;
    [SerializeField]
    private float _invincibleTime=0.2f;
    private float _invincibleCount = 0f;
    #endregion
    private bool _isPlayer = default;
    #region coolTime
    public float ShotCoolTime { get => _shotCoolTime; }
    [SerializeField]
    private float _shotCoolTime=0.1f;
    private float _shotCoolTimeCount=0;
    private bool _isShotCoolTime = default;
    #endregion
    public bool IsPlayer { get => _isPlayer; }

    protected override void SubCharacterStart()
    {
        _move = GetComponent<Move>();

        _bomb = GetComponent<Bomb>();

        _updateDel += UpdatePrevPos;
        _updateDel += UpdateInvincible;
        _updateDel += UpdateShotCoolTime;
        if (_bulletPrefab == null)
        {
            Debug.Log("‹…‚ª‚È‚¢");
            return;
        }
        _bulletNameHash = _bulletPrefab.name.GetHashCode();
    }
    public override void Attack()
    {
       if(_isShotCoolTime)
        {
            return;
        }
        _isShotCoolTime = true;
        BulletPrefabValue bulletPrefab = new(_bulletPrefab);
        _shotRequest.Publish(new BulletShotRequestEvent(bulletPrefab, _shotPoint.position, _bulletSpeed, Status.AttackDamage,this));
    }
    public override void OnCollisionEvent(ICollisionable collisionable)
    {
        switch (collisionable)
        {
           
            case EnemyCharacter enemy:
                {
                    if(_isIncible)
                    {
                        return;
                    }
                    //‘Ì—ÍŒ¸­
                    _status.CalclateHp(enemy.Status.AttackDamage);
                    
                    _isIncible = true;
                    
                    _takeDamageEvent.Publish(new PlayerTakeDamageEvent(enemy.Status.AttackDamage, this));
                    if(!CheckIsPlayerDead())
                    {
                        return;
                    }
                    _deadEvent.Publish(new PlayerDeadEvent(this));
                    return;
                }
            case Wall:
                {
                    
                    _isWallHit = true;
                    transform.position = _prevPosition;
                    return;
                }
           
        }
    }
    private bool CheckIsPlayerDead()
    {
        if(this.Status.Hp>0)
        {
            return false;
        }
        return true;
    }
    private void UpdatePrevPos()
    {
        if(_isWallHit)
        {
            _isWallHit = false;
            return;
        }
        _prevPosition = transform.position;
    }
    private void UpdateInvincible()
    {
       if(!_isIncible)
        {
            return;
        }
        
        _invincibleCount += Time.deltaTime;
        if(_invincibleCount>=_invincibleTime)
        {
            _invincibleCount = 0f;
            _isIncible = false;
        }
    }
    private void UpdateShotCoolTime()
    {
        if(!_isShotCoolTime)
        {
            return;
        }
        _shotCoolTimeCount += Time.deltaTime;
        if(_shotCoolTime>_shotCoolTimeCount)
        {
            return;
        }
        _shotCoolTimeCount = 0;
        _isShotCoolTime = false;
    }
   public void SetPlayerFlag()
    {
        _isPlayer = true;
    }
}
