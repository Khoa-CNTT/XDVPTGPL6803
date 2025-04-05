using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HieuDev
{
    /// <summary> Hiện ứng chuyển cảnh </summary>
    public class TransEffect : MonoBehaviour
    {
        [SerializeField] Animator _anim;
        [SerializeField] string _nextSceneName;
        [SerializeField] float transitionTime = 1f;

        public void LoadNextLevel(string nextSceneName)
        {
            _nextSceneName = nextSceneName;
            LoadNextLevel();
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
