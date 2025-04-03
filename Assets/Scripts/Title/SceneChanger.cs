using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    private AsyncOperation async = new AsyncOperation();


    [SerializeField, HideInInspector]
    private string _titleSceneName = default;
    [SerializeField, HideInInspector]
    private string _gameSceneName = default;
    [SerializeField, HideInInspector]
    private string _clearSceneName = default;
    [SerializeField, HideInInspector]
    private string _gameOverSceneName = default;
    /// <summary>
    /// �^�C�g���V�[���̃��[�h
    /// </summary>
    public void LoadTitleScene()
    {
        StartCoroutine(LoadScene(_titleSceneName));
    }


    /// <summary>
    /// �X�^�[�g�V�[���̃��[�h
    /// </summary>
    public void LoadGameScene()
    {
        StartCoroutine(LoadScene(_gameSceneName));
    }
    /// <summary>
    /// �N���A�V�[���̃��[�h
    /// </summary>
    public void LoadClearScene()
    {
        StartCoroutine(LoadScene(_clearSceneName));
    }
    /// <summary>
    /// �Q�[���I�[�o�[�V�[���̃��[�h
    /// </summary>
    public void LoadGameOverScene()
    {
        StartCoroutine(LoadScene(_gameOverSceneName));
    }

    public void EndGame()
    {


#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
        Application.Quit();//�Q�[���v���C�I��
#endif


    }
    

#if UNITY_EDITOR
    [Header("�J�ڐ�V�[��(�^�C�g��)")]
    [SerializeField]
    private UnityEditor.SceneAsset _titleScene = default;


    [Header("�J�ڐ�V�[��(�Q�[��)")]
    [SerializeField]
    private UnityEditor.SceneAsset _gameScene = default;
    [Header("�J�ڐ�V�[��(�N���A)")]
    [SerializeField]
    private UnityEditor.SceneAsset _clearScene = default;
    [Header("�J�ڐ�V�[��(�Q�[���I�[�o�[)")]
    [SerializeField]
    private UnityEditor.SceneAsset _gameOverScene = default;
    private void OnValidate()
    {
        if (_titleScene != null)
        {
            _titleSceneName = _titleScene.name;
        }
        if (_gameScene != null)
        {
            _gameSceneName = _gameScene.name;
        }
        if (_clearScene != null)
        {
            _clearSceneName = _clearScene.name;
        }
        if (_gameOverScene != null)
        {
            _gameOverSceneName = _gameOverScene.name;
        }
    }
#endif

    private IEnumerator LoadScene(string sceneName)
    {
        Time.timeScale = 1.0f;
        async = SceneManager.LoadSceneAsync(sceneName);
        Scene activeScene = SceneManager.GetActiveScene();
        async.priority = 10;
        while (!async.isDone)
        {
            print(async.isDone);

            if (async.isDone)
            {
                print("in");
            }
            print("nowLoading" + this.gameObject + async.progress + "" + sceneName);
            yield return null;

        }
        SceneManager.UnloadSceneAsync(activeScene);
        //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        //print("in");

    }

}
