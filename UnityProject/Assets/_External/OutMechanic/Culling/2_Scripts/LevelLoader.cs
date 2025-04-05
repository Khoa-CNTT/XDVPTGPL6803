using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HieuDev
{
    /// <summary> Hiện ứng chuyển cảnh </summary>
    public class LevelLoader : MonoBehaviour
    {
        public Animator _anim;
        public string _nextSceneName;
        public float transitionTime = 1f;

        private void Start()
        {
            // DontDestroyOnLoad(gameObject);
        }

        public void LoadNextLevel()
        {
            StartCoroutine(LoadLevel(_nextSceneName));
        }

        IEnumerator LoadLevel(String sceneName)
        {
            _anim.SetTrigger("Start");
            yield return new WaitForSeconds(transitionTime);
            SceneManager.LoadScene(sceneName);
        }


    }
}
