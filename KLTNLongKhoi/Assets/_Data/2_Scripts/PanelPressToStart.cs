using UnityEngine;

public class PanelPressToStart : MonoBehaviour
{
    private SceneLoader sceneLoader;

    private void Start()
    {
        sceneLoader = FindFirstObjectByType<SceneLoader>();
    }

    private void Update()
    {
        // nhấn bất kỳ phím để chuyển level
        if (Input.anyKeyDown)
        {
            LoadNextLevel();
        }
    }

    private void LoadNextLevel()
    {
        sceneLoader.LoadNextLevel();
    }
}
