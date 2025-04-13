using StarterAssets;
using UnityEngine;
using UnityEngine.Events;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;

    public UnityEvent<bool> onGamePaused;

    public bool IsPaused => isPaused;

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
        isPaused = true;

        onGamePaused?.Invoke(true);
    }

    public void ResumeGame()
    {
        isPaused = false;

        onGamePaused?.Invoke(false);
    }
}
