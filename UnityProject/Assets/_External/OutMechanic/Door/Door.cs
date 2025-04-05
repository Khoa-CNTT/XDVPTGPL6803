using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] float rotationSpeed = 300f;
    [SerializeField] Transform rotationPoint;
    [SerializeField] string correctPassword = "1234"; // Mật khẩu đúng
    [SerializeField] Vector3 rotationAxis = Vector3.up; // Trục xoay
    [SerializeField] float closedAngle = 0f; // Góc đóng cửa
    [SerializeField] float openAngle = 150f; // Góc mở cửa
    [SerializeField] bool useFKey = false;
    private bool isOpen = false; // Trạng thái cửa
    private Coroutine currentRotationCoroutine = null; // Biến để theo dõi Coroutine hiện tại 

    void Update()
    {
        if (useFKey) NhanFDeTest();
    }

    private void NhanFDeTest()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TriggerDoor();
        }
    }

    public void Interact()
    {
        TriggerDoor();
    }

    public void TriggerDoor()
    {
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    public void OpenDoor(string password)
    {
        if (password == correctPassword)
        {
            OpenDoor();
        }
        else
        {
            Debug.Log("Sai mật khẩu!"); // Thông báo sai mật khẩu
        }
    }

    public void OpenDoor()
    {
        if (currentRotationCoroutine != null) StopCoroutine(currentRotationCoroutine);
        StartRotation(openAngle, true);
        isOpen = true;
    }

    public void CloseDoor()
    {
        if (currentRotationCoroutine != null) StopCoroutine(currentRotationCoroutine);
        StartRotation(closedAngle, false);
        isOpen = false;
    }

    public void StartRotation(float toAngle, bool clockwise)
    {
        currentRotationCoroutine = StartCoroutine(RotateDoorCoroutine(toAngle, clockwise));
    }

    private IEnumerator RotateDoorCoroutine(float toAngle, bool clockwise)
    {
        float direction = clockwise ? 1f : -1f;
        float startAngle = rotationPoint.rotation.eulerAngles.y;
        float elapsedTime = 0f;
        float duration = Mathf.Abs(toAngle - startAngle) / rotationSpeed; // Thời gian để hoàn thành chuyển động

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float smoothStep = Mathf.SmoothStep(0, 1, t); // Sử dụng SmoothStep để tạo chuyển động mượt
            float currentAngle = Mathf.Lerp(startAngle, toAngle, smoothStep);
            rotationPoint.rotation = Quaternion.AngleAxis(currentAngle, rotationAxis); // Sử dụng trục xoay
            yield return null; // Chờ đến frame tiếp theo
        }
        // Đảm bảo cửa đạt đúng góc cuối cùng
        rotationPoint.rotation = Quaternion.AngleAxis(toAngle, rotationAxis); // Sử dụng trục xoay
    }
}

