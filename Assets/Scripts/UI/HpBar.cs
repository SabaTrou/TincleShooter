using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    private Slider _hpSlider = default;
    private bool _isSet=default;
    private void Awake()
    {
        if(!this.gameObject.TryGetComponent<Slider>(out Slider slider))
        {
            _isSet = false;
           
            return;
        }
        
        _isSet = true;
        _hpSlider = slider;
    }

    /// <summary>
    /// キャラクターの情報を取得
    /// </summary>
    /// <param name="character"></param>
    public void SetCharacter(PlayerCharacter character)
    {
        if(!_isSet)
        {
            return;
        }
        _hpSlider.maxValue = character.Status.MaxHp;
        _hpSlider.value = character.Status.Hp;
        _hpSlider.minValue = 0;
    }
    /// <summary>
    /// hpの値を適用
    /// </summary>
    /// <param name="hp"></param>
    public void SetHpValue(int hp)
    {
        if(!_isSet)
        {
            return;
        }
        _hpSlider.value = hp;
    }
}
