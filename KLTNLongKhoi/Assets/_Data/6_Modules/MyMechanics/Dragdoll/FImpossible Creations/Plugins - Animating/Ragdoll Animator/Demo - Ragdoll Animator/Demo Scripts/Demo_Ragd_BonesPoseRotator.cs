using System;
using System.Collections.Generic;
using UnityEngine;

namespace FIMSpace.FProceduralAnimation
{
    public class Demo_Ragd_BonesPoseRotator : MonoBehaviour
    {
        public RagdollAnimator Ragdoll;
        [Space(2)]
        public LayerMask RaycastMask;
        [Space(5)]
        public float RotationSensitivty = 180f;
        [Space(2)]
        [Tooltip("Limiting controlled rotation accordingly to the joints limits")]
        public bool LimitRotation = true;

        [Space(5)]
        public List<BoneController> BoneControl;

        // Helper class to handle rotating bones
        [System.Serializable]
        public class BoneController
        {
            public Transform bone;

            [Tooltip("Depending on local rotation in skeleton, adjust manually")]
            public Vector3 RotationAxis = Vector3.right;

            public enum ELimit
            {
                None, XLimits, YLimits, ZLimits
            }

            [Tooltip("It depends on the ragdoll setup which limits are correct to the bone, test it manually and find correct ones")]
            public ELimit LimitGuide = ELimit.XLimits;

            /// <summary> Only playmode variable</summary>
            [NonSerialized] public float Rotated = 0f;
            private Quaternion initialLocalRotation;
            private Vector3 limits = Vector3.zero;
            private float highXLimit = 0f;

            public void Init(RagdollAnimator ragdoll)
            {
                initialLocalRotation = bone.localRotation;

                ConfigurableJoint joint = bone.GetComponent<ConfigurableJoint>();
                if (joint == null)
                {
                    var posingBone = ragdoll.Parameters.GetRagdollDummyBoneByAnimatorBone(bone);
                    joint = posingBone.transform.GetComponent<ConfigurableJoint>();
                }

                limits = new Vector3(joint.lowAngularXLimit.limit, joint.angularYLimit.limit, joint.angularZLimit.limit);
                highXLimit = joint.highAngularXLimit.limit;
            }

            public void Update(bool limit)
            {
                if (limit)
                {
                    if (LimitGuide == ELimit.XLimits)
                    {
                        if (Rotated < limits.x) Rotated = limits.x;

                        if (highXLimit > 0f)
                        {
                            if (Rotated > highXLimit) Rotated = highXLimit;
                        }
                    }
                    else if (LimitGuide == ELimit.YLimits)
                    {
                        if (Rotated < -limits.y) Rotated = -limits.y;
                        if (Rotated > limits.y) Rotated = limits.y;
                    }
                    else if (LimitGuide == ELimit.ZLimits)
                    {
                        if (Rotated < -limits.z) Rotated = -limits.z;
                        if (Rotated > limits.z) Rotated = limits.z;
                    }
                }

                bone.localRotation = initialLocalRotation * Quaternion.AngleAxis(Rotated, RotationAxis);
            }
        }

        void Start()
        {
            for (int b = 0; b < BoneControl.Count; b++)
            {
                BoneControl[b].Init(Ragdoll);
            }
        }

        BoneController isPressed = null;

        void Update()
        {
            if (isPressed != null)
            {
                if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
                {
                    isPressed = null;
                }
            }

            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 10000f, RaycastMask, QueryTriggerInteraction.Ignore))
                {
                    for (int b = 0; b < BoneControl.Count; b++)
                    {
                        if (BoneControl[b].bone == hit.transform) { isPressed = BoneControl[b]; break; }
                    }
                }
            } // Input end

            // Control bone being pressed
            if (isPressed != null)
            {
                // Left mouse button - rotate positive
                if (Input.GetMouseButton(0))
                {
                    isPressed.Rotated += Time.deltaTime * RotationSensitivty;
                }
                else // Left mouse button - rotate negative
                if (Input.GetMouseButton(1))
                {
                    isPressed.Rotated -= Time.deltaTime * RotationSensitivty;
                }
            }

            // Update skeleton bones rotations
            for (int b = 0; b < BoneControl.Count; b++)
            {
                BoneControl[b].Update(LimitRotation);
            }

            Ragdoll.Parameters.CaptureAnimation(true); // Update pose for ragdoll
        }
    }
}