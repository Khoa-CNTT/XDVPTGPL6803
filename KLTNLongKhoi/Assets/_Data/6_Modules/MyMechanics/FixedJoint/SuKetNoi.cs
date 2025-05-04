using UnityEngine;
using System.Collections.Generic;

public class SuKetNoi : MonoBehaviour
{
    public float nguongLucVaCham = 10f; // Ngưỡng lực để ngắt kết nối
    public float nguongMoMenXoay = 10f; // Ngưỡng mômen xoắn để ngắt kết nối
    public List<Transform> danhSachDoiTuongCanKetNoi; // Danh sách các đối tượng cần kết nối
    private List<ConfigurableJoint> danhSachConfigurableJoint = new List<ConfigurableJoint>(); // Danh sách các khớp nối có thể cấu hình

    void Start()
    {
        TaoConfigurableJoints();
    }

    void FixedUpdate()
    {
        // KiemTraLucVaNgatKetNoi();
    }

    // Tạo và cấu hình ConfigurableJoint cho từng đối tượng trong danh sách
    private void TaoConfigurableJoints()
    {
        foreach (Transform doiTuong in danhSachDoiTuongCanKetNoi)
        {
            if (doiTuong != null)
            {
                Rigidbody rbDoiTuong = doiTuong.GetComponent<Rigidbody>();

                // Tạo một ConfigurableJoint và kết nối với doiTuong
                ConfigurableJoint joint = gameObject.AddComponent<ConfigurableJoint>();
                joint.connectedBody = rbDoiTuong;

                // Cấu hình joint để loại bỏ đàn hồi
                joint.xMotion = ConfigurableJointMotion.Locked;
                joint.yMotion = ConfigurableJointMotion.Locked;
                joint.zMotion = ConfigurableJointMotion.Locked;
                joint.angularXMotion = ConfigurableJointMotion.Locked;
                joint.angularYMotion = ConfigurableJointMotion.Locked;
                joint.angularZMotion = ConfigurableJointMotion.Locked;

                // Cải thiện độ ổn định của khớp nối
                joint.projectionMode = JointProjectionMode.PositionAndRotation;
                joint.projectionDistance = 0.05f; // Không cho phép khoảng cách
                joint.projectionAngle = 0.05f; // Không cho phép góc

                // Đặt ngưỡng lực và mômen xoắn để tự động ngắt kết nối
                joint.breakForce = nguongLucVaCham;
                joint.breakTorque = nguongMoMenXoay;

                // Thêm joint vào danh sách
                danhSachConfigurableJoint.Add(joint);
            }
        }
    }

    // Kiểm tra lực tác động và ngắt kết nối nếu vượt ngưỡng cho từng khớp nối
    private void KiemTraLucVaNgatKetNoi()
    {
        for (int i = danhSachConfigurableJoint.Count - 1; i >= 0; i--)
        {
            ConfigurableJoint joint = danhSachConfigurableJoint[i];
            if (joint != null)
            {
                // Lấy lực hiện tại tác động lên khớp nối
                Vector3 force = joint.currentForce;

                // Kiểm tra nếu lực vượt quá ngưỡng
                if (force.magnitude > nguongLucVaCham)
                {
                    // In ra thông báo đối tượng bị lực va đập vượt ngưỡng
                    InRaThongBaoLucVuotNguong(joint, force);

                    // Ngắt kết nối
                    Destroy(joint);
                    danhSachConfigurableJoint.RemoveAt(i);
                }
            }
        }
    }

    // Hàm in ra thông báo khi lực vượt ngưỡng
    private void InRaThongBaoLucVuotNguong(ConfigurableJoint joint, Vector3 force)
    {
        Debug.Log("Đối tượng " + joint.connectedBody.name + " bị lực va đập vượt ngưỡng: " + force.magnitude);
    }
}
