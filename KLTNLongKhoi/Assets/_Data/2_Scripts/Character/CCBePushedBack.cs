using UnityEngine;

public class CCBePushedBack : MonoBehaviour
{
    [Header("Push Back Settings")]
    [SerializeField] private float pushForce = 10f;
    [SerializeField] private float damageForceMultiplier = 0.5f;
    [SerializeField] private float recoveryTime = 0.5f;
    [SerializeField] private float gravity = -9.81f; // Thêm gravity

    private CharacterController controller;
    private bool isPushed = false;
    private float pushTimer = 0f;
    private Vector3 pushVelocity;
    private float verticalVelocity;
    private bool isGrounded;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("CCBePushedBack requires a CharacterController component!");
        }
    }

    private void Update()
    {
        // Kiểm tra ground
        isGrounded = controller.isGrounded;

        if (isPushed)
        {
            pushTimer += Time.deltaTime;
            if (pushTimer >= recoveryTime)
            {
                isPushed = false;
                pushTimer = 0f;
                pushVelocity = Vector3.zero;
                verticalVelocity = 0f;
            }
            else
            {
                // Xử lý trọng lực
                if (isGrounded && verticalVelocity < 0)
                {
                    verticalVelocity = -2f; // Reset vertical velocity khi chạm đất
                }
                else
                {
                    verticalVelocity += gravity * Time.deltaTime;
                }

                // Tính toán movement với decay effect
                float pushProgress = 1 - (pushTimer / recoveryTime);
                Vector3 movement = pushVelocity * pushProgress;

                // Kết hợp movement ngang với movement dọc
                movement.y = verticalVelocity;

                // Apply movement
                if (controller.enabled == true) controller.Move(movement * Time.deltaTime);
            }
        }
    }

    public void PushBack(Vector3 hitDirection, float finalDamage)
    {
        if (!controller || isPushed) return;

        // Normalize hit direction và thêm component dọc để tạo effect bị hất tung lên
        hitDirection.y = 0.5f; // Có thể điều chỉnh giá trị này để thay đổi độ cao bị hất
        hitDirection.Normalize();

        // Tính toán lực đẩy dựa trên damage
        float totalForce = pushForce + (finalDamage * damageForceMultiplier);

        // Set initial push velocity
        pushVelocity = hitDirection * totalForce;
        verticalVelocity = 0f; // Reset vertical velocity khi bắt đầu push

        // Start push back state
        isPushed = true;
        pushTimer = 0f;
    }
}
