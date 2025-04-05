using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    [SerializeField] private bool _dontDestroyOnLoad;

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
            if (_dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetDontDestroyOnLoad(bool value)
    {
        _dontDestroyOnLoad = value;
        if (_dontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
