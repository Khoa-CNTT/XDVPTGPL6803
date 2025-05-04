using UnityEngine;

public class LucTacDong : MonoBehaviour
{
    public float khoiLuong = 10.0f; // Đơn vị: kg
    private Vector3 viTriTruocDo;
    private Vector3 vanTocTruocDo;
    private float thoiGianTruocDo;

    // Các biến bổ sung cho hàm CheckCollision
    private Vector3 previousPosition;
    public float assumedMass = 10.0f; // Khối lượng giả định
    public float impactThreshold = 5.0f; // Ngưỡng lực tác động
    public LayerMask collisionMask; // Lớp va chạm

    void Start()
    {
        viTriTruocDo = transform.position;
        vanTocTruocDo = Vector3.zero;
        thoiGianTruocDo = Time.time;
        previousPosition = transform.position; // Khởi tạo vị trí trước đó
    }

    void Update()
    {
        float giaToc = TinhGiaToc(transform.position, Time.time);
        float lucTacDong = TinhLucTacDong(giaToc);
        // Debug.Log("Lực tác động: " + lucTacDong);

        CheckCollision(); // Gọi hàm CheckCollision trong Update

        // Cập nhật vị trí trước đó
        previousPosition = transform.position;
    }

    private float TinhGiaToc(Vector3 viTriHienTai, float thoiGianHienTai)
    {
        float deltaThoiGian = thoiGianHienTai - thoiGianTruocDo;

        Vector3 vanTocHienTai = (viTriHienTai - viTriTruocDo) / deltaThoiGian;
        Vector3 giaTocVector = (vanTocHienTai - vanTocTruocDo) / deltaThoiGian;

        // Cập nhật giá trị cho khung hình tiếp theo
        viTriTruocDo = viTriHienTai;
        vanTocTruocDo = vanTocHienTai;
        thoiGianTruocDo = thoiGianHienTai;

        return giaTocVector.magnitude;
    }

    private float TinhLucTacDong(float giaToc)
    {
        // Công thức F = m * a
        return khoiLuong * giaToc;
    }

    private void CheckCollision()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity, collisionMask);
        if (hitColliders.Length > 0)
        {
            // Sử dụng hàm TinhGiaToc để tính gia tốc
            float giaToc = TinhGiaToc(transform.position, Time.time);

            // Tính toán lực tác động dựa trên gia tốc và khối lượng
            Vector3 impactForce = khoiLuong * giaToc * (transform.position - previousPosition).normalized;

            // Kiểm tra nếu lực tác động vượt qua ngưỡng
            if (impactForce.magnitude > impactThreshold)
            {
                float equivalentMass = impactForce.magnitude / Physics.gravity.magnitude;
                Debug.Log("Lực tác động mạnh: " + impactForce.magnitude + " N, Khối lượng tương đương: " + equivalentMass + " kg");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Lấy tổng xung lực từ va chạm
        Vector3 totalImpulse = Vector3.zero;

        foreach (ContactPoint contact in collision.contacts)
        {
            totalImpulse += contact.normal * collision.impulse.magnitude;
        }

        // Tính toán lực va chạm trung bình
        float impactForce = totalImpulse.magnitude / Time.fixedDeltaTime;

        Debug.Log("Lực va chạm: " + impactForce + " N");
    }

    // void CheckCollision()
    // {
    //     Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity, collisionMask);
    //     if (hitColliders.Length > 0)
    //     {
    //         // Tính toán vận tốc dựa trên sự thay đổi vị trí
    //         Vector3 velocity = (transform.position - previousPosition) / Time.deltaTime;

    //         // Giả định lực tác động dựa trên sự thay đổi vận tốc
    //         Vector3 impactForce = assumedMass * velocity / Time.deltaTime;

    //         // Kiểm tra nếu lực tác động vượt qua ngưỡng
    //         if (impactForce.magnitude > impactThreshold)
    //         {
    //             float equivalentMass = impactForce.magnitude / gravity;
    //             Debug.Log("Lực tác động mạnh: " + impactForce.magnitude + " N, Khối lượng tương đương: " + equivalentMass + " kg");
    //         }
    //     }
    // }
}

