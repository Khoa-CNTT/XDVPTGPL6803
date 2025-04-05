using FIMSpace.FProceduralAnimation;
using System.Collections;
using UnityEngine;

namespace FIMSpace.RagdollAnimatorDemo
{
    public class Demo_Ragd_RagdollDrag : MonoBehaviour
    {
        public RagdollAnimator toDrag;
        public float DragPower = 1.5f;

        [Space(4)]
        public bool ChangeMusclesPower = true;

        [Header("Switch Repose Mode in RagdollAnim")]
        [Tooltip("Most universal is Bottom Center mode")]
        public bool AutoGetUp = true;
        public LayerMask GroundMaskForGetup;
        public bool Animate = false;

        IEnumerator Start()
        {
            if (toDrag == null) yield break;

            // Wait for ragdoll initialization
            while (toDrag.Parameters.Initialized == false) yield return null;

            // Add dragger for every bone
            RagdollProcessor.PosingBone ragdollBone = toDrag.Parameters.GetPelvisBone();
            initialMuscles = toDrag.Parameters.RotateToPoseForce;

            while (ragdollBone != null)
            {
                ragdollBone.DummyBone.gameObject.AddComponent<Dragger>().Initialize(this, ragdollBone);
                ragdollBone = ragdollBone.child;
            }
        }

        float toGetUpElapsed = 0f;
        float initialMuscles = 0.75f;
        bool dragging = false;
        private void Update()
        {

            if (toDrag.Parameters.FreeFallRagdoll == false)
            {
                toGetUpElapsed = 0f;

                if (ChangeMusclesPower)
                    toDrag.Parameters.RotateToPoseForce = Mathf.MoveTowards(toDrag.Parameters.RotateToPoseForce, initialMuscles, Time.deltaTime);

                return;
            }

            if (ChangeMusclesPower)
                toDrag.Parameters.RotateToPoseForce = Mathf.MoveTowards(toDrag.Parameters.RotateToPoseForce, 0.08f, Time.deltaTime);

            if (Animate) ComputeAnimate();

            if (!AutoGetUp) return;
            if (dragging) return; // Dont getup when dragging
            ComputeGetUp();

        }

        void ComputeGetUp()
        {
            var canGetup = toDrag.Parameters.User_CanGetUp(null, false);
            if (canGetup == RagdollProcessor.EGetUpType.None) return;

            bool probeBelow = toDrag.Parameters.ProbeGroundBelowHips(GroundMaskForGetup, 0.6f).transform;


            if (probeBelow &&
                 toDrag.Parameters.User_GetSpineLimbsAngularVelocity().magnitude < 1f &&
                     toDrag.Parameters.User_GetSpineLimbsVelocity().magnitude < 0.1f)
            {
                toGetUpElapsed += Time.deltaTime;

                if (toGetUpElapsed > 0.3f)
                {
                    toGetUpElapsed = 0f;

                    // Getup
                    toDrag.transform.rotation = toDrag.Parameters.User_GetMappedRotationHipsToHead(Vector3.up);
                    toDrag.User_SwitchAnimator(null, true);
                    toDrag.User_GetUpStackV2(0f, 0.8f, 0.7f);
                    toDrag.User_ForceRagdollToAnimatorFor(0.5f, 0.5f); // (if using blend on collision) Force non-ragdoll for 0.5 sec and restore transition in 0.5 sec

                    if (Animate)
                    {
                        Animator anim = toDrag.Parameters.mecanim;
                        if (anim)
                        {
                            string animationClip = "GetUpFace";
                            if (canGetup == RagdollProcessor.EGetUpType.FromBack) animationClip = "GetUpBack";
                            anim.Play(animationClip, 0, 0f);
                        }
                    }
                }
            }
        }


        void ComputeAnimate()
        {
            Animator anim = toDrag.Parameters.mecanim;
            if (anim)
            {
                if (dragging)
                {
                    if (anim.GetBool("Dragging") == false)
                    {
                        anim.SetBool("Dragging", true);
                        anim.CrossFadeInFixedTime("Falling", 0.25f);
                    }
                }
                else
                {
                    anim.SetBool("Dragging", false);
                }
            }
        }


        class Dragger : MonoBehaviour
        {
            internal void Initialize(Demo_Ragd_RagdollDrag ragdollCatchDrag, RagdollProcessor.PosingBone ragdollBone)
            {
                Parent = ragdollCatchDrag;
                toDrag = ragdollCatchDrag.toDrag;
                bone = ragdollBone;
            }

            public Demo_Ragd_RagdollDrag Parent;
            public RagdollAnimator toDrag;
            RagdollProcessor.PosingBone bone;

            bool wasDragging = false;
            Vector3 dragStartPos;
            Vector3 limbLocalHitPos;
            Vector3 dragStartScreenWPos;

            void FixedUpdate()
            {
                if (wasDragging)
                {
                    Vector3 newPos = dragStartPos;

                    Vector3 mousePos = Input.mousePosition;
                    mousePos.z = Camera.main.transform.InverseTransformPoint(dragStartPos).z;

                    newPos += Camera.main.ScreenToWorldPoint(mousePos) - dragStartScreenWPos;

                    DragRigidbodyTo(newPos, bone.rigidbody, Parent.DragPower, limbLocalHitPos);

                    if (Input.GetMouseButtonUp(0))
                    {
                        EndDragging();
                    }
                }
            }

            private void OnMouseDrag()
            {
                Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(r, out hit, Mathf.Infinity))
                {
                    if (hit.rigidbody == bone.rigidbody)
                    {
                        if (!wasDragging) StartDrag(hit);
                    }
                }
            }

            void StartDrag(RaycastHit hit)
            {
                wasDragging = true;
                dragStartPos = hit.point;

                Vector3 mousePos = Input.mousePosition;
                mousePos.z = Camera.main.transform.InverseTransformPoint(dragStartPos).z;
                dragStartScreenWPos = Camera.main.ScreenToWorldPoint(mousePos);

                limbLocalHitPos = hit.rigidbody.transform.InverseTransformPoint(hit.point);

                toDrag.User_SwitchFreeFallRagdoll(true);
                toDrag.User_FadeRagdolledBlend(1f, 0.125f);
                Parent.dragging = true;
            }

            void EndDragging()
            {
                wasDragging = false;
                Parent.dragging = false;
            }


            void DragRigidbodyTo(Vector3 wPos, Rigidbody rig, float power, Vector3? rigLocalPos = null)
            {
                float targetPower = power;
                float powerBoost = 0f;
                if (targetPower > 1f) { targetPower = 1f; powerBoost = power - 1f; }

                float deltaDiv = Time.fixedDeltaTime * (50f - (powerBoost * 49f));
                deltaDiv = Mathf.Clamp(deltaDiv, 0.005f, 1f);

                Vector3 dragToPos = rig.position;
                if (rigLocalPos != null) dragToPos = rig.transform.TransformPoint(rigLocalPos.Value);

                Vector3 targetStableVelo = (wPos - dragToPos) / deltaDiv;
                rig.velocity = Vector3.Lerp(rig.velocity, targetStableVelo, targetPower);
            }

        }

    }
}