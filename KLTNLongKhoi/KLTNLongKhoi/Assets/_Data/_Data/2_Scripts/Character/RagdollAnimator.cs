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

        foreach (var collider in GetComponentsInChildren<Collider>())
        {
            if (collider.GetComponent<CharacterJoint>()) collider.enabled = false;
        }

        _animator.enabled = true;
        _characterController.enabled = true;
    }

    public void EnableRagdoll()
    {
        foreach (var rigidbody in _ragdollRigidbodies)
        {
            rigidbody.isKinematic = false;
        }
        
        foreach (var collider in GetComponentsInChildren<Collider>())
        {
            if (collider.GetComponent<Rigidbody>()) collider.enabled = true;
        }

        _animator.enabled = false;
        _characterController.enabled = false;
    }
}
