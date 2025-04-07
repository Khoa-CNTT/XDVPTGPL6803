using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace HieuDev
{
    /// <summary> Loading khi mới khởi động game </summary>
    public class LoadingProgress : MonoBehaviour
    { 
        private AsyncOperation m_async; 

        private void Start()
        {
            m_async = SceneManager.LoadSceneAsync(GameScene.DataHolder.ToString(), LoadSceneMode.Additive);
        }

        public float getLoadingProgress()
        {
            return m_async.progress;
        }
    }
}
