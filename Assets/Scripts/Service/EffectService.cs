using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using MessagePipe;
using static UnityEngine.ParticleSystem;
using CartoonFX;
using System.Runtime.CompilerServices;


[RequireComponent(typeof(AudioSource))]
public class EffectService:MonoBehaviour
{
    #region Values
    private ParticlePool _Pool=new();
    private ParticlePrefabValue _shotValue;
    private ParticlePrefabValue _hitValue;
    private ParticlePrefabValue _deadValue;
    private ParticlePrefabValue _moveValue;
    private AudioSource _audioSource;
    #region serialize
    [SerializeField]
    private ParticleSystem _shotParticle;
    [SerializeField]
    private AudioClip _shotClip;
    [SerializeField]
    private ParticleSystem _hitParticle;
    [SerializeField]
    private AudioClip _hitClip;
    [SerializeField]
    private ParticleSystem _deadParticle;
    [SerializeField]
    private AudioClip _deadClip;
    [SerializeField]
    private ParticleSystem _moveParticle;
    [SerializeField]
    private AudioClip _moveClip;
    #endregion serialize
    #region pipe
    private ISubscriber<EnemyDeadEvent> _enemyDeadEvent;
    private ISubscriber<BulletShotRequestEvent> _shotReqest;
    private ISubscriber<PlayerTakeDamageEvent> _playerDamageEvent;
    private ISubscriber<EnemyTakeDamageEvent> _enemyDamageEvent;
    private ISubscriber<CharacterMoveRequetEvent> _moveReqest;
    #endregion pipe
    #endregion values
    #region DIMethods
    [Inject]
    private void EnemyDead(ISubscriber<EnemyDeadEvent> subscriber)
    {
        _enemyDeadEvent = subscriber;
        _enemyDeadEvent.Subscribe(OnEnemyDead);
    }
    [Inject]
    private void BulletShotRequest(ISubscriber<BulletShotRequestEvent> subscriber)
    {
        _shotReqest = subscriber;
        _shotReqest.Subscribe(OnBulletShot);
    }
    [Inject]
    private void PlayerTakeDamage(ISubscriber<PlayerTakeDamageEvent> subscriber)
    {
        _playerDamageEvent = subscriber;
        _playerDamageEvent.Subscribe(OnPlayerTakeDamage);
    }
    [Inject]
    private void EnemyTakeDamage(ISubscriber<EnemyTakeDamageEvent> subscriber)
    {
        _enemyDamageEvent = subscriber;
        _enemyDamageEvent.Subscribe(OnEnemyTakeDamage);
    }
    [Inject]
    private void CharacterMoveRequest(ISubscriber<CharacterMoveRequetEvent> subscriber)
    {
        _moveReqest = subscriber;
        _moveReqest.Subscribe(OnMoveRequest);
    }
    #endregion
    #region Event
    private void OnEnemyDead(EnemyDeadEvent deadEvent)
    {
        PlayParticle(_deadValue,deadEvent.enemyCharacter.transform.position);
        PlayAudioClip(_deadClip);
    }
    private void OnBulletShot(BulletShotRequestEvent requestEvent)
    {
        PlayParticle(_shotValue,requestEvent.position);
        PlayAudioClip(_shotClip);
    }
    private void OnPlayerTakeDamage(PlayerTakeDamageEvent damageEvent)
    {
        PlayParticle(_hitValue,damageEvent.playerCharacter.transform.position);
        PlayAudioClip(_hitClip);
    }
    private void OnEnemyTakeDamage(EnemyTakeDamageEvent damageEvent)
    {
        PlayParticle(_hitValue, damageEvent.character.transform.position);
        PlayAudioClip(_hitClip);
    }
    private void OnMoveRequest(CharacterMoveRequetEvent requetEvent)
    {
        PlayParticle(_moveValue, requetEvent.character.transform.position);
        PlayAudioClip(_moveClip);
    }
    #endregion
    private void Start()
    {
        if(_shotParticle!=null)
        {
            InitParticle(_shotParticle);
            _shotValue = new ParticlePrefabValue(_shotParticle);
        }
        if(_hitParticle!=null)
        {
            InitParticle(_hitParticle);
            _hitValue = new ParticlePrefabValue(_hitParticle);
        }
        if(_deadParticle!=null)
        {
            InitParticle(_deadParticle);
            _deadValue = new ParticlePrefabValue(_deadParticle);
        }
        if(_moveParticle!=null)
        {
            InitParticle(_moveParticle);
            _moveValue = new(_moveParticle);
        }
        _audioSource = gameObject.GetComponent<AudioSource>();
    }
    [MethodImpl(MethodImplOptions.NoInlining)]
    private void PlayAudioClip(AudioClip clip)
    {
        if(clip==null)
        {
            return;
        }
        _audioSource.PlayOneShot(clip);
    }
    /// <summary>
    /// playParticle
    /// </summary>
    /// <param name="prefabValue"></param>
    /// <param name="pos"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void PlayParticle(ParticlePrefabValue prefabValue,Vector3 pos)
    {
        if (prefabValue == null)
        {
            return;
        }
        ParticleSystem particle = _Pool.GetParticle(prefabValue);
        particle.transform.position = pos;
        particle.Play();
    }
    #region init
    /// <summary>
    /// 
    /// </summary>
    /// <param name="particle"></param>
    private void InitParticle(ParticleSystem particle)
    {
        MainModule main = particle.main;
        main.stopAction = ParticleSystemStopAction.None;
        if(!particle.gameObject.TryGetComponent<CFXR_Effect>(out CFXR_Effect cfxr))
        {
            return;
        }
        cfxr.clearBehavior=CFXR_Effect.ClearBehavior.None;
    }
    #endregion
}


