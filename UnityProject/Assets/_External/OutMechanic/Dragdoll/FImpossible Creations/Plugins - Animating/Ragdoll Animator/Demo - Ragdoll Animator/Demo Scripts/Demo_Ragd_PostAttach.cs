using FIMSpace.FProceduralAnimation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FIMSpace.RagdollAnimatorDemo
{
    public class Demo_Ragd_PostAttach : MonoBehaviour
    {
        public Joint joint;
        public RagdollAnimator ragdoll;
        private Transform tr;

        void Awake()
        {
            if (joint == null) joint = GetComponentInChildren<Joint>();

            if (joint.connectedBody != null)
                tr = joint.connectedBody.transform;
        }

        void Start()
        {
            StartCoroutine(DelayedStart());
        }

        IEnumerator DelayedStart()
        {
            yield return null;
            if (ragdoll != null)
            {
                Transform bone = ragdoll.Parameters.GetRagdollDummyBoneByAnimatorBone(tr);
                if (bone != null)
                {
                    Rigidbody rigidBody = bone.GetComponent<Rigidbody>();
                    joint.connectedBody = rigidBody;
                }
            }
        }
    }
}
