using MessagePipe;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;
using VContainer;
using System.Linq;

/// <summary>
/// �e�Ǘ��N���X
/// </summary>
public class BulletsUpdater
{
    #region�@�ϐ�
    public List<BaseBullet> baseBullets { get => _baseBullets; }//�v���p�e�B
    //�e�̃��X�g
    private List<BaseBullet> _baseBullets = new();
    //messagePipe
    private ISubscriber<BulletAddEvent> _bulletAddEvent;
    //
    #endregion
    #region DI       
    //DI
    [Inject]
    private void BulletAdd(ISubscriber<BulletAddEvent> subscriber)
    {
        _bulletAddEvent = subscriber;
        _bulletAddEvent.Subscribe(OnAddedBullet);

    }
    #endregion
    #region Update
    public void BulletsUpdate()
    {
        foreach (BaseBullet bullet in _baseBullets)
        {
            bullet.BulletUpdate();
        }
    }
    #endregion
    /// <summary>
    /// �e�̒ǉ��ʒm�������^�C�~���O�ŌĂ΂��
    /// </summary>
    /// <param name="addEvent"></param>
    private void OnAddedBullet(BulletAddEvent addEvent)
    {
        //�Ǘ��Ώۂɒǉ�
        addObserveBullet(addEvent.baseBullet);
        
    }
    //�Ǘ�����e��ǉ�
    private void addObserveBullet(BaseBullet bullet)
    {
        _baseBullets.Add(bullet);
    }
    
   

}
