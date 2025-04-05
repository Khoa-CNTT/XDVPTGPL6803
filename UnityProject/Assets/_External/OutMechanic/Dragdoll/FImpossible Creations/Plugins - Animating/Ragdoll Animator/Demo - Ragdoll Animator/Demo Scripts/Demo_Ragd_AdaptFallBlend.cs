using FIMSpace.FProceduralAnimation;
using UnityEngine;


namespace FIMSpace.RagdollAnimatorDemo
{
    public class Demo_Ragd_AdaptFallBlend : MonoBehaviour
    {
        public Animator TargetAnimator;
        public RagdollAnimator ragd;
        public LayerMask GroundMask;
        public string AdditiveLayerName = "Additive Body";
        public bool IsHumanoid = true;

        float ragdTime = 0f;
        Vector3 safeUpOffset = Vector3.zero;
        Vector3 lastAppliedImpact = Vector3.zero;
        internal float veloMagn = 0f;


        #region Hashes


        private int _hash_ExtraX = -1;
        private int _hash_ExtraZ = -1;
        private int _hash_ExtraW = -1;

        private int _hash_Ragdolled = -1;
        private int _additiveLayer = 0;

        protected virtual void PrepareHashes()
        {
            _hash_ExtraX = Animator.StringToHash("ExtraX");
            _hash_ExtraZ = Animator.StringToHash("ExtraZ");
            _hash_ExtraW = Animator.StringToHash("ExtraW");
            _hash_Ragdolled = Animator.StringToHash("Ragdolled");
        }

        #endregion

        #region Animator Properties

        public float ExtraX { get { return TargetAnimator.GetFloat(_hash_ExtraX); } protected set { TargetAnimator.SetFloat(_hash_ExtraX, value); } }
        public float ExtraZ { get { return TargetAnimator.GetFloat(_hash_ExtraZ); } protected set { TargetAnimator.SetFloat(_hash_ExtraZ, value); } }
        public float ExtraW { get { return TargetAnimator.GetFloat(_hash_ExtraW); } protected set { TargetAnimator.SetFloat(_hash_ExtraW, value); } }
        public bool AnimatorRagdolled { get { return ragd.Parameters.FreeFallRagdoll && ragd.Parameters.RagdolledBlend > 0.9f; }  }

        #endregion

        void Start()
        {
            PrepareHashes();

            for (int i = 0; i < TargetAnimator.layerCount; i++)
                if (TargetAnimator.GetLayerName(i) == AdditiveLayerName)
                    _additiveLayer = i;
        }


        void Update()
        {

            //backLay = ragd.Parameters.User_CanGetUp(Vector3.up, true, false, 0.35f, true);
            //sideLay = ragd.Parameters.User_LayingOnSide(Vector3.up);
            //return;

            if (AnimatorRagdolled == false)
            {
                SetAdditive = Mathf.Max(GetAdditive - Time.deltaTime, 0f);
                return;
            }

            safeUpOffset = Vector3.up * ragd.Parameters.User_ReferenceLength() * 0.5f;

            RaycastHit hit;
            Physics.Raycast(transform.position + safeUpOffset, Vector3.down, out hit, 100f, GroundMask, QueryTriggerInteraction.Ignore);

            Vector3 pelvisVelo = ragd.Parameters.GetPelvisBone().rigidbody.velocity;
            Quaternion refRotation = ragd.Parameters.User_GetMappedRotation(Vector3.up);

            Matrix4x4 refMx = Matrix4x4.TRS(transform.position, refRotation, Vector3.one);
            Matrix4x4 refMxInv = refMx.inverse;

            veloMagn = pelvisVelo.magnitude;

            if (ragdTime < 0.3f && lastAppliedImpact != Vector3.zero)
            {
                localVelo = refMxInv.MultiplyVector(lastAppliedImpact);
                veloMagn = lastAppliedImpact.magnitude;
            }
            else
            {
                localVelo = refMxInv.MultiplyVector(pelvisVelo);
                lastAppliedImpact = Vector3.zero;
            }

            backLay = ragd.Parameters.User_CanGetUp(Vector3.up, true, false, 0.35f, !IsHumanoid);
            sideLay = ragd.Parameters.User_LayingOnSide(Vector3.up);

            smoothDampSpd = 0.2f;

            bool nearGround = false;
            if (hit.transform) if (hit.distance < 3f) nearGround = true;

            float groundAngle = Vector3.Angle(hit.normal, Vector3.up);
            ragdTime += Time.deltaTime;

            bool additiveOverride = false;

            if (veloMagn < 2.1f && nearGround)
            {
                smoothDampSpd = 0.225f;


                if (groundAngle > 26f)
                {
                    DoExtraRaycasts(ref groundAngle);
                }


                if (groundAngle < 26f && veloMagn < 1.4f) // Stable slope
                {
                    if (backLay == RagdollProcessor.EGetUpType.FromBack)
                    {
                        SetX = 0f;
                        SetZ = -1f;
                    }
                    else if (backLay == RagdollProcessor.EGetUpType.FromFacedown)
                    {
                        SetX = 0f;
                        SetZ = 1f;
                    }
                    else
                    {
                        if (sideLay == RagdollProcessor.EGetUpType.FromLeftSide)
                        {
                            smoothDampSpd = 0.225f;
                            SetX = -.5f;
                            SetZ = 0f;
                            if (veloMagn < 0.3f) { SetX = -1.25f; SetAdditive = 0.8f; additiveOverride = true; }
                        }
                        else if (sideLay == RagdollProcessor.EGetUpType.FromRightSide)
                        {
                            smoothDampSpd = 0.225f;
                            SetX = .5f;
                            SetZ = 0f;
                            if (veloMagn < 0.3f) { SetX = 1.25f; SetAdditive = 0.8f; additiveOverride = true; }
                        }
                        else
                        {
                            smoothDampSpd = 0.265f;
                            SetX = 0f;
                            SetZ = 0f;
                        }
                    }
                }
                else // Rolling from large angle slope
                {
                    float tgtX = 0f;

                    if (backLay == RagdollProcessor.EGetUpType.FromBack)
                    {
                        SetZ = -1f;
                    }
                    else if (backLay == RagdollProcessor.EGetUpType.FromFacedown)
                    {
                        SetZ = 1f;
                    }

                    if (sideLay == RagdollProcessor.EGetUpType.FromLeftSide || sideLay == RagdollProcessor.EGetUpType.FromRightSide)
                    {
                        SetAdditive = 0.6f;
                        additiveOverride = true;

                        if (veloMagn < 0.6f)
                        {
                            if (sideLay == RagdollProcessor.EGetUpType.FromLeftSide) tgtX = 1f;
                            else if (sideLay == RagdollProcessor.EGetUpType.FromRightSide) tgtX = -1f;
                        }
                    }
                    else
                    {
                        SetAdditive = 0.25f; additiveOverride = true;
                    }

                    SetX = tgtX;
                }

            }
            else // Far from ground or high velo
            {
                Vector3 locAbs = new Vector3(Mathf.Abs(localVelo.x), Mathf.Abs(localVelo.y), Mathf.Abs(localVelo.z));
                smoothDampSpd = 0.3f;

                if (locAbs.y * 0.5f > locAbs.x && locAbs.y * 0.5f > locAbs.z) // Y Dominant
                {
                    smoothDampSpd = 0.4f;
                    if (localVelo.y > 0f)
                    {
                        SetZ = 1f;
                        SetX = 0f;
                    }
                    else
                    {
                        SetZ = -1f;
                        SetX = 0f;
                    }
                }
                else // X or Z Dominant
                {
                    if (ragdTime < 0.4f) if (veloMagn > 2.5f) smoothDampSpd = 0.01f;

                    float maxSpd = 2f; // Blend Power
                    if (veloMagn > 2.25f) maxSpd = MaxVeloVal(veloMagn); else maxSpd = 1.2f;

                    if (locAbs.x > locAbs.z) // X Dominant
                    {
                        if (localVelo.x < 0f) SetX = -maxSpd; else SetX = maxSpd;
                        SetZ = Mathf.Clamp(localVelo.z * 0.5f, -1f, 1f);
                    }
                    else // Z dominant
                    {
                        if (localVelo.z < 0f) SetZ = -maxSpd; else SetZ = maxSpd;
                        SetX = Mathf.Clamp(localVelo.x * 0.5f, -1f, 1f);
                    }

                }

            }



            // Additive layer blending
            if (additiveOverride == false)
                if (_additiveLayer > 0)
                {
                    if (veloMagn > 2f)
                    {
                        SetAdditive = Mathf.InverseLerp(0.5f, 1f, veloMagn * 0.25f);
                    }
                    else
                    {
                        if (hit.distance > 2.5f) // When above ground then don't slow down with additive
                            SetAdditive = 0.25f + Mathf.InverseLerp(0.0f, 1.5f, veloMagn * 0.2f) * 0.25f;
                        else
                            SetAdditive = Mathf.InverseLerp(0.0f, 0.5f, veloMagn * 0.15f) * 0.25f;
                    }
                }
        }



        void DoExtraRaycasts(ref float groundAngle)
        {
            // Additional raycasts for average angle
            RaycastHit extraHit;
            Physics.Raycast(ragd.Parameters.GetHeadBone().transform.position + safeUpOffset, Vector3.down, out extraHit, 2f, GroundMask, QueryTriggerInteraction.Ignore);
            if (extraHit.transform) groundAngle = Mathf.LerpUnclamped(groundAngle, Vector3.Angle(extraHit.normal, Vector3.up), 0.35f);

            Physics.Raycast(ragd.Parameters.User_GetMiddleFootPos() + safeUpOffset, Vector3.down, out extraHit, 2f, GroundMask, QueryTriggerInteraction.Ignore);
            if (extraHit.transform) groundAngle = Mathf.LerpUnclamped(groundAngle, Vector3.Angle(extraHit.normal, Vector3.up), 0.35f);
        }

        float MaxVeloVal(float magnitude)
        {
            return Mathf.Lerp(1.3f, 2f, Mathf.InverseLerp(2.2f, 7f, magnitude));
        }


        float smoothDampSpd = 0.1f;
        float SetX { set { ExtraX = Mathf.SmoothDamp(ExtraX, value, ref sd_extraX, smoothDampSpd); } }
        float SetZ { set { ExtraZ = Mathf.SmoothDamp(ExtraZ, value, ref sd_extraZ, smoothDampSpd); } }
        float GetAdditive { get { return TargetAnimator.GetLayerWeight(_additiveLayer); } }
        float SetAdditive { set { SetAdditiveW = Mathf.SmoothDamp(GetAdditive, value, ref sd_layer, 0.2f); } }
        float SetAdditiveW { set { TargetAnimator.SetLayerWeight(_additiveLayer, value); } }
        float sd_extraX = 0f;
        float sd_extraZ = 0f;
        float sd_layer = 0f;

        internal Vector3 localVelo;
        internal RagdollProcessor.EGetUpType backLay;
        internal RagdollProcessor.EGetUpType sideLay;


    }
}