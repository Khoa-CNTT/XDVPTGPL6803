using System.Collections;
using UnityEngine;


namespace FIMSpace.FProceduralAnimation
{
    public partial class RagdollAnimator
    {

        #region Getting Bones Related Methods

        /// <summary> Returning ragdolled bone rigidbody - can return null! 
        /// For more detailed data, use ragdollAnim.Parameters.GetRagdollBoneControllerByHumanoidBone </summary>
        public Rigidbody User_GetRagdollBone(HumanBodyBones humanoidBone)
        {
            var bone = Parameters.GetRagdollBoneControllerByHumanoidBone(humanoidBone);
            if (bone == null) return null;
            return bone.rigidbody;
        }

        /// <summary> Returning ragdoll bone rigidbody. Can return null!
        /// For more detailed data, use ragdollAnim.Parameters.GetRagdollDummyBoneControllerByAnimatorBone </summary>
        public Rigidbody User_GetRagdollBoneUsingCharacterBone(Transform characterBone)
        {
            var bone = Parameters.GetRagdollDummyBoneControllerByAnimatorBone(characterBone);
            if (bone == null) return null;
            return bone.rigidbody;
        }

        /// <summary> Returning ragdoll bone rigidbody. Can return null!
        /// For more detailed data, use ragdollAnim.Parameters.GetRagdollDummyBoneControllerByAnimatorBone </summary>
        public Rigidbody User_GetRagdollBoneUsingName(string boneName, bool caseSensitive = false)
        {
            var bone = Parameters.GetRagdollBoneControllerUsingName(boneName, caseSensitive);
            if (bone == null) return null;
            return bone.rigidbody;
        }

        /// <summary> Returning ragdolled bone controller - can return null! </summary>
        public RagdollProcessor.PosingBone User_GetRagdollBoneController(HumanBodyBones humanoidBone)
        {
            return Parameters.GetRagdollBoneControllerByHumanoidBone(humanoidBone);
        }

        /// <summary> Returning bone controller which contains ragdoll dummy transform, rigidbody, collider etc. Can return null! </summary>
        public RagdollProcessor.PosingBone User_GetRagdollBoneControllerUsingCharacterBone(Transform characterBone)
        {
            return Parameters.GetRagdollDummyBoneControllerByAnimatorBone(characterBone);
        }

        /// <summary> Returning bone controller which contains ragdoll dummy transform, rigidbody, collider etc. Can return null! </summary>
        public RagdollProcessor.PosingBone User_GetRagdollBoneControllerUsingName(string boneName, bool caseSensitive = false)
        {
            return Parameters.GetRagdollBoneControllerUsingName(boneName, caseSensitive);
        }

        /// <summary> [For custom limb bone chains] Searching custom limbs setup by names, for desired humanoid bone reference. Can return null! </summary>
        public RagdollProcessor.PosingBone User_TryGetCustomLimbChainsBone(HumanBodyBones humanoidBone)
        {
            return Parameters.TryGetCustomLimbChainsBone(humanoidBone);
        }

        /// <summary> [For custom limb bone chains] Returning bone controller which contains ragdoll dummy transform, rigidbody, collider etc. Can return null! </summary>
        public RagdollProcessor.BonesChain User_GetCustomLimbChainUsingName(string chainName, bool caseSensitive = false)
        {
            return Parameters.GetCustomLimbChainUsingName(chainName, caseSensitive);
        }

        /// <summary> [For custom limb bone chains] Returning custom limb chain bone setup, it contains bone controller and other bone setup data </summary>
        public RagdollProcessor.RagdollBoneSetup User_GetCustomLimbChainBoneSetup(string chainName, int chainBoneIndex, bool caseSensitive = false)
        {
            return Parameters.GetCustomLimbChainBoneSetup(chainName, chainBoneIndex, caseSensitive);
        }

        /// <summary> [For custom limb bone chains] Returning bone controller which contains ragdoll dummy transform, rigidbody, collider etc. Can return null! </summary>
        public RagdollProcessor.PosingBone User_GetCustomLimbChainBoneController(string chainName, int chainBoneIndex, bool caseSensitive = false)
        {
            var boneSetup = User_GetCustomLimbChainBoneSetup(chainName, chainBoneIndex, caseSensitive);
            if (boneSetup == null) return null;
            return boneSetup.Posing;
        }

        /// <summary> [For custom limb bone chains] Returning ragdoll dummy bone rigidbody </summary>
        public Rigidbody User_GetCustomLimbChainBone(string chainName, int chainBoneIndex, bool caseSensitive = false)
        {
            var posingBone = User_GetCustomLimbChainBoneController(chainName, chainBoneIndex, caseSensitive);
            if (posingBone == null) return null;
            return posingBone.rigidbody;
        }

        #endregion


        #region Impacts Related Methods


        /// <summary>
        /// Adding physical push impact to single rigidbody limb
        /// </summary>
        /// <param name="limb"> Access 'Parameters' for ragdoll limb </param>
        /// <param name="powerDirection"> World space direction vector </param>
        /// <param name="duration"> Time in seconds, set zero to impact just once </param>
        public void User_SetLimbImpact(Rigidbody limb, Vector3 powerDirection, float duration, ForceMode forceMode = ForceMode.Impulse)
        {
            if (duration <= 0f) // No impact duration - add impulse once
            {
                RagdollProcessor.ApplyLimbImpact(limb, powerDirection, forceMode);
                return;
            }

            StartCoroutine(Processor.User_SetLimbImpact(limb, powerDirection, duration, forceMode));
        }

        public void User_SetAllLimbsVelocity(Vector3 velocity, float delay = 0f, bool waitExtraFixedStep = true)
        {
            if (delay > 0f)
            {
                StartCoroutine(IECallAfter(delay, () => { User_SetAllLimbsVelocity(velocity, 0f); }, waitExtraFixedStep));
                return;
            }

            Parameters.User_SetAllLimbsVelocity(velocity);
        }

        /// <summary>
        /// Same as 'User_SetLimbImpact' but also weakening muscles and switching to Free Fall Ragdoll.
        /// Adding physical push impact to single rigidbody limb
        /// </summary>
        /// <param name="fadeMusclesTo"> from 0 to 1, default can be 0.05 for fall weaker muscles. Can be null to not change muscles power. </param>
        /// <param name="limb"> Access 'Parameters' for ragdoll limb </param>
        /// <param name="powerDirection"> World space direction vector </param>
        /// <param name="duration"> Time in seconds, set zero to impact just once </param>
        public void User_SetFallAndLimbImpact(float? fadeMusclesTo, Rigidbody limb, Vector3 powerDirection, float duration, ForceMode forceMode = ForceMode.Impulse)
        {
            if (fadeMusclesTo != null) User_FadeMuscles(fadeMusclesTo.Value, 0.6f, 0.1f);
            User_SwitchFreeFallRagdoll(true);
            User_SetLimbImpact(limb, powerDirection, duration, forceMode);
        }



        /// <summary>
        /// Adding physical push impact to all limbs of the ragdoll
        /// </summary>
        /// <param name="powerDirection"> World space direction vector </param>
        /// <param name="duration"> Time in seconds </param>
        public void User_SetPhysicalImpactAll(Vector3 powerDirection, float duration, ForceMode forceMode = ForceMode.Impulse)
        {
            if (duration <= 0f)
            {
                RagdollProcessor.PosingBone c = Parameters.GetPelvisBone();

                while (c != null)
                {
                    if (c.rigidbody) c.rigidbody.AddForce(powerDirection, forceMode);
                    c = c.child;
                }
            }
            else
            {
                StartCoroutine(Processor.User_SetPhysicalImpactAll(powerDirection, duration, forceMode));
            }
        }

        /// <summary>
        /// Same as 'User_SetPhysicalImpactAll' but also weakening muscles and switching to Free Fall Ragdoll.
        /// Adding physical push impact to all limbs of the ragdoll
        /// </summary>
        /// <param name="fadeMusclesTo"> from 0 to 1, default can be 0.05 for fall weaker muscles. Can be null to not change muscles power. </param>
        /// <param name="powerDirection"> World space direction vector </param>
        /// <param name="duration"> Time in seconds </param>
        public void User_SetFallAndPhysicalImpactAll(float? fadeMusclesTo, Vector3 powerDirection, float duration, ForceMode forceMode = ForceMode.Impulse)
        {
            if (fadeMusclesTo != null) User_FadeMuscles(fadeMusclesTo.Value, 0.6f, 0.1f);

            User_SwitchFreeFallRagdoll(true);
            User_SetPhysicalImpactAll(powerDirection, duration, forceMode);
        }

        /// <summary>
        /// Adding physical torque impact to the core limbs
        /// </summary>
        /// <param name="rotationPower"> Rotation angles torque power </param>
        /// <param name="duration"> Time in seconds </param>
        public void User_SetPhysicalTorque(Vector3 rotationPower, float duration, bool relativeSpace = false, ForceMode forceMode = ForceMode.Impulse, bool deltaScale = false)
        {
            if (deltaScale)
            {
                if (Time.fixedDeltaTime > 0f) rotationPower /= Time.fixedDeltaTime;
            }

            if (duration <= 0f)
            {
                RagdollProcessor.PosingBone c = Parameters.GetPelvisBone();

                while (c != null)
                {
                    if (c.rigidbody)
                    {
                        if (relativeSpace) c.rigidbody.AddRelativeTorque(rotationPower, forceMode);
                        else c.rigidbody.AddTorque(rotationPower, forceMode);
                    }

                    c = c.child;
                }
            }
            else
            {
                StartCoroutine(Processor.User_SetPhysicalTorque(rotationPower, duration, relativeSpace, forceMode));
            }
        }

        /// <summary>
        /// Adding physical torque impact to the selected limb
        /// </summary>
        /// <param name="rotationPower"> Rotation angles torque power </param>
        /// <param name="duration"> Time in seconds </param>
        public void User_SetPhysicalTorque(Rigidbody limb, Vector3 rotationPower, float duration, bool relativeSpace = false, ForceMode forceMode = ForceMode.Impulse, bool deltaScale = false)
        {
            if (deltaScale)
            {
                if (Time.fixedDeltaTime > 0f) rotationPower /= Time.fixedDeltaTime;
            }

            if (duration <= 0f)
            {
                if (relativeSpace)
                    limb.AddRelativeTorque(rotationPower, forceMode);
                else
                    limb.AddTorque(rotationPower, forceMode);
                return;
            }

            StartCoroutine(Processor.User_SetPhysicalTorque(limb, rotationPower, duration, relativeSpace, forceMode));
        }


        /// <summary>
        /// Adding physical torque impact to the core limbs by local euler angles for example of the baseTransform
        /// </summary>
        public void User_SetPhysicalTorqueFromLocal(Vector3 localEuler, Transform localOf, float duration, Vector3? power = null)
        {
            Quaternion rot = FEngineering.QToWorld(localOf.rotation, Quaternion.Euler(localEuler));
            Vector3 angles = FEngineering.WrapVector(rot.eulerAngles);

            if (power != null) angles = Vector3.Scale(angles, power.Value);

            StartCoroutine(Processor.User_SetPhysicalTorque(angles, duration, false));
        }


        /// <summary>
        /// Setting defined velocity value for all limbs of the ragdoll dummy
        /// </summary>
        public void User_SetVelocityAll(Vector3 newVelocity)
        {
            Processor.User_SetAllLimbsVelocity(newVelocity);
        }


        /// <summary>
        /// Adding physical push impact to single rigidbody limb
        /// </summary>
        /// <param name="limb"> Humanoid bone type </param>
        /// <param name="powerDirection"> World space direction vector </param>
        /// <param name="duration"> Time in seconds, set zero to impact just once </param>
        public void User_SetHumanoidLimbImpact(HumanBodyBones limb, Vector3 powerDirection, float duration, ForceMode forceMode = ForceMode.Impulse)
        {
            var bone = Parameters.GetRagdollBoneControllerByHumanoidBone(limb);
            if (bone == null) return;
            User_SetLimbImpact(bone.rigidbody, powerDirection, duration, forceMode);
        }


        #endregion


        #region Extra Ragdoll Control Methods


        /// <summary>
        /// Change 'Repose Mode' (parameter in the 'Extra' category) to target and restore to the component's initial (saved at Start) repose value after the delay
        /// </summary>
        public void User_ChangeReposeAndRestore(RagdollProcessor.EBaseTransformRepose set, float restoreAfter)
        {
            StartCoroutine(IEChangeReposeAndRestore(set, restoreAfter));
        }

        /// <summary>
        /// Change Blend On Collision (parameter in 'Play' category) to some value and restore it to previous value after delay
        /// </summary>
        public void User_ChangeBlendOnCollisionAndRestore(bool temporaryBlend, float delay)
        {
            StartCoroutine(IEChangeBlendOnCollAndRestore(temporaryBlend, delay));
        }


        /// <summary>
        /// Enable free fall ragdoll mode with delay (or no delay if delay argument == 0)
        /// </summary>
        public void User_SwitchFreeFallRagdoll(bool freeFall, float delay = 0f)
        {
            if (freeFall == Parameters.FreeFallRagdoll) return;

            if (delay > 0f)
            {
                StartCoroutine(IESwitchFreeFallRagdoll(freeFall, delay));
                return;
            }
            else
            {
                Parameters.SwitchFreeFallRagdoll(freeFall);
            }
        }

        /// <summary>
        /// Disable free fall ragdoll mode (switch to stance ragdoll) with delay (or no delay if delay argument == 0)
        /// </summary>
        public void User_TransitionToNonFreeFallRagdoll(float duration, float delay = 0f)
        {
            if (Parameters.FreeFallRagdoll == false) return;
            StartCoroutine(Parameters.User_TransitionToNonFreeFallRagdoll(duration, delay));
        }


        /// <summary>
        /// Transitioning 'Ragdoll Blend' value
        /// </summary>
        public void User_EnableFreeRagdoll(float blend = 1f, float transitionDuration = 0.2f)
        {
            Parameters.SwitchFreeFallRagdoll(true);
            User_FadeRagdolledBlend(blend, transitionDuration);
        }


        /// <summary>
        /// Enable / disable animator component with delay
        /// </summary>
        public void User_SwitchAnimator(Transform unityAnimator = null, bool enabled = false, float delay = 0f)
        {
            if (unityAnimator == null) unityAnimator = ObjectWithAnimator;
            if (unityAnimator == null) return;

            Animator an = unityAnimator.GetComponent<Animator>();
            if (an)
            {
                if (delay <= 0f)
                    an.enabled = enabled;
                else
                    StartCoroutine(Processor.User_SwitchAnimator(an, enabled, delay));
            }
        }



        /// <summary>
        /// Triggering different methods which are used in the demo scene for animating getting up from ragdolled state
        /// </summary>
        /// <param name="groundMask"></param>
        public void User_GetUpStack(RagdollProcessor.EGetUpType getUpType, LayerMask groundMask, float targetRagdollBlend = 0f, float targetMusclesPower = 0.85f, float duration = 1.1f)
        {
            StopAllCoroutines();
            Parameters.SafetyResetAfterCouroutinesStop();
            User_SwitchAnimator(null, true);
            User_ForceRagdollToAnimatorFor(duration * 0.5f, duration * 0.15f);
            Parameters.FreeFallRagdoll = false;
            User_FadeMuscles(targetMusclesPower, duration, duration * 0.125f);
            User_FadeRagdolledBlend(targetRagdollBlend, duration, duration * 0.125f);
            User_RepositionRoot(null, null, getUpType, groundMask);
            Parameters._User_GetUpResetProbe();
        }

        /// <summary>
        /// Just fre fall off, fade muscles, fade ragdolled blend
        /// + repose switch to none and blend on collisions disable
        /// </summary>
        public void User_GetUpStackV2(float targetRagdollBlend = 0f, float targetMusclesPower = 0.85f, float duration = 1.1f, float? enableBackCollisionAfter = null)
        {
            StopAllCoroutines();
            Parameters.SafetyResetAfterCouroutinesStop();
            User_FadeMuscles(targetMusclesPower, duration * 0.75f);
            User_FadeRagdolledBlend(targetRagdollBlend, duration * 0.15f, 0f);
            //User_TransitionToNonFreeFallRagdoll(duration * 0.75f); // Smooth ragdoll to animator transition (needs to be tested)
            User_SwitchFreeFallRagdoll(false, duration * 0.75f);

            User_ChangeReposeAndRestore(RagdollProcessor.EBaseTransformRepose.None, duration);

            float collRestoreDur = duration;
            if (enableBackCollisionAfter != null) collRestoreDur = enableBackCollisionAfter.Value;
            if (collRestoreDur > 0f) User_ChangeBlendOnCollisionAndRestore(false, collRestoreDur);

            Parameters._User_GetUpResetProbe();
        }


        /// <summary>
        /// Capture current animator state pose for ragdoll pose drive
        /// </summary>
        public void User_OverrideRagdollStateWithCurrentAnimationState()
        {
            Processor.User_OverrideRagdollStateWithCurrentAnimationState();
        }


        /// <summary>
        /// Force move visible animator bones to the ragdoll pose (with blending amount) if done some
        /// changes between execution order
        /// </summary>
        public void User_UpdateBonesToRagdollPose()
        {
            Processor.UpdateBonesToRagdollPose();
        }


        /// <summary>
        /// Transitioning all rigidbody muscles power to target value
        /// </summary>
        /// <param name="forcePoseEnd"> Target muscle power </param>
        /// <param name="duration"> Transition duration </param>
        /// <param name="delay"> Delay to start transition </param>
        public void User_FadeMuscles(float forcePoseEnd = 0f, float duration = 0.75f, float delay = 0f)
        {
            StartCoroutine(Parameters.User_FadeMuscles(forcePoseEnd, duration, delay));
        }

        /// <summary>
        /// Forcing applying rigidbody pose to the animator pose and fading out to zero smoothly
        /// </summary>
        public void User_ForceRagdollToAnimatorFor(float duration = 1f, float forcingFullDelay = 0.2f)
        {
            StartCoroutine(Parameters.User_ForceRagdollToAnimatorFor(duration, forcingFullDelay));
        }

        public bool IsFadeRagdollCoroutineRunning { get { return Parameters.IsFadeRagdollCoroutineRunning; } }

        /// <summary>
        /// Transitioning ragdoll blend value
        /// </summary>
        public void User_FadeRagdolledBlend(float targetBlend = 0f, float duration = 0.75f, float delay = 0f)
        {
            StartCoroutine(Parameters.User_FadeRagdolledBlend(targetBlend, duration, delay));
        }



        // -------------------------------------- New in V1.2.5

        float _User_HardSwitchOffRagdollAnimator_blend = 1f;
        Vector3 _User_HardSwitchOffRagdollAnimator_HipsAnchor;

        /// <summary>
        /// Disabling Ragdoll Animator jobs for optimization.
        /// </summary>
        /// <param name="disableCollisions"> Disabling presence of ragdoll dummy colliders in the world </param>
        public void User_HardSwitchOffRagdollAnimator(bool turnOn = false, bool disableCollisions = true)
        {
            if (turnOn)
            {
                Parameters.RagdolledBlend = 0f;
                User_FadeRagdolledBlend(_User_HardSwitchOffRagdollAnimator_blend, 0.1f, 0);
            }
            else
            {
                #region Hips pin save

                if (Parameters.HipsPin)
                {
                    var pelvConfig = Parameters.GetPelvisBone().ConfigurableJoint;
                    if (pelvConfig) _User_HardSwitchOffRagdollAnimator_HipsAnchor = pelvConfig.connectedAnchor;
                }

                #endregion

                _User_HardSwitchOffRagdollAnimator_blend = Parameters.RagdolledBlend;
            }

            enabled = turnOn;

            if (disableCollisions)
            {
                if (turnOn)
                {
                    Parameters.AlignRagdollBonesWithAnimatorBones();

                    #region Hips pin restore

                    if (Parameters.HipsPin)
                    {
                        var pelv = Parameters.GetPelvisBone();
                        pelv.transform.localRotation = pelv.initialLocalRotation;
                        var pelvConfig = pelv.ConfigurableJoint;
                        if (pelvConfig)
                        {
                            pelvConfig.autoConfigureConnectedAnchor = false;
                            pelvConfig.connectedAnchor = _User_HardSwitchOffRagdollAnimator_HipsAnchor;
                        }
                    }

                    #endregion

                }

                Parameters.RagdollDummyBase.gameObject.SetActive(turnOn);
            }

        }


        /// <summary>
        /// Call it if you want to abandom active ragdoll calculations for your character forever (optimization)
        /// but you want to keep ragdoll physics on the character. 
        /// This method will add joints to your character skeleton setted up to current lying pose.
        /// Method will also disable Unity's Animator (required to not override lying pose)
        /// If you don't need to keep physics on the character body, call User_DestroyRagdollAnimatorAndFreeze
        /// </summary>
        ///// <param name="convertToCharacterJoints"></param>
        /// <param name="removeAfter"> [If value is zero then this feature is not used!] Time in seconds to compleately destroy ragdoll animator and freeze character pose. </param>
        /// <param name="removeOnlyWhenLowVelocity"> [If value is zero then this feature is not used!] Complete removing will be triggered only when total velocity of rigidbody bones will be smaller than this value </param>
        /// <param name="destroyAndFreeze"> If using 'RemoveAfter' it decides if after time it should destroy all physical components </param>
        public void User_DestroyRagdollAnimatorAndKeepPhysics(float removeAfter = 0f, float removeOnlyWhenLowVelocity = 0f, bool destroyAndFreeze = false)
        {
            if (removeAfter <= 0f && removeOnlyWhenLowVelocity <= 0f)
            {
                _User_DestroyRagdollAnimatorAndKeepPhysics_Proceed();
            }
            else
            {
                StartCoroutine(IEDestroyRagdollAnimatorAndKeepPhysics(removeAfter, removeOnlyWhenLowVelocity, destroyAndFreeze));
            }
        }

        /// <summary>
        /// Call it if your character died and you want to keep it in the current lying pose.
        /// This method is disabling unity's Animator! It's required to avoid playing standing animations.
        /// </summary>
        public void User_DestroyRagdollAnimatorAndFreeze(float destroyAfter = 0f, bool disableAnimator = true)
        {
            if (destroyAfter <= 0f)
                _User_DestroyAndFreeze(disableAnimator);
            else
                StartCoroutine(IECallAfter(destroyAfter, () => { _User_DestroyAndFreeze(disableAnimator); }));
        }


        /// <summary>
        /// Switching animator pose to fit with current ragdoll pose and disabling ragdoll animator.
        /// It can be used like sleep mode.
        /// </summary>
        public IEnumerator User_Coroutine_DisableAnimatingAndKeepPose(Animator animatorToDisable)
        {
            animatorToDisable.enabled = false;
            Parameters.ExtendedAnimatorSync = RagdollProcessor.ESyncMode.AnimatorToRagdoll;
            Parameters.RagdolledBlend = 1f;
            yield return null;
            yield return new WaitForFixedUpdate();
            enabled = false;
        }


        #endregion


        #region Extra Methods


        /// <summary>
        /// Returning bounding box generated out of all ragdoll dummy bones
        /// </summary>
        public Bounds User_GetRagdollBonesStateBounds(bool fast = true)
        {
            return Parameters.User_GetRagdollBonesStateBounds(fast);
        }



        /// <summary>
        /// Moving character to new placement with all physics reset (can work uncorrectly on not-animated characters/with disabled animator)
        /// </summary>
        public void User_Teleport(Vector3 newWorldPos)
        {
            enabled = false;
            transform.position = newWorldPos;
            StartCoroutine(IECallAfter(0f, () => { enabled = true; }, true));
        }

        /// <summary>
        /// Setting all ragdoll limbs rigidbodies kinematic or non kinematic
        /// </summary>
        public void User_SetAllKinematic(bool kinematic = true)
        {
            Parameters.User_SetAllKinematic(kinematic);
        }

        /// <summary>
        /// Setting all ragdoll limbs rigidbodies angular speed limit (by default unity restricts it very tightly)
        /// </summary>
        public void User_SetAllAngularSpeedLimit(float angularLimit)
        {
            Parameters.User_SetAllAngularSpeedLimit(angularLimit);
        }

        /// <summary>
        /// Making pelvis kinematic and anchored to pelvis position
        /// </summary>
        public void User_AnchorPelvis(bool anchor = true, float duration = 0f)
        {
            StartCoroutine(Parameters.User_AnchorPelvis(anchor, duration));
        }

        /// <summary>
        /// Moving ragdoll controller object to fit with current ragdolled position hips
        /// </summary>
        public void User_RepositionRoot(Transform root = null, Vector3? worldUp = null, RagdollProcessor.EGetUpType getupType = RagdollProcessor.EGetUpType.None, LayerMask? snapToGround = null)
        {
            Parameters.User_RepositionRoot(root, null, worldUp, getupType, snapToGround);
        }




        #endregion


        #region User private helper methods and coroutines


        IEnumerator IEChangeReposeAndRestore(RagdollProcessor.EBaseTransformRepose set, float restoreAfter)
        {
            Parameters.ReposeMode = set;
            yield return new WaitForSeconds(restoreAfter);
            Parameters.ReposeMode = _initialReposeMode;
        }


        IEnumerator IEChangeBlendOnCollAndRestore(bool temporaryBlend, float delay)
        {
            bool toRestore = Parameters.BlendOnCollision;
            Parameters.BlendOnCollision = temporaryBlend;
            yield return new WaitForSeconds(delay);
            Parameters.BlendOnCollision = toRestore;
        }


        IEnumerator IESwitchFreeFallRagdoll(bool freeFall, float delay)
        {
            yield return new WaitForSeconds(delay);
            User_SwitchFreeFallRagdoll(freeFall);
        }


        void _User_DestroyRagdollAnimatorAndKeepPhysics_Proceed()
        {
            CopyPhysicsBackToAnimatorSkeleton();
            Parameters.FreeFallRagdoll = true;
            Parameters.RagdolledBlend = 0f;
            enabled = false;
            User_SwitchAnimator(null, false, 0f);
            Destroy(Parameters.RagdollDummyBase.gameObject);
            Destroy(this);
        }

        void CopyPhysicsBackToAnimatorSkeleton()
        {

            #region Add Rigidbody and collider

            RagdollProcessor.PosingBone c = Parameters.GetPelvisBone();

            while (c != null)
            {
                if (c.rigidbody)
                {
                    Rigidbody r = c.visibleBone.gameObject.AddComponent<Rigidbody>();
                    r.mass = c.rigidbody.mass;
                    r.drag = c.rigidbody.drag;
                    r.angularDrag = c.rigidbody.angularDrag;
                    r.interpolation = c.rigidbody.interpolation;
                    r.velocity = c.rigidbody.velocity;
                    r.angularVelocity = c.rigidbody.angularVelocity;

                    if (Parameters.KeepCollidersOnAnimator == false)
                    {
                        Transform collTarget;
                        if (c.collider.transform == c.transform) collTarget = c.visibleBone;
                        else collTarget = c.visibleBone.Find(c.collider.transform.name);
                        c.transportHelper = collTarget;

                        if (collTarget)
                        {
                            #region Collider Type Copy

                            if (c.collider is CapsuleCollider)
                            {
                                CapsuleCollider refColl = c.collider as CapsuleCollider;
                                var coll = collTarget.gameObject.AddComponent<CapsuleCollider>();
                                coll.radius = refColl.radius;
                                coll.height = refColl.height;
                                coll.center = refColl.center;
                                coll.direction = refColl.direction;
                            }
                            else if (c.collider is BoxCollider)
                            {
                                BoxCollider refColl = c.collider as BoxCollider;
                                var coll = collTarget.gameObject.AddComponent<BoxCollider>();
                                coll.size = refColl.size;
                                coll.center = refColl.center;
                            }
                            else if (c.collider is SphereCollider)
                            {
                                SphereCollider refColl = c.collider as SphereCollider;
                                var coll = collTarget.gameObject.AddComponent<SphereCollider>();
                                coll.radius = refColl.radius;
                                coll.center = refColl.center;
                            }

                            #endregion
                        }
                    }
                }

                c = c.child;
            }

            #endregion


            #region When everything stored then we can apply joints

            c = Parameters.GetPelvisBone();
            var pelv = c;
            while (c != null)
            {
                if (c.rigidbody)
                {
                    Rigidbody r = c.visibleBone.gameObject.GetComponent<Rigidbody>();
                    if (r)
                    {
                        var cign = Parameters.GetPelvisBone(); // Ignore collisions between ragdoll colliders

                        Collider coll = c.transportHelper.GetComponent<Collider>();
                        while (cign != null)
                        {
                            if (cign == c) { cign = cign.child; continue; }
                            Collider oColl = cign.transportHelper.GetComponent<Collider>();
                            if (oColl == null) { cign = cign.child; continue; }

                            var mBounds = coll.bounds; mBounds.size *= 0.9f;
                            if (mBounds.Intersects(oColl.bounds)) Physics.IgnoreCollision(coll, oColl, true);
                            cign = cign.child;
                        }


#if UNITY_EDITOR
                        if (c.CharacterJoint)
                        {
                            UnityEngine.Debug.Log("!!! [Ragdoll Animator] Character Joints transport is not implemented !!!");
                        }
#endif


                        if (c != pelv)
                            if (c.ConfigurableJoint)
                            {
                                ConfigurableJoint cf = c.visibleBone.gameObject.AddComponent<ConfigurableJoint>();
                                cf.autoConfigureConnectedAnchor = false;

                                RagdollProcessor.PosingBone connectedParent = Parameters.User_GetPosingBoneWithRagdollDummyBone(c.ConfigurableJoint.connectedBody.transform);
                                if (connectedParent != null)
                                {
                                    cf.connectedBody = connectedParent.visibleBone.GetComponent<Rigidbody>();
                                }

                                #region Angular motion variables copy

                                cf.angularXDrive = c.ConfigurableJoint.angularXDrive;
                                cf.angularXLimitSpring = c.ConfigurableJoint.angularXLimitSpring;
                                cf.angularXMotion = c.ConfigurableJoint.angularXMotion;
                                cf.angularYLimit = c.ConfigurableJoint.angularYLimit;
                                cf.angularYMotion = c.ConfigurableJoint.angularYMotion;
                                cf.angularYZDrive = c.ConfigurableJoint.angularYZDrive;
                                cf.angularYZLimitSpring = c.ConfigurableJoint.angularYZLimitSpring;
                                cf.angularZLimit = c.ConfigurableJoint.angularZLimit;
                                cf.angularZMotion = c.ConfigurableJoint.angularZMotion;

                                #endregion


                                #region Setup variables copy

                                cf.axis = c.ConfigurableJoint.axis;

                                cf.rotationDriveMode = c.ConfigurableJoint.rotationDriveMode;
                                cf.secondaryAxis = c.ConfigurableJoint.secondaryAxis;
                                cf.slerpDrive = c.ConfigurableJoint.slerpDrive;

                                #endregion


                                #region Extra variables copy

                                cf.enablePreprocessing = c.ConfigurableJoint.enablePreprocessing;
                                cf.breakForce = c.ConfigurableJoint.breakForce;
                                cf.breakTorque = c.ConfigurableJoint.breakTorque;
                                cf.configuredInWorldSpace = c.ConfigurableJoint.configuredInWorldSpace;

#if UNITY_2021_3_OR_NEWER
                                cf.connectedArticulationBody = c.ConfigurableJoint.connectedArticulationBody;
#endif
                                cf.connectedMassScale = c.ConfigurableJoint.connectedMassScale;
                                cf.enableCollision = c.ConfigurableJoint.enableCollision;
                                cf.massScale = c.ConfigurableJoint.massScale;

                                cf.projectionAngle = c.ConfigurableJoint.projectionAngle;
                                cf.projectionDistance = c.ConfigurableJoint.projectionDistance;
                                cf.projectionMode = c.ConfigurableJoint.projectionMode;

                                cf.swapBodies = c.ConfigurableJoint.swapBodies;

                                #endregion


                                #region Target and Drive variables copy

                                cf.highAngularXLimit = c.ConfigurableJoint.highAngularXLimit;
                                cf.linearLimit = c.ConfigurableJoint.linearLimit;
                                cf.linearLimitSpring = c.ConfigurableJoint.linearLimitSpring;
                                cf.lowAngularXLimit = c.ConfigurableJoint.lowAngularXLimit;

                                cf.targetAngularVelocity = c.ConfigurableJoint.targetAngularVelocity;
                                cf.targetPosition = c.ConfigurableJoint.targetPosition;
                                cf.targetRotation = c.ConfigurableJoint.targetRotation;
                                cf.targetVelocity = c.ConfigurableJoint.targetVelocity;

                                cf.xDrive = c.ConfigurableJoint.xDrive;
                                cf.xMotion = c.ConfigurableJoint.xMotion;
                                cf.yDrive = c.ConfigurableJoint.yDrive;
                                cf.yMotion = c.ConfigurableJoint.yMotion;
                                cf.zDrive = c.ConfigurableJoint.zDrive;
                                cf.zMotion = c.ConfigurableJoint.zMotion;

                                #endregion

                                cf.anchor = c.ConfigurableJoint.anchor;
                                cf.connectedAnchor = c.ConfigurableJoint.connectedAnchor;
                            }
                    }
                }

                c = c.child;
            }

            #endregion

        }


        IEnumerator IEDestroyRagdollAnimatorAndKeepPhysics(float removeAfter = 0f, float velocityBelowToRemove = 0f, bool destroyAndFreeze = false)
        {
            if (removeAfter > 0f) yield return new WaitForSeconds(removeAfter);

            if (velocityBelowToRemove > 0f)
            {
                // Wait for low velocity
                while (Parameters.User_GetSpineLimbsVelocity().magnitude > velocityBelowToRemove && Parameters.User_GetSpineLimbsAngularVelocity().magnitude > 1.25f)
                {
                    yield return null;
                }
            }

            if (destroyAndFreeze) User_DestroyRagdollAnimatorAndFreeze();
            else _User_DestroyRagdollAnimatorAndKeepPhysics_Proceed();

            yield break;
        }

        void _User_DestroyAndFreeze(bool disableAnimator = true)
        {
            Parameters.User_FreezeAndDestroyRagdollDummy(disableAnimator);
            if (autoDestroy != null) { Destroy(autoDestroy.gameObject); autoDestroy = null; }
            Destroy(this);
        }

        IEnumerator IECallAfter(float delay, System.Action act, bool waitExtraFixedStep = false)
        {
            if (waitExtraFixedStep) yield return new WaitForFixedUpdate();
            if (act == null) yield break;
            if (delay > 0) yield return new WaitForSeconds(delay);
            act.Invoke();
            yield break;
        }

        #endregion

    }
}
