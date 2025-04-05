using UnityEngine;

public class TaoLucDay : MonoBehaviour
{
    public float forceAmount = 10f; // Lực đẩy 

    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            // Tạo lực đẩy bằng cách sử dụng Rigidbody
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Vector3.up * forceAmount, ForceMode.Force);
            }
        } 
    }


}