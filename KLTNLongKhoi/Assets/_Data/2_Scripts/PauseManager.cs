using StarterAssets;
using UnityEngine;
using UnityEngine.Events;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    [SerializeField] private bool isPaused = false;

    [Header("Events")]
    public UnityEvent<bool> onGamePaused;

    public bool IsPaused => isPaused;

    StarterAssetsInputs inputs;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        inputs = FindFirstObjectByType<StarterAssetsInputs>();
    }

    void Update()
    {
        if (inputs.escape != isPaused)
        {
            TogglePause(inputs.escape);
        }
    }

    public void TogglePause(bool value)
    {
        isPaused = value;
        if (value)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        inputs.escape = true;
        isPaused = true;

        onGamePaused?.Invoke(true);
    }

    public void ResumeGame()
    {
        inputs.escape = false;
        isPaused = false;

        onGamePaused?.Invoke(false);
    }
}
