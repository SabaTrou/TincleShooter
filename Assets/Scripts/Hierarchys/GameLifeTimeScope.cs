using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using MessagePipe;
using VContainer.Unity;

public class GameLifeTimeScope : LifetimeScope
{
    [SerializeField]
    private CharacterSelector _selector;

    [SerializeField]
    private UIMessenger _uiManager;
    [SerializeField]
    private EnemyHolder _enemyHolder;
    [SerializeField]
    private SceneChanger _sceneChanger;
    [SerializeField]
    private EffectService _particleService;

    protected override void Configure(IContainerBuilder builder)
    {
        #region RegisterComponent

        //register�o�^
        if (_selector != null)
        {
            builder.RegisterComponent(_selector);
        }

        if (_uiManager != null)
        {
            builder.RegisterComponent(_uiManager);
        }
        if (_enemyHolder != null)
        {
            builder.RegisterComponent(_enemyHolder);
        }
        if(_sceneChanger!=null)
        {
            builder.RegisterComponent(_sceneChanger);
        }
        if(_particleService!=null)
        {
            builder.RegisterComponent(_particleService);
        }
        #endregion
        #region Register
        builder.Register<CharacterUpdater>(Lifetime.Singleton);
        builder.Register<BulletsUpdater>(Lifetime.Singleton);
        builder.Register<BulletPool>(Lifetime.Singleton);
        builder.Register<CollisionChecker>(Lifetime.Singleton);
        builder.Register<CollisionableUpdater>(Lifetime.Singleton);
        builder.Register<ControllerUpdater>(Lifetime.Singleton);
        builder.Register<EnemySearchService>(Lifetime.Singleton);
        builder.Register<IScorePropertyHolder, ScoreHolder>(Lifetime.Singleton);
        #endregion
        #region RegisterEntryPoint

        builder.RegisterEntryPoint<ControllerProvider>(Lifetime.Singleton);

        builder.RegisterEntryPoint<EnemySpawnService>(Lifetime.Singleton);

        builder.RegisterEntryPoint<CharacterInstantiateService>(Lifetime.Singleton);

        builder.RegisterEntryPoint<BulletInstantiateService>(Lifetime.Singleton);

        builder.RegisterEntryPoint<CollisionableProvider>(Lifetime.Singleton);

        builder.RegisterEntryPoint<GameManager>(Lifetime.Singleton);

        builder.RegisterEntryPoint<EnemyDeadMoveService>(Lifetime.Singleton);

        builder.RegisterEntryPoint<CharacterMoveService>(Lifetime.Singleton);

        builder.RegisterEntryPoint<EnemySearchService>(Lifetime.Singleton);

        builder.RegisterEntryPoint<AiLogicCreater>(Lifetime.Singleton);

        builder.RegisterEntryPoint<BulletShooter>(Lifetime.Singleton);

        builder.RegisterEntryPoint<CharacterSeter>(Lifetime.Singleton);

        builder.RegisterEntryPoint<EnemyStackProvider>(Lifetime.Singleton);

        builder.RegisterEntryPoint<PlayerCharacterProvider>(Lifetime.Singleton);

        builder.RegisterEntryPoint<ScoreProvider>(Lifetime.Singleton);

        builder.RegisterEntryPoint<GameLoopService>(Lifetime.Singleton);

        
       
        #endregion
        #region MessagePipe
        //MessagePipe�̐ݒ�
        MessagePipeOptions pipeOptions = builder.RegisterMessagePipe();
        //�L�����N�^�[�̐��������N�G�X�g����Ƃ��Ɏg��
        builder.RegisterMessageBroker<CharacterInstantiateRequestEvent>(pipeOptions);
        //�L�����N�^�[��ǉ������Ƃ��ɂ��
        builder.RegisterMessageBroker<CharacterAddEvent>(pipeOptions);
        //�R���g���[����ǉ������Ƃ�
        builder.RegisterMessageBroker<ControllerAddEvent>(pipeOptions);
        //�e�̔��˗v�������ꂽ�Ƃ�
        builder.RegisterMessageBroker<BulletShotRequestEvent>(pipeOptions);
        //�e���ǉ����ꂽ��
        builder.RegisterMessageBroker<BulletAddEvent>(pipeOptions);
        //�e�̐����v�������ꂽ�Ƃ�
        builder.RegisterMessageBroker<BulletInstantiateRequestEvent>(pipeOptions);
        //ICollisionable���ǉ����ꂽ�Ƃ�
        builder.RegisterMessageBroker<CollisionableAddEvent>(pipeOptions);
        //�G�̃X�|�[�����N�G�X�g
        builder.RegisterMessageBroker<EnemySpawnRequestEvent>(pipeOptions);
        //�G�����񂾂Ƃ�
        builder.RegisterMessageBroker<EnemyDeadEvent>(pipeOptions);
        //�L�����N�^�[�̈ʒu��ς������Ƃ�
        builder.RegisterMessageBroker<CharacterMoveRequetEvent>(pipeOptions);
        //AI�̒ǉ��ʒm
        builder.RegisterMessageBroker<AiLogicAddEvent>(pipeOptions);
        //Ai�̐����v��
        builder.RegisterMessageBroker<AiLogicRequestEvent>(pipeOptions);
        //�X�e�[�W��Փx�K�p�ʒm
        builder.RegisterMessageBroker<StageDifficultySetEvent>(pipeOptions);
        //���W�b�N��Փx�K�p�ʒm
        builder.RegisterMessageBroker<LogicDifficultySetEvent>(pipeOptions);
        //�G�̃��T�C�N���\�ʒm
        builder.RegisterMessageBroker<EnemyReSycleEvent>(pipeOptions);
        //�v���C���[�̒ǉ��ʒm
        builder.RegisterMessageBroker<PlayerAddEvent>(pipeOptions);
        //�X�R�A�ǉ��ʒm
        builder.RegisterMessageBroker<ScoreAddEvent>(pipeOptions);
        //�v���C���[���S�ʒm
        builder.RegisterMessageBroker<PlayerDeadEvent>(pipeOptions);

        builder.RegisterMessageBroker<EnemyTakeDamageEvent>(pipeOptions);
        #endregion




    }


}
