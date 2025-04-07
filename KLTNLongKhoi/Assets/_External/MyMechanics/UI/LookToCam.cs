using UnityEngine;

public class LookToCam : MonoBehaviour
{
    // Tham chiếu đến camera
    public Camera targetCamera;

    private void Start()
    {
        targetCamera = FindFirstObjectByType<Camera>();
    }

    private void FixedUpdate()
    {
        LookToCamera();
    }

    private void LookToCamera()
    {
        if (targetCamera)
        {
            transform.LookAt(targetCamera.transform.position);
        }
    }
}

