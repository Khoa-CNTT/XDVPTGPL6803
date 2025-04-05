using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FIMSpace.RagdollAnimatorDemo
{
    public class Demo_Ragd_Move : MonoBehaviour
    {
        public Vector3 LocalVelocity;
        public Rigidbody rig;

        private void FixedUpdate()
        {
            rig.velocity = transform.TransformVector(LocalVelocity);
        }

    }
}