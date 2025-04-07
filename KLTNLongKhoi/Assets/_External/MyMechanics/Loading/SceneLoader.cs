using System;
using System.Collections;
using System.Data.Common;
using HieuDev;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public UnityAction OnStartLoadScene;
    [SerializeField] string sceneNext;
    [SerializeField] TransEffect transEffect;

    private AsyncOperation m_async;

    public void LoadNextLevel()
    {
        LoadSceneName(sceneNext);
    }

    public void LoadSceneName(string nextSceneName)
    {
        m_async = SceneManager.LoadSceneAsync(GameScene.DataHolder.ToString(), LoadSceneMode.Additive);

        if (transEffect)
        {
            transEffect.LoadNextLevel(nextSceneName);
        }
        else if (m_async.isDone)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}

