
using FIMSpace.Basics;
using FIMSpace.FProceduralAnimation;
using UnityEngine;


namespace FIMSpace.RagdollAnimatorDemo
{
    /// <summary>
    /// Warning, this code is like a draft version
    /// </summary>
    public class Demo_Ragd_FredAI : MonoBehaviour
    {
        [Header("It's just example script - not dedicated for real gameplay")]
        public Animator TargetAnimator;
        public RagdollAnimator Ragdoll;
        public LayerMask GroundMask;

        private Rigidbody rig;
        private FAnimationClips anim;

        [Space(5)]
        public Transform Enemy;

        bool initialized = false;

        void Start()
        {

            PrepareHashes();

            // Helper animator class
            anim = new FAnimationClips(TargetAnimator);
            anim.AddClip("Idle");
            anim.AddClip("Jump Attack");
            anim.AddClip("Roll");
            anim.AddClip("Walk");
            anim.AddClip("Fall");
            anim.AddClip("Try Get Up");
            anim.AddClip("Get Up");

            rig = GetComponent<Rigidbody>();

            initialized = true;
        }


        public enum EAIStage
        {
            FindPosition,
            PositionToAttack,
            StartAttacking,
            DuringAttack,
            OnTheGround,
            GetUp
        }

        private EAIStage aiStage = EAIStage.FindPosition;
        private Vector3 attackPosition = Vector3.zero;
        private bool canWalkAnimation = true;
        private float getUpDur = 0f;

        [Header("Debug AI Settings")]
        public Vector2 ToAttackDistance = new Vector2(4f, 5f);
        public float MovSpeed = 1f;
        public float RotSpeed = 2f;

        public Vector3 JumpImpact = new Vector3(0f, 1f, 4f);

        public Vector3 JumpHelpTorque = Vector3.zero;
        public float JumpBoost = 1f;
        public Vector3 GetUpHelperTorque = Vector3.zero;
        //public float JumpPower = 1f;
        //public float JumpPowerY = 1f;
        float nearPointToleranceLowerer = 1f;
        float stuckTimer = 0f;

        Vector3 ZeroY(Vector3 v)
        {
            v.y = 0f;
            return v;
        }

        void FixedUpdate()
        {
            if (!initialized) return;

            wasMoving = false;
            forceWalkAnim = false;
            disableRigMove = false;
            bool additiveFadeIn = false;
            MovementDir = transform.forward;

            if (aiStage == EAIStage.FindPosition)
            {
                canWalkAnimation = true;
                aiStage = EAIStage.PositionToAttack;
                attackPosition = Enemy.position;

                tryGetSign = -1f;
                Vector3 enemyToMe = (transform.position - Enemy.position);
                attackPosition += enemyToMe.normalized * Random.Range(ToAttackDistance.x, ToAttackDistance.y);
                nearPointToleranceLowerer = 1f;
                stuckTimer = 0f;
            }
            else if (aiStage == EAIStage.PositionToAttack)
            {
                canWalkAnimation = true;

                float distance = FVectorMethods.DistanceTopDown(transform.position, attackPosition);

                if (distance < 0.2f * nearPointToleranceLowerer)
                {
                    lastMaxRootVelo = Vector3.zero;
                    aiStage = EAIStage.StartAttacking;
                }
                else if (distance < 1.25f)
                {
                    nearPointToleranceLowerer += Time.fixedDeltaTime * 0.1f;
                    if (accel > 0.3f) accel -= Time.fixedDeltaTime * 2f;
                    MovementDir = Vector3.Slerp(MovementDir, (ZeroY(attackPosition) - ZeroY(transform.position)).normalized, distance * 0.7f);
                }

                GoForward();
                RotateTowards(attackPosition);
            }
            else if (aiStage == EAIStage.StartAttacking)
            {
                hittedEnemy = false;
                Vector3 meToEnemy = (Enemy.position - transform.position).normalized;

                float angle = Vector3.Angle(transform.forward, meToEnemy);

                if (Mathf.Abs(angle) < 7f)
                {
                    attackExecution = true;
                    disableRigMove = true;
                    anim.CrossFadeInFixedTime("Jump Attack", 0.25f, 0f);
                    canWalkAnimation = false;
                    attackDur = 0f;
                }

                if (attackExecution == false)
                {
                    canWalkAnimation = true;
                    RotateTowards(Enemy.position);
                    forceWalkAnim = true;
                }
            }
            else if (aiStage == EAIStage.DuringAttack)
            {
                attackExecution = false;
                disableRigMove = true;
                canWalkAnimation = false;

                if (hittedEnemy)
                {
                    hittedEnemy = false;
                    anim.CrossFadeInFixedTime("Fall");
                }

                attackDur += Time.fixedDeltaTime;

                if (attackDur > 0.6f)
                {
                    if (Ragdoll.Parameters.User_GetSpineLimbsVelocity().magnitude < 1f)
                    {
                        stuckTimer += Time.fixedDeltaTime;
                        if (stuckTimer > 4f)
                        {
                            stuckTimer = 0f;
                            aiStage = EAIStage.OnTheGround;
                        }
                    }

                    RaycastHit checkGround = Ragdoll.Parameters.ProbeGroundBelowHips(GroundMask, 0.45f);

                    if (checkGround.transform)
                    {
                        aiStage = EAIStage.OnTheGround;
                        anim.CrossFadeInFixedTime("Fall");
                    }
                }

            }
            else if (aiStage == EAIStage.OnTheGround)
            {
                canWalkAnimation = false;
                //anim.CrossFadeInFixedTime("Fall", 0.5f);
                var canGetup = Ragdoll.Parameters.User_CanGetUp(null, true, false, 0.35f, true);


                bool probeBelow = Ragdoll.Parameters.ProbeGroundBelowHips(GroundMask, 0.425f).transform;

                if (!probeBelow)
                    if (canGetup != RagdollProcessor.EGetUpType.None)
                        if (Ragdoll.Parameters.User_GetSpineLimbsVelocity().magnitude < .2f)
                        {
                            stuckTimer += Time.fixedDeltaTime;
                            if (stuckTimer > 2f)
                            {
                                stuckTimer = 0f;
                                Ragdoll.User_SetPhysicalImpactAll(Vector3.up * 0.15f, 0.125f);
                                Ragdoll.User_SetPhysicalTorque(Vector3.one, 0.2f, false);
                            }
                        }


                if (tryGetUpDur < 0.1f && canGetup == RagdollProcessor.EGetUpType.FromFacedown)
                {
                    SetX = 0f;
                    SetZ = 1f;


                    if (probeBelow)
                    {
                        if (Ragdoll.Parameters.User_GetSpineLimbsVelocity().magnitude < 0.3f)
                        {
                            anim.CrossFadeInFixedTime("Get Up", 0f);
                            rig.rotation = Ragdoll.Parameters.User_GetMappedRotationHipsToHead(Vector3.up, false);
                            //GetComponent<Collider>().enabled = true;
                            //rig.useGravity = true;

                            //Ragdoll.User_GetUpStackV2(0f, 0.7f, 1.5f);
                            float duration = 0.5f;

                            Ragdoll.StopAllCoroutines();
                            Ragdoll.Parameters.SafetyResetAfterCouroutinesStop();
                            Ragdoll.User_SwitchFreeFallRagdoll(false, duration * 0.75f);
                            Ragdoll.User_FadeMuscles(0.7f, duration * 0.75f);
                            Ragdoll.User_FadeRagdolledBlend(0f, duration * 0.55f, 0f);

                            Ragdoll.User_ChangeReposeAndRestore(RagdollProcessor.EBaseTransformRepose.None, duration);
                            Ragdoll.User_ChangeBlendOnCollisionAndRestore(false, duration);
                            Ragdoll.Parameters._User_GetUpResetProbe();

                            Ragdoll.User_ForceRagdollToAnimatorFor(.5f, .4f);

                            aiStage = EAIStage.GetUp;
                            getUpDur = duration;
                            accel = 0f;
                        }
                    }
                }
                else
                {
                    var canGetupSide = Ragdoll.Parameters.User_LayingOnSide();

                    if (canGetup == RagdollProcessor.EGetUpType.FromBack)
                    {
                        SetZ = -0.5f;
                    }
                    else if (canGetup == RagdollProcessor.EGetUpType.FromFacedown)
                    {
                        SetZ = 0.7f;
                    }
                    else
                        SetZ = 0f;

                    if (canGetupSide == RagdollProcessor.EGetUpType.FromLeftSide)
                    {
                        SetX = 0.5f;
                    }
                    else if (canGetupSide == RagdollProcessor.EGetUpType.FromRightSide)
                    {
                        SetX = -0.5f;
                    }
                    else
                    {
                        SetX = Mathf.Sin(Time.fixedTime * 1.5f);
                    }


                    if (tryGetUpDur <= 0f)
                    {

                        Vector3 velo = Ragdoll.Parameters.User_GetSpineLimbsVelocity();
                        float veloMagn = velo.magnitude;
                        additiveFadeIn = true;

                        if (Time.time - lastTryGetup > 1.5f)
                            if (veloMagn < 0.8f)
                                if (canGetup == RagdollProcessor.EGetUpType.FromBack)
                                {
                                    tryGetSign = -tryGetSign;
                                    tryGetUpDur = 2f;
                                    anim.CrossFadeInFixedTime("Try Get Up");
                                }

                        //anim.SetFloat("Blend", Mathf.Abs(Mathf.Sin(Time.fixedTime * 2f)));
                    }
                    else
                    {
                        tryGetUpDur -= Time.fixedDeltaTime;

                        if (canGetup != RagdollProcessor.EGetUpType.FromFacedown)
                        {
                            Ragdoll.User_SetPhysicalTorque(Ragdoll.Parameters.GetPelvisBone().rigidbody, GetUpHelperTorque * tryGetSign, 0f, true, ForceMode.Acceleration, true);
                            Ragdoll.User_SetPhysicalTorque(Ragdoll.Parameters.GetPelvisBone().child.rigidbody, GetUpHelperTorque * tryGetSign * 0.9f, 0f, true, ForceMode.Acceleration, true);
                            Ragdoll.User_SetPhysicalTorque(Ragdoll.Parameters.GetPelvisBone().child.child.rigidbody, GetUpHelperTorque * tryGetSign * 0.8f, 0f, true, ForceMode.Acceleration, true);
                        }

                        if (tryGetUpDur <= 0f)
                        {
                            anim.CrossFadeInFixedTime("Fall");
                            lastTryGetup = Time.time;
                        }
                    }
                }
            }
            else if (aiStage == EAIStage.GetUp)
            {
                canWalkAnimation = false;
                getUpDur -= Time.fixedDeltaTime;
                if (getUpDur < 0f) aiStage = EAIStage.FindPosition;
            }


            if (!disableRigMove)
            {
                if (wasRootMotion == false)
                {
                    Vector3 vel = MovementDir * MovSpeed * accel;
                    vel.y = rig.velocity.y;
                    rig.velocity = vel;
                }
            }

            if (!wasMoving)
            {
                accel = Mathf.Lerp(accel, 0f, Time.fixedDeltaTime * 4f);
            }

            HandleBasicAnimations();

            if (additiveFadeIn)
            {
                SetAdditive = 0.4f + Mathf.Abs(Mathf.Sin(Time.fixedTime * 1.5f) * 0.6f);
            }
            else
                SetAdditive = 0f;
        }

        float SetAdditive { set { SetAdditiveW = Mathf.SmoothDamp(TargetAnimator.GetLayerWeight(1), value, ref sd_layer, 0.1f); } }
        float SetAdditiveW { set { TargetAnimator.SetLayerWeight(1, value); } }
        float sd_layer = 0f;

        float smoothDampSpd = 0.25f;
        float SetX { set { ExtraX = Mathf.SmoothDamp(ExtraX, value, ref sd_extraX, smoothDampSpd); } }
        float SetZ { set { ExtraZ = Mathf.SmoothDamp(ExtraZ, value, ref sd_extraZ, smoothDampSpd); } }

        float sd_extraX = 0f;
        float sd_extraZ = 0f;


        Vector3 MovementDir;



        #region Hashes


        private int _hash_ExtraX = -1;
        private int _hash_ExtraZ = -1;

        protected virtual void PrepareHashes()
        {
            _hash_ExtraX = Animator.StringToHash("ExtraX");
            _hash_ExtraZ = Animator.StringToHash("ExtraZ");
        }

        #endregion

        #region Animator Properties

        public float ExtraX { get { return TargetAnimator.GetFloat(_hash_ExtraX); } protected set { TargetAnimator.SetFloat(_hash_ExtraX, value); } }
        public float ExtraZ { get { return TargetAnimator.GetFloat(_hash_ExtraZ); } protected set { TargetAnimator.SetFloat(_hash_ExtraZ, value); } }

        #endregion



        void HandleBasicAnimations()
        {
            if (canWalkAnimation)
            {
                if (forceWalkAnim)
                    anim.CrossFadeInFixedTime("Walk", 0.2f);
                else
                {
                    if (accel < 0.05f) anim.CrossFadeInFixedTime("Idle", 0.2f);
                    else anim.CrossFadeInFixedTime("Walk", 0.2f);
                }
            }
        }

        bool attackExecution = false;
        bool forceWalkAnim = false;
        bool attackAnimation = false;
        float attackDur = 0f;
        float tryGetUpDur = 0f;
        float tryGetSign = 1f;
        float lastTryGetup = -1f;

        public void EJumpAttack()
        {
            attackAnimation = false;
            aiStage = EAIStage.DuringAttack;
            Ragdoll.User_EnableFreeRagdoll(1f, 0.2f);

            Vector3 velocity = transform.TransformVector(JumpImpact);
            //Vector3 velocity = lastMaxRootVelo;
            //velocity.y *= 0.35f;
            //UnityEngine.Debug.Log("apply " + lastMaxRootVelo);
            Ragdoll.User_SetVelocityAll(velocity);

            Vector3 boost = velocity;
            boost.y *= 0.4f;
            Ragdoll.User_SetPhysicalImpactAll(boost * 0.02f * JumpBoost, 0.15f);

            Ragdoll.User_SetPhysicalTorque(JumpHelpTorque, 0.125f, true);
            //GetComponent<Collider>().enabled = false;
            //rig.useGravity = false;
            //Vector3 jumpImpact = (Enemy.position - transform.position).normalized;
            //Ragdoll.User_SetPhysicalImpactAll(jumpImpact * 0.15f * JumpPower + Vector3.up * JumpPowerY * 0.2f, 0f, ForceMode.VelocityChange);
            //Ragdoll.User_SetPhysicalImpactAll(jumpImpact * JumpPower + Vector3.up * JumpPowerY, 0.125f);
        }

        private void OnDrawGizmosSelected()
        {
            if (Enemy)
            {
                Gizmos.DrawRay(Enemy.position, (transform.position - Enemy.position).normalized * ToAttackDistance.y);
                if (attackPosition != Vector3.zero) Gizmos.DrawRay(attackPosition, Vector3.up);
            }
        }

        bool disableRigMove = false;
        float accel = 0f;
        bool wasMoving = false;
        void GoForward()
        {
            wasMoving = true;
            accel = Mathf.Lerp(accel, 1f, Time.fixedDeltaTime * 4f);
        }

        void RotateTowards(Vector3 pos)
        {
            //rig.rotation = Quaternion.Slerp(rig.rotation, Quaternion.LookRotation(transform.position, pos), Time.fixedDeltaTime * RotSpeed);

            rig.angularVelocity = Vector3.zero;
            Vector3 rotDir = Vector3.ProjectOnPlane(pos - transform.position, Vector3.up);
            rig.rotation = Quaternion.Slerp(rig.rotation, Quaternion.LookRotation(rotDir), Time.fixedDeltaTime * RotSpeed);
        }

        bool hittedEnemy = false;
        float lastBounce = -1f;
        void ERagColl(RagdollCollisionHelper coll)
        {

            if (coll.LatestEnterCollision.collider.gameObject.layer == coll.gameObject.layer)
            {
                hittedEnemy = true;


                //if (Time.time - lastBounce > 1f)
                //{
                //    lastBounce = Time.time;
                //    Vector3 bounceDir = Ragdoll.Parameters.GetRagdolledPelvis().position - Enemy.position;
                //    bounceDir.y = 0f;
                //    Ragdoll.User_SetPhysicalImpactAll(bounceDir.normalized * (JumpImpact.magnitude * 0.1f), 0.1f);
                //}

            }

        }


        Vector3 lastMaxRootVelo = Vector3.zero;
        bool wasRootMotion = false;
        private void OnAnimatorMove()
        {
            if (disableRigMove == false) return; // not executing if not disabled coded rigidbody char movement

            TargetAnimator.ApplyBuiltinRootMotion();
            wasRootMotion = false;

            // we implement this function to override the default root motion.
            // this allows us to modify the positional speed before it's applied.
            if (Time.deltaTime > 0)
            {
                if (TargetAnimator.deltaPosition.sqrMagnitude > 0.0001f)
                {
                    Vector3 v = (TargetAnimator.deltaPosition) / Time.deltaTime;
                    wasRootMotion = true;

                    // Remembering max animator velocity for jump attack to apply to the ragdoll velocity
                    if (aiStage == EAIStage.StartAttacking)
                        if (v.sqrMagnitude > lastMaxRootVelo.sqrMagnitude)
                            lastMaxRootVelo = v;

                    rig.velocity = v;
                }
            }
        }

    }
}