using UnityEngine;
using FIMSpace.FProceduralAnimation;

namespace FIMSpace.RagdollAnimatorDemo
{
    // RagdollDamageExample component with RagdollProcessor.IRagdollAnimatorReceiver implementation
    // Assign game object with this component in "Send Collision Events To" field of RagdollAnimator inspector window.
    // DISABLE "Send only on free fall" - you want to detect hits when character is not ragdolled yet
    public class Demo_Ragd_DamageExample : MonoBehaviour, RagdollProcessor.IRagdollAnimatorReceiver
    {
        public float ImpactPower = 1f;
        public float FallAtPower = 15f;
        public float DismemberAtPower = 10000f;

        public void RagdAnim_OnCollisionEnterEvent(RagdollCollisionHelper c)
        {
            if (c.ParentRagdollProcessor.FreeFallRagdoll) return; // Already ragdolled - don't proceed push impacts (you can do HeathPoints-- in else)

            var lastCollision = c.LatestEnterCollision;
            if (c.LatestEnterCollision.rigidbody == null) return; // Colliding with non rigidbody object (like floor/walls)

            Vector3 hitVelocity = lastCollision.relativeVelocity;
            float hitFactor = hitVelocity.magnitude * lastCollision.rigidbody.mass;

            if (hitFactor > DismemberAtPower)
            {
                if (!c.RagdollBone.BrokenJoint)
                    if (c.RagdollBone.ConfigurableJoint)
                    {
                        c.RagdollBone.ConfigurableJoint.breakForce = 0f;
                        //c.RagdollBone.user_internalMuscleMultiplier = 0f;
                        //c.RagdollBone.user_internalMuscleMultiplier = 0f;
                        c.RagdollBone.rigidbody.AddForce(new Vector3(0.001f, 0f, 0f), ForceMode.Acceleration);
                        c.RagdollBone.BrokenJoint = true;
                    }
            }

            if (hitFactor > 10f)
            {
                UnityEngine.Debug.Log("HIT POWER = " + hitFactor);
                UnityEngine.Debug.Log("HITTED = " + c.LimbID);

                if (hitFactor > FallAtPower)
                {
                    // Play some fall animation for ragdoll fall pose (not needed)
                    if (c.ParentRagdollProcessor.mecanim)
                    {
                        c.ParentRagdollProcessor.mecanim.CrossFadeInFixedTime("Fall", 0.1f);
                    }

                    if (c.LimbID == HumanBodyBones.Hips)
                    {
                        c.RagdollBone.rigidbody.isKinematic = false;
                        c.ParentRagdollAnimator.User_SetLimbImpact(c.RagdollBone.rigidbody, hitVelocity.normalized * ImpactPower, 0.25f, ForceMode.VelocityChange);
                    }

                    // Free Fall
                    c.ParentRagdollAnimator.User_SwitchFreeFallRagdoll(true);
                    // Push whole ragdoll with some force
                    c.ParentRagdollAnimator.User_SetPhysicalImpactAll(hitVelocity.normalized * ImpactPower, 0.175f, ForceMode.Acceleration);
                    // Empathise hitted limb with impact
                    c.ParentRagdollAnimator.User_SetLimbImpact(c.RagdollBone.rigidbody, hitVelocity.normalized * ImpactPower, 0.25f, ForceMode.VelocityChange);
                    // Make character muscles weak
                    c.ParentRagdollAnimator.User_FadeMuscles(0.01f, 0.8f, 0.1f);
                }
            }
        }

        public void RagdAnim_OnCollisionExitEvent(RagdollCollisionHelper c)
        { /* Not needed handling exit event */ }

    }
}