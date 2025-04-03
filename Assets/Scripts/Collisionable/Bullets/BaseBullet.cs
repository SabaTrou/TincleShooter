using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public abstract class BaseBullet : MonoBehaviour, ICollisionable
{
    #region 変数
    private CircleData _circleData;
    [SerializeField]
    private LayerMask _layerMask = default;
    private SphereCollider _circle;
    private float _circleRadius = default;
    [SerializeField]
    private bool _isShot = default;
    private float _moveSpeed = default;
    private int _damage;
    private PlayerCharacter _shotPlayer = default;
    #endregion
    #region プロパティ
    public IBaseCollisionData BaseData { get => _circleData; }
    public CheckCollisionMode CheckCollisionMode { get => _checkCollisionMode; }
    private CheckCollisionMode _checkCollisionMode = CheckCollisionMode.collisionable;
    public LayerMask CollisionableLayer { get => _layerMask; }
    public bool IsShot { get => _isShot; }
    public int Damage { get => _damage; }
    public PlayerCharacter ShotPlayer { get => _shotPlayer; }
    #endregion

    protected delegate void UpdateDel();
    //主に情報更新など
    protected UpdateDel _updateDel;
    readonly private Vector3 _afterHitPosition = new Vector3(0, 100, 0);
    // Start is called before the first frame update

    private void Awake()
    {
        _circle = GetComponent<SphereCollider>();
        SetCircle();
        _circleRadius = _circle.radius;
        _updateDel = new(UpdateCircle);
        
    }


    protected virtual void BulletMove()
    {
        //球が撃たれているときは1倍撃たれていなければ0倍
        this.gameObject.transform.position += Vector3.forward * _moveSpeed * Time.deltaTime * Convert.ToInt32(_isShot);
    }
    public void OnCollisionEvent(ICollisionable collisionable)
    {
       
        _isShot = false;
        _checkCollisionMode = CheckCollisionMode.dontCollisionable;
        transform.position = _afterHitPosition;
        //_shotPlayer = null;
    }
    protected void SetCircle()
    {

        _circleData = new(transform.position.ToVector2XZ(),_circleRadius);


    }
    protected void UpdateCircle()
    {

        _circleData.SetData(transform.position.ToVector2XZ(), transform.lossyScale.x*_circleRadius);

    }
    public void BulletUpdate()
    {

        if (_updateDel != null)
        {
            _updateDel.Invoke();
        }
        else
        {
            Debug.LogError("null");
        }
        UpdateCircle();
        BulletMove();
    }

    public void ShotBullet(Vector3 vector3,PlayerCharacter shotPlayer, float moveSpeed, int damage)
    {

        this.transform.position = vector3;
        _isShot = true;
        _checkCollisionMode = CheckCollisionMode.collisionable;
        _moveSpeed = moveSpeed;
        _damage = damage;
        _shotPlayer = shotPlayer;

    }
}
