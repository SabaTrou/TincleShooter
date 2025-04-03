using UnityEngine;
using MessagePipe;
using VContainer;


public class EnemyCharacter : BaseCharacter
{
    [SerializeField]
    private float _takeShotHitBack = 1.5f;
    [SerializeField]
    private int _enemyScore = 10;
    public int EnemyScore { get => _enemyScore; }
    [Inject] private IPublisher<EnemyDeadEvent> _deadEvent;
    [Inject] private IPublisher<CharacterMoveRequetEvent> _moveService;
    [Inject] private IPublisher<EnemyReSycleEvent> _reSycleEvent;
    [Inject] private IPublisher<EnemyTakeDamageEvent> _damageEvent;

    
    private bool _isAreaMoving=default;
    private bool _isMove = default;
    protected override void SubCharacterStart()
    {
        _updateDel += EneMove;
        Debug.Log(CollisionableLayer.value);
    }
    private void EneMove()
    {
        transform.position += Vector3.back * Status.MoveSpeed * Time.deltaTime;
    }
    public void ResetEnemyStatus()
    {
        _status.ResetStatus();
    }
    public override void OnCollisionEvent(ICollisionable collisionable)
    {

        switch (collisionable)
        {
            case BaseBullet bullet:
                {
                    _status.CalclateHp(bullet.Damage);
                    _isHitStop = true;
                    if (_status.Hp <= 0)
                    {
                        if (bullet.ShotPlayer == null)
                        {
                            Debug.Log("player is null");
                        }
                        Dead();
                        _deadEvent.Publish(new EnemyDeadEvent(this, bullet.ShotPlayer));
                    }
                    else
                    {
                        _damageEvent.Publish(new EnemyTakeDamageEvent(this));
                        if (!_isMove)
                        {
                            //‰¼“ü‚ê
                            Vector3 pos = new Vector3(transform.position.x * -1, transform.position.y, transform.position.z + _takeShotHitBack);
                            _moveService.Publish(new CharacterMoveRequetEvent(this, pos));
                            _isMove = true;
                        }
                    }
                    return;
                }
            case PlayerCharacter:
                {

                    _reSycleEvent.Publish(new EnemyReSycleEvent(this, new Vector3(0, 1000, 0)));
                    return;
                }
            case Wall:
                {
                    Debug.Log(collisionable.gameObject.layer);
                    _reSycleEvent.Publish(new EnemyReSycleEvent(this, new Vector3(0, 1000, 0)));
                    

                    return;
                }
        }

    }
    protected override void Dead()
    {
        InitializeHp();
        _isMove = false;

    }
    public void InitializeHp()
    {
        _status.ResetStatus();
    }
}
