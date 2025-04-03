using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using MessagePipe;
using VContainer.Unity;

[RequireComponent(typeof(SphereCollider))]
public abstract class BaseCharacter : SeledMonoBehaviour, ICollisionable
{
    public CharacterStatus Status { get => _status; }//�v���p�e�B
    //�X�e�[�^�X
    [SerializeField]
    protected CharacterStatus _status;
    //�ړ��p�N���X
    protected Move _move;
   //�ǉ��\��̃{���N���X
    protected Bomb _bomb;
    //�����蔻�莩�������p�̃R���C�_�[
    protected SphereCollider _circle;
    public float CircleRadius { get => _circle.radius; }
    //�����蔻��p�̃N���X
    protected CircleData _hitCircleData;
    public CheckCollisionMode CheckCollisionMode { get=>_checkCollisionMode; }//�v���p�e�B
    //
    protected CheckCollisionMode _checkCollisionMode = CheckCollisionMode.collisionable;

    [SerializeField]
    private LayerMask _layerMask = default;
    public LayerMask CollisionableLayer { get=>_layerMask; }
   
    //Update�ŉ񂷃��\�b�h�i�[�p�̃f���Q�[�g
    //�׋��p�Ȃ̂ō���ς��邩��
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
    //�T�u�N���X�p�̃X�^�[�g
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
    /// CircleData�̏������p
    /// </summary>
    protected void InitializeCircle()
    {
        _hitCircleData = new(transform.position.ToVector2XZ(), _circle.radius);

    }
    /// <summary>
    /// CircleData�̍X�V�p
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
