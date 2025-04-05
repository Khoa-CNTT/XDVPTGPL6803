using UnityEngine;
using UnityEngine.UI;

public class btnQuitGame : MonoBehaviour
{
    Button thisButton;

    void Start()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(QuitGame);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
