using System.Collections.Generic;
using UnityEngine;

public class OnTriggerThis : MonoBehaviour
{
    [Header("Object Active")]
    [SerializeField] List<GameObject> objectActive;
    [SerializeField] List<GameObject> objectUnActive;

    void Start()
    {
        GetComponent<UnityEngine.UI.Button>()?.onClick.AddListener(OnClickThis);
    }

    public void OnClickThis()
    {
        ActiveObjects();
    }

    public void ActiveObjects()
    {
        foreach (GameObject obj in objectActive)
        {
            obj.SetActive(true);
        }

        foreach (GameObject obj in objectUnActive)
        {
            obj.SetActive(false);
        }
    }

    public void UnActiveObjects()
    {
        foreach (GameObject obj in objectActive)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in objectUnActive)
        {
            obj.SetActive(true);
        }
    }
}
