using FIMSpace.FProceduralAnimation;
using UnityEngine;

namespace FIMSpace.RagdollAnimatorDemo
{
    public class Demo_Ragd_GoToRagdollCenter : MonoBehaviour
    {
        public RagdollAnimator Ragdoll;
        public float HeightOffset = 1f;
        public bool FollowRotation = false;

        void LateUpdate()
        {
            Vector3 targetPosition = Ragdoll.Parameters.User_GetRagdollBonesBoundsCenterBottom();
            targetPosition = Ragdoll.Parameters.BaseTransform.InverseTransformPoint(targetPosition);
            targetPosition.y = HeightOffset;
            transform.position = Ragdoll.Parameters.BaseTransform.TransformPoint(targetPosition);

            if ( FollowRotation)
            {
                transform.rotation = Ragdoll.Parameters.BaseTransform.rotation * Ragdoll.Parameters.GetPelvisBone().initialLocalRotation;
            }
        }
    }
}