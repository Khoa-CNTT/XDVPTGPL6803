using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace KLTNLongKhoi
{
    public class GameOverPanel : MonoBehaviour
    {
        public void Show()
        { 
            gameObject.SetActive(true);
        }

        public void Hide()
        { 
            gameObject.SetActive(false);
        }


    }
}