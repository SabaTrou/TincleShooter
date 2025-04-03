using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreBoard : MonoBehaviour
{
    private Text _scoreText = default;
    private bool _isSet=default;
    private string _scoreTextTmplate = "SCORE: ";
    private void Awake()
    {
        if(!this.gameObject.TryGetComponent<Text>(out Text text))
        {
            
            _isSet = false;
            return;
        }
        _scoreText = text;
        _scoreText.text = _scoreTextTmplate+"0";
        _isSet = true;
    }
    public void SetScore(int score)
    {
       
        if(!_isSet)
        {
            return;
        }
        _scoreText.text = _scoreTextTmplate + score.ToString();
    }
}
