using UnityEngine;
using System.Linq;

public class RagdollAnimator : MonoBehaviour
{
    private CharacterController _characterController;
    private Animator _animator;
    private Rigidbody[] _ragdollRigidbodies;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        _characterController = GetComponentInChildren<CharacterController>();

        DisableRagdoll();
    }


    public void TriggerRagdoll(Vector3 force, Vector3 hitPoint)
    {
        EnableRagdoll();

        Rigidbody hitRigidbody = _ragdollRigidbodies.OrderBy(rigidbody => Vector3.Distance(rigidbody.position, hitPoint)).First();

        hitRigidbody.AddForceAtPosition(force, hitPoint, ForceMode.Impulse);
    }

    public void DisableRagdoll()
    {
        foreach (var rigidbody in _ragdollRigidbodies)
        {
            rigidbody.isKinematic = true;
        }

        _animator.enabled = true;
        if (_characterController != null) _characterController.enabled = true;
    }

    public void EnableRagdoll()
    {
        foreach (var rigidbody in _ragdollRigidbodies)
        {
            rigidbody.isKinematic = false;
        }

        _animator.enabled = false;
        if (_characterController != null) _characterController.enabled = false;
    }
}
