using FIMSpace.FProceduralAnimation;
using UnityEngine;

namespace FIMSpace.RagdollAnimatorDemo
{
    [DefaultExecutionOrder(-2)] // Before ragdoll animator
    public class Demo_Ragd_PushAndGetUp : MonoBehaviour
    {
        public RagdollAnimator ragdoll;
        public float PowerMul = 5f;
        [Range(0f, 0.65f)] public float ImpactDuration = 0.4f;

        [Range(0f, 1f)] public float FadeMusclesTo = 0.01f;
        [Range(0f, 1.25f)] public float FadeMusclesDuration = 0.75f;

        public LayerMask snapToGroundLayer = 1 << 0;

        [Tooltip("After ragdolled and stabilization, wait about second and then triggering get up")]
        public bool AutoGetUp = true;
        public bool GetUpVersion2 = true;

        [FPD_Header("Debugging")]
        public string TestPlayAnimOnRagdoll = "";
        public RagdollProcessor.EGetUpType CanGetUp = RagdollProcessor.EGetUpType.None;
        public Vector3 LimbsVelocity;
        private Vector3 LimbsAngularVelocity;
        public float LimbsVelocityMagn;
        private float LimbsAngularVelocityMagn;

        float toGetUpElapsed = 0f;

        void Update()
        {
            if (ragdoll.Parameters == null)
            {
                return;
            }


            if (AutoGetUp)
            {
                if (ragdoll.Parameters.FreeFallRagdoll)
                    if (CanGetUp != RagdollProcessor.EGetUpType.None)
                    {
                        if (LimbsAngularVelocityMagn < 1f)
                            if (LimbsVelocityMagn < 0.1f)
                            {
                                toGetUpElapsed += Time.deltaTime;
                                if (toGetUpElapsed > 0.5f)
                                {
                                    toGetUpElapsed = 0f;
                                    TriggerGetUp();
                                }
                            }
                    }
            }


            if (Input.GetMouseButtonDown(0))
            {
                Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(r, out hit, Mathf.Infinity))
                {
                    if (hit.rigidbody)
                    {
                        if (ragdoll.Parameters.RagdollLimbs.Contains(hit.rigidbody))
                        {
                            ragdoll.StopAllCoroutines();
                            ragdoll.Parameters.SafetyResetAfterCouroutinesStop();
                            ragdoll.User_SetAllKinematic(false);
                            ragdoll.User_EnableFreeRagdoll();
                            ragdoll.User_SwitchAnimator(null, false, 0.15f);

                            ragdoll.User_SetLimbImpact(hit.rigidbody, r.direction.normalized * PowerMul, ImpactDuration);

                            if (FadeMusclesTo < 1f)
                                ragdoll.User_FadeMuscles(FadeMusclesTo, FadeMusclesDuration);

                            if (TestPlayAnimOnRagdoll != "") ragdoll.ObjectWithAnimator.GetComponent<Animator>().CrossFadeInFixedTime(TestPlayAnimOnRagdoll, 0.15f);
                        }

                    }
                }

            }

            CanGetUp = ragdoll.Parameters.User_CanGetUp(null, false);
            LimbsVelocity = ragdoll.Parameters.User_GetSpineLimbsVelocity();
            LimbsAngularVelocity = ragdoll.Parameters.User_GetSpineLimbsAngularVelocity();
            LimbsVelocityMagn = LimbsVelocity.magnitude;
            LimbsAngularVelocityMagn = LimbsAngularVelocity.magnitude;

        }

        public void TriggerGetUp()
        {
            if (GetUpVersion2)
            {
                ragdoll.transform.rotation = ragdoll.Parameters.User_GetMappedRotationHipsToHead(Vector3.up);
                ragdoll.User_SwitchAnimator(null, true);
                ragdoll.User_GetUpStackV2(0f, 0.8f, 0.7f);
                ragdoll.User_ForceRagdollToAnimatorFor(0.5f, 0.5f); // (if using blend on collision) Force non-ragdoll for 0.5 sec and restore transition in 0.5 sec
                TryPlayGetupAnimation();
            }
            else
            {
                ragdoll.StopAllCoroutines();
                ragdoll.Parameters.SafetyResetAfterCouroutinesStop();
                ragdoll.User_SwitchAnimator(null, true);
                ragdoll.User_ForceRagdollToAnimatorFor(0.75f, 0.2f);
                ragdoll.Parameters.FreeFallRagdoll = false;
                ragdoll.User_FadeMuscles(0.85f, 1f, 0.05f);
                ragdoll.User_FadeRagdolledBlend(0f, 1.25f);
                ragdoll.User_RepositionRoot(null, null, CanGetUp, snapToGroundLayer);

                TryPlayGetupAnimation();
            }
        }


        void TryPlayGetupAnimation()
        {
            Animator anim = GetComponentInChildren<Animator>();
            if (anim)
            {
                string animationClip = "GetUpFace";

                if (CanGetUp == RagdollProcessor.EGetUpType.FromBack)
                    animationClip = "GetUpBack";

                anim.Play(animationClip, 0, 0f);

                UnityEngine.Debug.Log("[Ragdoll Animator] There you can trigger playing get-up animation! Package is not including any get up animation.");
            }
        }


    }



#if UNITY_EDITOR
    [UnityEditor.CanEditMultipleObjects]
    [UnityEditor.CustomEditor(typeof(Demo_Ragd_PushAndGetUp))]
    public class Demo_Ragd_PushAndGetUpEditor : UnityEditor.Editor
    {
        public Demo_Ragd_PushAndGetUp Get { get { if (_get == null) _get = (Demo_Ragd_PushAndGetUp)target; return _get; } }
        private Demo_Ragd_PushAndGetUp _get;

        public override void OnInspectorGUI()
        {
            GUILayout.Space(4f);
            DrawDefaultInspector();

            if (Application.isPlaying)
            {
                if (Get.CanGetUp != RagdollProcessor.EGetUpType.None)
                {
                    if (Get.LimbsVelocityMagn < 0.1f)
                        if (Get.ragdoll.Parameters.FreeFallRagdoll)
                            if (GUILayout.Button("Try Get Up"))
                            {
                                Get.TriggerGetUp();
                            }
                }
            }
        }

    }
#endif
}