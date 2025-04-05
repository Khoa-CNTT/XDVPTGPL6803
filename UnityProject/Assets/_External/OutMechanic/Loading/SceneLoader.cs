using System;
using System.Collections;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("SCENES MANAGER")]
    [SerializeField] float transitionTime = 1f;

    public UnityAction OnStartLoadScene;

    public void LoadSceneMenuOnGameOver()
    {
        // dataManager.OnStartNewGame();
        LoadSceneMenu();
    }

    public void LoadGameScene(SceneName gameScene)
    {
        Debug.Log($"Load scene {gameScene}");
        StartCoroutine(LoadSceneByGameScene(gameScene));
    }

    // delay để hiện ứng chuyển scene chạy
    IEnumerator LoadSceneByGameScene(SceneName gameScene)
    {
        OnStartLoadScene?.Invoke();
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(gameScene.ToString());
    }

    public void LoadSceneMenu() => LoadGameScene(SceneName.Menu);
    public void LoadSceneDemo() => LoadGameScene(SceneName.Demo);

}

