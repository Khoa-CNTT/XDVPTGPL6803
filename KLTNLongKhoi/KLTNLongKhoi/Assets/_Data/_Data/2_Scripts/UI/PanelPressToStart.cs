using UnityEngine;

public class PanelPressToStart : MonoBehaviour
{
    private SceneLoader sceneLoader;
    [SerializeField] AudioSource audioSource;

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
            audioSource.Play();
        }
    }

    private void LoadNextLevel()
    {
        sceneLoader.LoadNextLevel();
    }
}
