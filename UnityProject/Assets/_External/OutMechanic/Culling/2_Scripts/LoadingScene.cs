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
        private AsyncOperation m_async; // Bien luu doi tuong AsyncOperation
        public static Action<float> OnLoading; // Su kien hook

        private void Start()
        {
            m_async = SceneManager.LoadSceneAsync(GameScene.DataHolder.ToString(), LoadSceneMode.Additive);
        }

        private void Update()
        {
            OnLoading?.Invoke(m_async.progress);

            // Neu scene DataHolder thuc su duoc load het
            if (m_async.isDone)
            {
                SceneManager.LoadScene(GameScene.Menu.ToString());
            }
        }
    }
}
