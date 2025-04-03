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

        //register登録
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
        //MessagePipeの設定
        MessagePipeOptions pipeOptions = builder.RegisterMessagePipe();
        //キャラクターの生成をリクエストするときに使う
        builder.RegisterMessageBroker<CharacterInstantiateRequestEvent>(pipeOptions);
        //キャラクターを追加したときによぶ
        builder.RegisterMessageBroker<CharacterAddEvent>(pipeOptions);
        //コントローラを追加したとき
        builder.RegisterMessageBroker<ControllerAddEvent>(pipeOptions);
        //弾の発射要求をされたとき
        builder.RegisterMessageBroker<BulletShotRequestEvent>(pipeOptions);
        //弾が追加された時
        builder.RegisterMessageBroker<BulletAddEvent>(pipeOptions);
        //弾の生成要求をされたとき
        builder.RegisterMessageBroker<BulletInstantiateRequestEvent>(pipeOptions);
        //ICollisionableが追加されたとき
        builder.RegisterMessageBroker<CollisionableAddEvent>(pipeOptions);
        //敵のスポーンリクエスト
        builder.RegisterMessageBroker<EnemySpawnRequestEvent>(pipeOptions);
        //敵が死んだとき
        builder.RegisterMessageBroker<EnemyDeadEvent>(pipeOptions);
        //キャラクターの位置を変えたいとき
        builder.RegisterMessageBroker<CharacterMoveRequetEvent>(pipeOptions);
        //AIの追加通知
        builder.RegisterMessageBroker<AiLogicAddEvent>(pipeOptions);
        //Aiの生成要求
        builder.RegisterMessageBroker<AiLogicRequestEvent>(pipeOptions);
        //ステージ難易度適用通知
        builder.RegisterMessageBroker<StageDifficultySetEvent>(pipeOptions);
        //ロジック難易度適用通知
        builder.RegisterMessageBroker<LogicDifficultySetEvent>(pipeOptions);
        //敵のリサイクル可能通知
        builder.RegisterMessageBroker<EnemyReSycleEvent>(pipeOptions);
        //プレイヤーの追加通知
        builder.RegisterMessageBroker<PlayerAddEvent>(pipeOptions);
        //スコア追加通知
        builder.RegisterMessageBroker<ScoreAddEvent>(pipeOptions);
        //プレイヤー死亡通知
        builder.RegisterMessageBroker<PlayerDeadEvent>(pipeOptions);

        builder.RegisterMessageBroker<EnemyTakeDamageEvent>(pipeOptions);
        #endregion




    }


}
