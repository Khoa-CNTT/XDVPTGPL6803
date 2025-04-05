using FIMSpace.FProceduralAnimation;
using UnityEngine;

namespace FIMSpace.RagdollAnimatorDemo
{
    public class Demo_Ragd_FaintOnClick : MonoBehaviour
    {
        [Header("If using 'Repose Mode' requires 'Full Ragdoll Only' enabed for seamless transition")]
        public RagdollAnimator ragdoll;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) == false) return;

            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(r, out hit, Mathf.Infinity) == false) return;

            if (hit.rigidbody && ragdoll.Parameters.RagdollLimbs.Contains(hit.rigidbody))
            {

                // Bring ragdolled blend to 100% in defined duration (here 0.4 sec)
                ragdoll.User_FadeRagdolledBlend(1f, 0.4f);

                // Falling Ragdoll switch delay to let 'RagdolledBlend' get a bit bigger value before fall which can help out transition a bit
                ragdoll.User_SwitchFreeFallRagdoll(true, 0.25f);

                // Fade muscles with delay to add a bit realistic feel to the fall animation
                ragdoll.User_FadeMuscles(0.006f, 0.8f, 0.2f);

                // Animator switch off delay 0.6 to keep some of the motion during initial ragdolling
                ragdoll.User_SwitchAnimator(null, false, 0.6f);
            }
        }
    }
}