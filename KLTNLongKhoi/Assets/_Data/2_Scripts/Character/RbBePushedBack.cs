using UnityEngine;

public class RbBePushedBack : MonoBehaviour
{
    [Header("Push Back Settings")]
    [SerializeField] private float pushForce = 10f;
    [SerializeField] private float damageForceMultiplier = 0.5f;
    [SerializeField] private float recoveryTime = 0.5f;
    
    private Rigidbody rb;
    private bool isPushed = false;
    private float pushTimer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("RbBePushedBack requires a Rigidbody component!");
        }
    }

    private void Update()
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

    public void PushBack(Vector3 hitDirection, float finalDamage)
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
}
