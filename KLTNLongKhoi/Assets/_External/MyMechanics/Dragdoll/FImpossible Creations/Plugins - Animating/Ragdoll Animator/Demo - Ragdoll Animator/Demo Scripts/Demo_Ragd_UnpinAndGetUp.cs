using FIMSpace.FProceduralAnimation;
using UnityEngine;

namespace FIMSpace.RagdollAnimatorDemo
{
    [DefaultExecutionOrder(-2)] // Before ragdoll animator
    public class Demo_Ragd_UnpinAndGetUp : MonoBehaviour
    {
        public RagdollAnimator Ragdoll;
        public float UnpinAtDistance = 1f;

        [Header("< Just Debugging >")]
        public float DebugHipsDistance = 0f;

        float getUpCulldown = 0f;

        void Start()
        {
            if (Ragdoll.Parameters.HipsPin == false)
            {
                UnityEngine.Debug.Log("[Ragdoll Animator] Hips Pin is not enabled!");
                return;
            }

        }

        void LateUpdate()
        {
            if (Ragdoll.Parameters.HipsPin == false) return;

            if (Ragdoll.Parameters.FreeFallRagdoll == false)
            {
                if (getUpCulldown < .5f)
                {
                    getUpCulldown += Time.deltaTime;
                    return;
                }

                Vector3 animPos = Ragdoll.Parameters.Pelvis.position;
                Vector3 ragPos = Ragdoll.Parameters.RagdollSetup[0].transform.position;

                DebugHipsDistance = Vector3.Distance(animPos, ragPos);
                //UnityEngine.Debug.DrawLine(animPos, ragPos, Color.green, 1.01f);

                if (DebugHipsDistance > 1f)
                {
                    Ragdoll.User_EnableFreeRagdoll(1f);
                    Ragdoll.User_FadeMuscles(0.05f, 1f, 0.1f);
                    Ragdoll.User_SwitchAnimator(null, false, 0.65f);
                    getUpCulldown = 0f;
                }
            }
            else
            {
                if (Ragdoll.Parameters.User_GetAllLimbsVelocity().magnitude < 0.2f)
                {
                    var getUpType = Ragdoll.Parameters.User_CanGetUp();

                    if (getUpType != RagdollProcessor.EGetUpType.None)
                    {
                        Ragdoll.User_GetUpStack(getUpType, ~(0 << 0), 1f, 0.75f);

                        Animator anim = GetComponentInChildren<Animator>();
                        if (anim)
                        {
                            string animationClip = "GetUpFace";
                            if (getUpType == RagdollProcessor.EGetUpType.FromBack) animationClip = "GetUpBack";
                            anim.Play(animationClip, 0, 0f);
                            UnityEngine.Debug.Log("[Ragdoll Animator] There you can trigger playing get-up animation! Package is not including any get up animation.");
                        }
                    }

                }


            }
        }

    }
}
