using System;
using UnityEngine;
using UnityEngine.AI;

public class CCBePushedBack : MonoBehaviour
{
    [Header("Push Back Settings")]
    [SerializeField] float pushForce = 10f;
    [SerializeField] float damageForceMultiplier = 0.5f;
    [SerializeField] float recoveryTime = 0.5f;
    [SerializeField] float gravity = -9.81f; // Thêm gravity
    private CharacterController controller;
    private NavMeshAgent agent;
    private bool isPushed = false;
    private float pushTimer = 0f;
    private Vector3 pushVelocity;
    private float verticalVelocity;
    private bool isGrounded;
    private Rigidbody rb;

    private bool isDead = false;

    public bool IsDead
    {
        get => isDead;
        set
        {
            isDead = value;
            if (isDead)
            {
                if (controller != null) controller.enabled = false;
                if (agent != null) agent.enabled = false;
                if (rb != null) rb.isKinematic = true;
            }
        }
    }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isDead) return;

        if (controller != null) CCPushBackUpdate();
        else if (agent != null) NavMeshAgentUpdate();
        else if (rb != null) RBPushBackUpdate();
    }

    private void NavMeshAgentUpdate()
    {
        if (isPushed)
        {
            pushTimer += Time.deltaTime;
            if (pushTimer >= recoveryTime)
            {
                isPushed = false;
                pushTimer = 0f;
                agent.velocity = Vector3.zero;
            }
            else
            {
                // Tính toán movement với decay effect
                float pushProgress = 1 - (pushTimer / recoveryTime);
                Vector3 movement = pushVelocity * pushProgress;

                // Apply movement
                if (agent.enabled == true) agent.velocity = movement;
            }
        }
    }


    private void CCPushBackUpdate()
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

    public void CCPushBack(Vector3 hitDirection, float finalDamage)
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

    private void RBPushBackUpdate()
    {
        if (isPushed)
        {
            pushTimer += Time.deltaTime;
            if (pushTimer >= recoveryTime)
            {
                isPushed = false;
                pushTimer = 0f;
                rb.linearVelocity = Vector3.zero;
            }
        }
    }

    public void RBPushBack(Vector3 hitDirection, float finalDamage)
    {  
        if (!rb || isPushed) return;

        // Normalize the hit direction and remove vertical component
        hitDirection.y = 0;
        hitDirection.Normalize();

        // Calculate push force based on damage
        float totalForce = pushForce + (finalDamage * damageForceMultiplier);

        // Apply the force
        rb.linearVelocity = Vector3.zero; // Reset current velocity
        rb.AddForce(hitDirection * totalForce, ForceMode.Impulse);

        // Start push back state
        isPushed = true;
        pushTimer = 0f;
    }

    public void PushBack(Vector3 hitDirection, float finalDamage)
    {
        // Debug.Log("Pushed back1 ");
        if (controller != null) CCPushBack(hitDirection, finalDamage);
        else if (rb != null) RBPushBack(hitDirection, finalDamage);
        else if (agent != null) NavMeshAgentPushBack(hitDirection, finalDamage);
    }

    private void NavMeshAgentPushBack(Vector3 hitDirection, float finalDamage)
    {
        if (!agent || isPushed) return;

        // Normalize the hit direction and remove vertical component
        hitDirection.y = 0;
        hitDirection.Normalize();

        // Calculate push force based on damage
        float totalForce = pushForce + (finalDamage * damageForceMultiplier);

        // Apply the force
        agent.velocity = Vector3.zero; // Reset current velocity
        agent.velocity = hitDirection * totalForce;

        // Start push back state
        isPushed = true;
        pushTimer = 0f;
    }
}
