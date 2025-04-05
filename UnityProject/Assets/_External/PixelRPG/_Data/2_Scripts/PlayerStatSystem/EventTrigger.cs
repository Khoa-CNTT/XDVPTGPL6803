using UnityEngine;
using UnityEngine.SceneManagement;

public class EventTrigger : MonoBehaviour
{
   public void ChangeScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
