using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using MessagePipe;
using VContainer.Unity;

[RequireComponent(typeof(SphereCollider))]
public abstract class BaseCharacter : SeledMonoBehaviour, ICollisionable
{
    public CharacterStatus Status { get => _status; }//プロパティ
    //ステータス
    [SerializeField]
    protected CharacterStatus _status;
    //移動用クラス
    protected Move _move;
   //追加予定のボムクラス
    protected Bomb _bomb;
    //当たり判定自動調整用のコライダー
    protected SphereCollider _circle;
    public float CircleRadius { get => _circle.radius; }
    //当たり判定用のクラス
    protected CircleData _hitCircleData;
    public CheckCollisionMode CheckCollisionMode { get=>_checkCollisionMode; }//プロパティ
    //
    protected CheckCollisionMode _checkCollisionMode = CheckCollisionMode.collisionable;

    [SerializeField]
    private LayerMask _layerMask = default;
    public LayerMask CollisionableLayer { get=>_layerMask; }
   
    //Updateで回すメソッド格納用のデリゲート
    //勉強用なので今後変えるかも
    protected delegate void UpdateDel();
    protected UpdateDel _updateDel;

    public int NameHashCode { get; private set; }

    public IBaseCollisionData BaseData { get => _hitCircleData; }

    protected bool _isHitStop = default;
    [SerializeField]
    private float _hitStopTime = 0.2f;
    private float _hitStopCount = 0f;
    private void Awake()
    {
        NameHashCode = this.gameObject.GetHashCode();
    }
    //Start
    protected override sealed void Start()
    {
        _circle = this.gameObject.GetComponent<SphereCollider>();
        InitializeCircle();
        _updateDel=new(UpdateCircle);
        
        SubCharacterStart();
        

    }
    //サブクラス用のスタート
    protected virtual void SubCharacterStart()
    {

    }
    
    public virtual void Attack()
    {
       
    }

    public virtual void Move(Vector2 vector)
    {
       
        _move.CharacterMove(_status.MoveSpeed, vector);
    }
    public virtual void Bomb()
    {
        _bomb.CharacterBomb();
    }

    protected virtual void Dead()
    {

    }
    /// <summary>
    /// CircleDataの初期化用
    /// </summary>
    protected void InitializeCircle()
    {
        _hitCircleData = new(transform.position.ToVector2XZ(), _circle.radius);

    }
    /// <summary>
    /// CircleDataの更新用
    /// </summary>
    protected void UpdateCircle()
    {
        _hitCircleData.SetData(transform.position.ToVector2XZ(),_circle.radius);
        
    }
    /// <summary>
    /// CharacterUpdate
    /// </summary>
    public void CharacterUpdate()
    {
        if(IsHitStop())
        {
            return;
        }
        _updateDel.Invoke();
        
    }
    private bool IsHitStop()
    {
        if (_isHitStop)
        {
            if (_hitStopCount >= _hitStopTime)
            {
                _hitStopCount = 0;
                _isHitStop = false;
            }
            _hitStopCount += Time.deltaTime;
           
        }
        return _isHitStop;
    }
    public virtual void OnCollisionEvent(ICollisionable collisionable)
    {
       
    }
   
}
