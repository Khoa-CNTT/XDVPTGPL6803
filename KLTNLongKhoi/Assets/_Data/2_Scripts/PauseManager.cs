using StarterAssets;
using UnityEngine;
using UnityEngine.Events;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    [SerializeField] private bool isPaused = false;

    [Header("Events")]
    public UnityEvent onGamePaused;
    public UnityEvent onGameResumed;

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

        inputs.onEscape.AddListener(TogglePause);
    }

    public void TogglePause(bool value)
    {
        Debug.Log($"TogglePause: {value}");
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
        isPaused = true;
        // Time.timeScale = 0f;
        onGamePaused?.Invoke();
    }

    public void ResumeGame()
    {
        isPaused = false;
        // Time.timeScale = 1f;
        onGameResumed?.Invoke();
    }
}
