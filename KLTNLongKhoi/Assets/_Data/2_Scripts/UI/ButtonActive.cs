using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonActive : MonoBehaviour
{
    public UnityEvent onClick;
    public List<GameObject> objectActive;
    public List<GameObject> objectUnActive;

    void Start()
    {
        GetComponent<UnityEngine.UI.Button>()?.onClick.AddListener(Active);
    }

    public void Active()
    {
        onClick.Invoke(); // Thêm event hoặc callback nếu cần
        foreach (GameObject obj in objectActive)
        {
            obj.SetActive(true);
        }

        foreach (GameObject obj in objectUnActive)
        {
            if (obj == null)
            {
                Debug.LogWarning("Object is null", transform);
                continue;
            }
            obj.SetActive(false);

        }

    }
}
