using UnityEngine;

public class LookToCam : MonoBehaviour
{
    // Tham chiếu đến camera
    public Camera targetCamera;

    private void Start()
    {
        targetCamera = FindObjectOfType<Camera>();
    }

    private void FixedUpdate()
    {
        if (targetCamera)
        {
            transform.LookAt(transform.position - targetCamera.transform.position);
        }

        // Kiểm tra xem camera có nhìn trúng đối tượng không
        // if (IsObjectVisible(targetCamera, gameObject))
        // {
        //     Debug.Log("Camera đang nhìn trúng đối tượng!");
        // }
    }

    private bool IsObjectVisible(Camera cam, GameObject obj)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        return GeometryUtility.TestPlanesAABB(planes, obj.GetComponent<Renderer>().bounds);
    }
}

