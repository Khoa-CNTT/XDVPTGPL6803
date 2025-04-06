using UnityEngine;
using FIMSpace.FProceduralAnimation;

public class RagdollAnimator : MonoBehaviour
{
    [Header("Ragdoll Settings")]
    [SerializeField] private float _ragdollForce = 10f;
    [SerializeField] private float _muscleRegainSpeed = 0.4f;
    [SerializeField] private float _getUpDelay = 2f;

    private Animator _animator;
    private CharacterController _characterController;
    private bool _isRagdolled = false;
    private float _targetMuscleMultiplier = 1f;
    private float _currentMuscleMultiplier = 1f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
    }

    // Enable free ragdoll
    public void User_EnableFreeRagdoll()
    {
        if (_isRagdolled) return;

        _isRagdolled = true;

        // Disable CharacterController to not affect physics
        if (_characterController != null)
            _characterController.enabled = false;

        // Enable ragdoll physics
        if (_animator != null)
        {
            _animator.enabled = false;  // Disable animator when ragdoll is active
            _targetMuscleMultiplier = 0f;  // Relax all muscles
            _currentMuscleMultiplier = 0f;
        }

        // Apply initial force to make the ragdoll fall
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(Random.insideUnitSphere * _ragdollForce, ForceMode.Impulse);
        }

        // Schedule getting up after delay
        Invoke("DisableRagdoll", _getUpDelay);
    }

    private void EnableAnimator()
    {
        if (_animator != null)
            _animator.enabled = true;
    }

    // Apply force to all ragdoll parts
    public void User_SetPhysicalImpactAll(Vector3 impactForce, float duration)
    {

    }

    private void RegainMuscles()
    {
        _targetMuscleMultiplier = 1f;
    }

    private void Update()
    {

    }

    // Disable ragdoll and return to normal state
    public void DisableRagdoll()
    {
        if (!_isRagdolled) return;

        _isRagdolled = false;

        // Enable CharacterController
        if (_characterController != null)
            _characterController.enabled = true;

        // Disable ragdoll physics
        if (_animator != null)
        {
            _animator.enabled = true;  // Enable animator when ragdoll is disabled
            _targetMuscleMultiplier = 1f;  // Reset muscle multiplier
            _currentMuscleMultiplier = 1f;
        }

        // Reset rigidbodies to kinematic
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
        }
    }
}
