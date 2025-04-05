using UnityEngine;

// Rotation this follow mouse drag rotation follow y axis
public class RotationFollowMouseDrag : MonoBehaviour
{
    [SerializeField] private bool isRotation360 = false;
    [SerializeField] private bool isRotationYAxis = false;
    [SerializeField] private float Speed = 100;
    private bool isRotating = false;
    private float startMousePosition = 0;
    private Vector3 mPrevPos = Vector3.zero;
    private Vector3 mPosDelta = Vector3.zero;

    private void Update()
    {
        if (isRotation360)
        {
            RotationFollow360();
        }
        else if (isRotationYAxis)
        {
            RotationFollowYAxis();
        }
    }

    private void RotationFollow360()
    {
        if (Input.GetMouseButton(0))
        {
            mPosDelta = Input.mousePosition - mPrevPos;

            if (Vector3.Dot(transform.up, Vector3.up) >= 0)
            {
                transform.Rotate(transform.up, -Vector3.Dot(mPosDelta, Camera.main.transform.right) * Speed * Time.deltaTime, Space.World);
            }
            else
            {
                transform.Rotate(transform.up, Vector3.Dot(mPosDelta, Camera.main.transform.right) * Speed * Time.deltaTime, Space.World);
            }

            transform.Rotate(Camera.main.transform.right, Vector3.Dot(mPosDelta, Camera.main.transform.up) * Speed * Time.deltaTime, Space.World);
        }

        mPrevPos = Input.mousePosition;
    }

    private void RotationFollowYAxis()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isRotating = true;
            startMousePosition = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isRotating = false;
        }

        if (isRotating)
        {
            float currentMousePosition = Input.mousePosition.x;
            float mouseMovement = currentMousePosition - startMousePosition;

            // Xoay đối tượng
            transform.Rotate(Vector3.up, -mouseMovement * Speed * Time.deltaTime);
            startMousePosition = currentMousePosition;
        }
    }
}

