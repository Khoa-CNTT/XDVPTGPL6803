using FIMSpace.FProceduralAnimation;
using System.Collections;
using UnityEngine;

namespace FIMSpace.RagdollAnimatorDemo
{
    public class Demo_Ragd_DelayedImpact : MonoBehaviour
    {
        public RagdollAnimator ragdoll;
        public float Delay = 2f;
        public float Power = 2f;

        private void Start()
        {
            StartCoroutine(DelayedImpact());
        }

        IEnumerator DelayedImpact()
        {
            yield return new WaitForSeconds(Delay);

            Vector3 direction = ragdoll.Parameters.Head.position - Camera.main.transform.position;
            direction += Vector3.up * 2f;

            ragdoll.User_EnableFreeRagdoll();
            ragdoll.User_SetPhysicalImpactAll(direction * Power, 0.15f);
        }
    }
}