using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace HieuDev
{
    /// <summary> Loading khi mới khởi động game </summary>
    public class LoadingScene : MonoBehaviour
    {
        [SerializeField] string sceneNext;
        [SerializeField] TransEffect transEffect;
        public UnityAction OnStartLoadScene;
        private AsyncOperation m_async;
        private float loadingProgress;

        private void Start()
        {
            m_async = SceneManager.LoadSceneAsync(GameScene.DataHolder.ToString(), LoadSceneMode.Additive);
        }

        private void Update()
        {
            loadingProgress = m_async.progress;

            if (transEffect && loadingProgress >= 0.9f)
            {
                transEffect.LoadNextLevel(sceneNext);
            }
            else if (m_async.isDone)
            {
                SceneManager.LoadScene(sceneNext);
            }
        }

        public float getLoadingProgress()
        {
            return loadingProgress;
        }
    }
}
