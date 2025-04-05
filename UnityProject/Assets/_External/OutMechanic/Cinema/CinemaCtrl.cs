using UnityEngine;
using UnityEngine.Playables;

public class CinemaCtrl : MonoBehaviour
{
    public PlayableDirector playableDirector;

    [ContextMenu("PlayPlayableDirector")]
    void PlayPlayableDirector()
    {
        if (playableDirector != null)
        {
            playableDirector.Play();
        }
        else
        {
            Debug.LogWarning("PlayableDirector chưa được gán!");
        }
    }
}


