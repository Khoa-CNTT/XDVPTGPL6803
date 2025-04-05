using UnityEngine;

/// <summary> Hiện ứng chuyển cảnh </summary>
public class SceneTransitionEffect : MonoBehaviour
{
    Animator animator;
    SceneLoader sceneLoader;

    private void Awake()
    {
        sceneLoader = FindFirstObjectByType<SceneLoader>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        sceneLoader.OnStartLoadScene += LoadLevel;
    }

    private void LoadLevel()
    {
        if (animator)
        {
            animator.SetTrigger("Start");
        }
    }

}
