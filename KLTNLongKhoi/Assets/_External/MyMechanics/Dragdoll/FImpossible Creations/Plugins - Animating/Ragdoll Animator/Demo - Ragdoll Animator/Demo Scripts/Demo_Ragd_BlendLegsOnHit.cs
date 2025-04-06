using UnityEngine;
using FIMSpace.FProceduralAnimation;

namespace FIMSpace.Basics
{
    public class Demo_Ragd_BlendLegsOnHit : MonoBehaviour, RagdollProcessor.IRagdollAnimatorReceiver
    {
        public string TriggerBlendOnTagged = "Bullet";
        public float RestoreCulldown = 0.4f;

        RagdollAnimator ragdoll;
        float blendInTimer = 0f;
        void Update()
        {
            if (ragdoll == null) return;

            blendInTimer -= Time.deltaTime;

            if (blendInTimer > 0f)
            {
                float blend = ragdoll.Parameters.RagdolledBlend;
                ragdoll.Parameters.RagdolledBlend = Mathf.MoveTowards(blend, 1f, Time.deltaTime * 15f);
            }
            else
            {
                float blend = ragdoll.Parameters.RagdolledBlend;
                ragdoll.Parameters.RagdolledBlend = Mathf.MoveTowards(blend, 0f, Time.deltaTime * 5f);
            }
        }

        public void RagdAnim_OnCollisionEnterEvent(RagdollCollisionHelper c)
        {
            // Checking if hitted one of the humanoid Leg Bones IDs
            if ((int)c.LimbID >= (int)HumanBodyBones.LeftUpperLeg &&
                (int)c.LimbID <= (int)HumanBodyBones.RightFoot)
            {
                if (c.LatestEnterCollision.collider.CompareTag(TriggerBlendOnTagged))
                {
                    ragdoll = c.ParentRagdollAnimator;
                    blendInTimer = RestoreCulldown;
                }
            }
        }

        public void RagdAnim_OnCollisionExitEvent(RagdollCollisionHelper c)
        {
        }

    }

}