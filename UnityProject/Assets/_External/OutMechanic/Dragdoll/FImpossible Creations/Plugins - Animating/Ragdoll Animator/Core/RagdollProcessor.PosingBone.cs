using System;
using System.Collections.Generic;
using UnityEngine;

namespace FIMSpace.FProceduralAnimation
{

    public partial class RagdollProcessor
    {
        public enum ESyncMode { None, RagdollToAnimator, AnimatorToRagdoll, SyncRagdollWithParentBones }

        public class PosingBone
        {
            /// <summary> Bone transform of ragdoll - hidden influence </summary>
            public Transform transform;
            /// <summary> Bone transform of animator - visible influence </summary>
            public Transform visibleBone;

            public Transform DummyBone { get { return transform; } }
            public Transform AnimatorBone { get { return visibleBone; } }


            /// <summary> Bone transform of custom pose ragdoll - hidden influence </summary>
            public Transform customRefBone;
            public Transform riggedParent;

            /// <summary> Used when transporting ragdoll dummy bones components onto animator skeleton </summary>
            public Transform transportHelper;

            public ToAnimateBone parentFixer;

            /// <summary> Tries to correct arms rotation with shoulder rotation etc. </summary>
            public ESyncMode FullAnimatorSync = ESyncMode.None;

            public Quaternion animatorLocalRotation;
            public Vector3 animatorLocalPosition;

            public Collider collider;
            [NonSerialized] public List<Collider> extraColliders = null;
            public Rigidbody rigidbody;

            public RagdollCollisionHelper collisions;
            public RagdollProcessor owner;
            public PosingBone child;

            public float user_internalMusclePower = 1f;
            public float user_internalRagdollBlend = 1f;
            public float user_internalMuscleMultiplier = 1f;
            public float internalMusclePower = 1f;
            public float internalRagdollBlend = 0f;
            public float targetMass = 3f;

            public ConfigurableJoint ConfigurableJoint { get; private set; }
            public CharacterJoint CharacterJoint { get; private set; }
            public bool Colliding { get { return collisions.EnteredCollisions.Count > 0; } }
            public bool CollidingOnlyWithSelf { get { if (collisions.EnteredSelfCollisions == null) return false; return collisions.EnteredCollisions.Count == collisions.EnteredSelfCollisions.Count; } }

            [NonSerialized] public bool BrokenJoint = false;

            [NonSerialized] public Transform DetachParent = null;

            [NonSerialized] public bool PositionAlign = false;

            [NonSerialized] public bool UnlimitedRotationJoint = false;

            internal Quaternion initialParentLocalRotation = Quaternion.identity;
            internal Vector3 initialLocalPosition;
            internal Quaternion initialLocalRotation;
            Quaternion localConvert;
            Quaternion jointAxisConversion;
            Quaternion initialAxisCorrection;
            Rigidbody parentHasRigidbody = null;

            public PosingBone(Transform tr, RagdollProcessor owner, bool requireRigidbody = true)
            {
                // Main ----------------------------------
                transform = tr;

                if (tr == null)
                {
                    UnityEngine.Debug.Log("[Ragdoll Animator] Null Exception! Try assigning 'Root Bone' field with first bone of your skeleton!");
                }

                initialLocalPosition = tr.localPosition;
                initialLocalRotation = tr.localRotation;
                visibleBone = null;
                this.owner = owner;
                animatorLocalRotation = tr.localRotation;

                // Physical Components ----------------------------------
                collider = transform.GetComponent<Collider>();

                if (!collider)
                {
                    if (transform.childCount > 0) collider = transform.GetChild(0).GetComponent<Collider>();
                    if (!collider) collider = transform.GetComponentInChildren<Collider>();

                    if (!collider)
                        if (transform.childCount > 0)
                            collider = transform.GetChild(0).GetComponentInChildren<Collider>();
                        else
                            collider = transform.GetComponentInChildren<Collider>();
                }

                if (transform.childCount > 0)
                {
                    for (int c = 0; c < transform.childCount; c++)
                    {
                        Collider childC = transform.GetChild(c).GetComponent<Collider>();
                        if (childC == collider) continue;
                        if (transform.GetChild(c).GetComponent<Rigidbody>() == null)
                        {
                            if (extraColliders == null) extraColliders = new List<Collider>();
                            extraColliders.Add(childC);
                        }
                    }
                }

                rigidbody = transform.GetComponent<Rigidbody>();

                if (rigidbody == null)
                {
                    if (requireRigidbody)
                    {
                        UnityEngine.Debug.Log("[Ragdoll Animator] Error in the ragdoll setup! Try assigning 'Root Bone' field manually (first bone of your skeleton)");
                    }
                }
                else
                {
                    rigidbody.maxAngularVelocity = 15f;
                    //if (rigidbody == null) Debug.Log("[Ragdoll Animator] Bone " + transform.name + " is not having Rigidbody attached to it!");
                    targetMass = rigidbody.mass;
                    JointInit();

                    var colliders = transform.GetComponents<Collider>();
                    if (colliders.Length > 1)
                    {
                        for (int cc = 0; cc < colliders.Length; cc++)
                        {
                            if (colliders[cc] == collider) continue;
                            if (extraColliders == null) extraColliders = new List<Collider>();
                            extraColliders.Add(colliders[cc]);
                        }
                    }
                }

                if (tr.parent)
                {
                    parentHasRigidbody = tr.parent.GetComponent<Rigidbody>();
                }
            }

            void JointInit()
            {

                // Joints ----------------------------------
                ConfigurableJoint = rigidbody.gameObject.GetComponent<ConfigurableJoint>();

                if (ConfigurableJoint == null)
                {
                    CharacterJoint = rigidbody.gameObject.GetComponent<CharacterJoint>();
                    if (CharacterJoint) riggedParent = CharacterJoint.connectedBody.transform;
                }
                else
                {
                    riggedParent = ConfigurableJoint.connectedBody == null ? transform.parent : ConfigurableJoint.connectedBody.transform;
                }

                if (riggedParent == null) riggedParent = transform.parent;

                // Joint space preparations ----------------------------------
                if (ConfigurableJoint)
                {
                    localConvert = Quaternion.identity;

                    Vector3 forward = Vector3.Cross(ConfigurableJoint.axis, ConfigurableJoint.secondaryAxis).normalized;
                    Vector3 up = Vector3.Cross(forward, ConfigurableJoint.axis).normalized;
                    if (forward == up)
                    {
                        jointAxisConversion = Quaternion.identity;
                        initialAxisCorrection = initialLocalRotation * Quaternion.identity;
                    }
                    else
                    {
                        Quaternion toJointSpace = Quaternion.LookRotation(forward, up);
                        jointAxisConversion = Quaternion.Inverse(toJointSpace);
                        initialAxisCorrection = initialLocalRotation * toJointSpace;
                    }
                }
            }

            public void StoreAnchor()
            {
                if (ConfigurableJoint)
                    if (ConfigurableJoint.autoConfigureConnectedAnchor)
                    {
                        Vector3 anchor = ConfigurableJoint.connectedAnchor;
                        ConfigurableJoint.autoConfigureConnectedAnchor = false;
                        ConfigurableJoint.connectedAnchor = anchor;
                    }
            }

            internal void SetVisibleBone(Transform visBone)
            {
                if (visBone) if (visBone.parent) initialParentLocalRotation = visBone.parent.localRotation;
                visibleBone = visBone;
            }

            internal void CaptureAnimator()
            {
                if (parentFixer != null) parentFixer.CaptureAnimation();

                if (DetachParent)
                {
                    animatorLocalRotation = FEngineering.QToLocal(DetachParent.rotation, visibleBone.rotation);
                    return;
                }

                if (rigidbody.isKinematic)
                {
                    animWorldRot = visibleBone.rotation;
                }

                if (customRefBone)
                    animatorLocalRotation = customRefBone.localRotation;
                else
                     if (visibleBone != null)
                    animatorLocalRotation = visibleBone.localRotation;

                if (PositionAlign)
                {
                    if (customRefBone)
                    {
                        animatorLocalPosition = customRefBone.localPosition;
                        animWorldPos = (customRefBone.position);
                        animWorldRot = customRefBone.rotation;
                    }
                    else
                    {
                        animatorLocalPosition = visibleBone.localPosition;
                        animWorldPos = (visibleBone.position);
                        animWorldRot = visibleBone.rotation;
                    }

                    //animWorldPos += (owner.BaseTransform.position - animWorldPos);
                }
            }

            Vector3 animWorldPos = Vector3.zero;
            Quaternion animWorldRot = Quaternion.identity;

            public float internalForceMultiplier = 1f;
            public void FixedUpdate()
            {
                if (BrokenJoint) return;

                // Hard animator pose for kinematic bones
                if (rigidbody.isKinematic)
                {
                    rigidbody.rotation = animWorldRot;

#if UNITY_2023_1_OR_NEWER
                    rigidbody.PublishTransform();
#else
                    rigidbody.transform.rotation = animWorldRot;
#endif
                    return;
                }

                float blend = owner.RotateToPoseForce * internalForceMultiplier * internalMusclePower * user_internalMusclePower;

                // Using configurable joint for ragdoll ---------------------------------------
                if (ConfigurableJoint)
                {
                    rigidbody.solverIterations = owner.UnitySolverIterations;

                    if (ConfigurableJoint.rotationDriveMode == RotationDriveMode.Slerp)
                    {
                        var dr = ConfigurableJoint.slerpDrive;
                        dr.positionSpring = owner.OutConfigurableSpring * blend;
                        dr.positionDamper = owner.ConfigurableDamp * blend;
                        dr.maximumForce = owner.OutConfigurableSpring;
                        ConfigurableJoint.slerpDrive = dr;

                        ConfigurableJoint.angularXDrive = dr;
                        ConfigurableJoint.angularYZDrive = dr;
                    }
                    else
                    {
                        var dr = ConfigurableJoint.xDrive;
                        dr.positionSpring = owner.OutConfigurableSpring * blend;
                        dr.positionDamper = owner.ConfigurableDamp * blend;
                        dr.maximumForce = owner.OutConfigurableSpring;

                        ConfigurableJoint.xDrive = dr;
                        ConfigurableJoint.yDrive = dr;
                        ConfigurableJoint.zDrive = dr;

                        ConfigurableJoint.angularXDrive = dr;
                        ConfigurableJoint.angularYZDrive = dr;
                    }


                    ConfigurableJoint.targetRotation = ToConfigurableSpaceRotation(animatorLocalRotation);

                    if (PositionAlign)
                    {
                        var pdr = ConfigurableJoint.xDrive;
                        float spring = owner.HipsPinSpring;
                        if (owner.HipsPinV2) { spring *= 2f; spring += 500f; }

                        pdr.positionSpring = spring * blend;
                        pdr.positionDamper = owner.ConfigurableDamp * blend;
                        pdr.maximumForce = owner.OutConfigurableSpring + spring;

                        ConfigurableJoint.xDrive = pdr;
                        ConfigurableJoint.yDrive = pdr;
                        ConfigurableJoint.zDrive = pdr;

                        Transform physParent = transform.parent;


                        if (owner.HipsPinV2)
                        {
                            Matrix4x4 mx = Matrix4x4.TRS(animWorldPos, jointAxisConversion * animWorldRot, physParent.lossyScale);

                            Vector3 wPos = mx.MultiplyPoint(animatorLocalPosition - ConfigurableJoint.connectedAnchor);
                            Vector3 lPos = mx.inverse.MultiplyPoint(wPos);
                            ConfigurableJoint.targetPosition = Vector3.Scale(lPos, -owner.HipsBoostMul) + owner.HipsPositionOffset;
                        }
                        else
                        {
                            Vector3 posMap = physParent.TransformPoint(ConfigurableJoint.connectedAnchor);
                            Quaternion rotMap = physParent.rotation;

                            Matrix4x4 mx = Matrix4x4.TRS(posMap, rotMap, physParent.lossyScale);

                            Vector3 wPos = physParent.TransformPoint(animatorLocalPosition);
                            ConfigurableJoint.targetPosition = Vector3.Scale(mx.inverse.MultiplyPoint(wPos), owner.HipsBoostMul) + owner.HipsPositionOffset;
                        }

                    }

                    if (UnlimitedRotationJoint)
                    {
                        ConfigurableJoint.angularXMotion = ConfigurableJointMotion.Free;
                        ConfigurableJoint.angularYMotion = ConfigurableJointMotion.Free;
                        ConfigurableJoint.angularZMotion = ConfigurableJointMotion.Free;
                    }

                    return;
                }

                if (blend <= 0f) return;


                // Using character joints ------------------------------------------------------
                Vector3 targetAngular = FEngineering.QToAngularVelocity(rigidbody.rotation, transform.parent.rotation * animatorLocalRotation, true);

                if (user_internalMuscleMultiplier != 0f)
                    rigidbody.angularVelocity = Vector3.LerpUnclamped(rigidbody.angularVelocity, targetAngular * user_internalMuscleMultiplier, blend);
                else
                    rigidbody.angularVelocity = Vector3.LerpUnclamped(rigidbody.angularVelocity, targetAngular, blend);


            }


            Quaternion ToConfigurableSpaceRotation(Quaternion local)
            {
                return jointAxisConversion * Quaternion.Inverse(local * localConvert) * initialAxisCorrection;
            }

            internal void SyncAnimatorToRagdoll(float blend)
            {
                if (visibleBone == null) return;
                if (blend <= 0f) return;

                if (BrokenJoint)
                {
                    visibleBone.rotation = transform.rotation;
                    visibleBone.position = transform.position;
                    return;
                }

                if (blend >= 1f)
                {
                    if (DetachParent)
                        visibleBone.rotation = transform.rotation;// FEngineering.QToLocal(DetachParent.rotation, transform.rotation);
                    else
                        visibleBone.localRotation = transform.localRotation;
                }
                else
                {
                    if (DetachParent)
                        visibleBone.localRotation = Quaternion.LerpUnclamped(visibleBone.localRotation, FEngineering.QToLocal(DetachParent.rotation, transform.rotation), blend);
                    else
                        visibleBone.localRotation = Quaternion.LerpUnclamped(visibleBone.localRotation, transform.localRotation, blend);
                }

            }

            public void AlignDummyBoneWithAnimatorPose(bool local = true)
            {
                if (local)
                {
                    transform.localPosition = visibleBone.localPosition;
                    transform.localRotation = visibleBone.localRotation;
                }
                else
                {
                    transform.position = visibleBone.position;
                    transform.rotation = visibleBone.rotation;
                }
            }
        }


        public Transform GetRagdollDummyBoneByAnimatorBone(Transform tr)
        {
            PosingBone p = posingPelvis;
            while (p != null)
            {
                if (p.visibleBone == tr) return p.transform;
                p = p.child;
            }

            return null;
        }


        public PosingBone GetRagdollDummyBoneControllerByAnimatorBone(Transform tr)
        {
            PosingBone p = posingPelvis;
            while (p != null)
            {
                if (p.visibleBone == tr) return p;
                p = p.child;
            }

            return null;
        }

        public PosingBone GetRagdollDummyBoneControllerByDummyTransform(Transform tr)
        {
            PosingBone p = posingPelvis;
            while (p != null)
            {
                if (p.DummyBone == tr) return p;
                p = p.child;
            }

            return null;
        }

        public PosingBone TryGetCustomLimbChainsBone(HumanBodyBones bone)
        {
            if (bone == HumanBodyBones.Hips) return posingPelvis;
            else if (bone == HumanBodyBones.Head) return posingHead;
            else if (bone == HumanBodyBones.Spine) { return GetCustomLimbChainBoneController("spine", 0); }
            else if (bone == HumanBodyBones.Chest) { return GetCustomLimbChainBoneController("spine", 1); }
            else if (bone == HumanBodyBones.UpperChest) { return GetCustomLimbChainBoneController("spine", 2); }
            else if (bone == HumanBodyBones.Neck) { return GetCustomLimbChainBoneController("spine", 3); }
            else if (bone == HumanBodyBones.LeftShoulder) { return GetCustomLimbChainBoneController("left arm", 0); }
            else if (bone == HumanBodyBones.LeftUpperArm) { var chain = GetCustomLimbChainUsingName("left arm", false); return GetCustomLimbChainBoneController("left arm", chain.BoneSetups.Count < 5 ? 0 : 1); }
            else if (bone == HumanBodyBones.LeftLowerArm) { var chain = GetCustomLimbChainUsingName("left arm", false); return GetCustomLimbChainBoneController("left arm", chain.BoneSetups.Count < 5 ? 1 : 2); }
            else if (bone == HumanBodyBones.LeftHand) { var chain = GetCustomLimbChainUsingName("left arm", false); return GetCustomLimbChainBoneController("left arm", chain.BoneSetups.Count-1); }
            else if (bone == HumanBodyBones.LeftUpperLeg) { return GetCustomLimbChainBoneController("left leg", 0); }
            else if (bone == HumanBodyBones.LeftLowerLeg) { return GetCustomLimbChainBoneController("left leg", 1); }
            else if (bone == HumanBodyBones.LeftFoot) { return GetCustomLimbChainBoneController("left leg", 2); }
            else if (bone == HumanBodyBones.RightShoulder) { return GetCustomLimbChainBoneController("right arm", 0); }
            else if (bone == HumanBodyBones.RightUpperArm) { var chain = GetCustomLimbChainUsingName("right arm", false); return GetCustomLimbChainBoneController("right arm", chain.BoneSetups.Count < 5 ? 0 : 1); }
            else if (bone == HumanBodyBones.RightLowerArm) { var chain = GetCustomLimbChainUsingName("right arm", false); return GetCustomLimbChainBoneController("right arm", chain.BoneSetups.Count < 5 ? 1 : 2); }
            else if (bone == HumanBodyBones.RightHand) { var chain = GetCustomLimbChainUsingName("right arm", false); return GetCustomLimbChainBoneController("right arm", chain.BoneSetups.Count - 1); }
            else if (bone == HumanBodyBones.RightUpperLeg) { return GetCustomLimbChainBoneController("right leg", 0); }
            else if (bone == HumanBodyBones.RightLowerLeg) { return GetCustomLimbChainBoneController("right leg", 1); }
            else if (bone == HumanBodyBones.RightFoot) { return GetCustomLimbChainBoneController("right leg", 2); }

            return null;
        }

        public PosingBone GetRagdollBoneControllerUsingName(string name, bool caseSensitive)
        {
            if (caseSensitive == false) name = name.ToLower();

            PosingBone p = posingPelvis;

            if (caseSensitive == false)
            {
                while (p != null) { if (p.visibleBone.name.ToLower() == name) return p; p = p.child; }
            }
            else
            {
                while (p != null) { if (p.visibleBone.name == name) return p; p = p.child; }
            }

            return null;
        }

        public BonesChain GetCustomLimbChainUsingName(string name, bool caseSensitive)
        {
            if (caseSensitive == false) name = name.ToLower();

            if (caseSensitive == false)
            {
                foreach (var chain in CustomLimbsBonesChains)
                {
                    if (chain.ChainName.ToLower() == name) return chain;
                }
            }
            else
            {
                foreach (var chain in CustomLimbsBonesChains)
                {
                    if (chain.ChainName == name) return chain;
                }
            }

            return null;
        }

        public RagdollProcessor.RagdollBoneSetup GetCustomLimbChainBoneSetup(string chainName, int chainBoneIndex, bool caseSensitive = false)
        {
            var chain = GetCustomLimbChainUsingName(chainName, caseSensitive);
            if (chain == null) return null;
            if (chainBoneIndex < 0) return null;
            if (chainBoneIndex >= chain.BoneSetups.Count) return null;
            return chain.BoneSetups[chainBoneIndex];
        }

        public RagdollProcessor.PosingBone GetCustomLimbChainBoneController(string chainName, int chainBoneIndex, bool caseSensitive = false)
        {
            var boneSetup = GetCustomLimbChainBoneSetup(chainName, chainBoneIndex, caseSensitive);
            if (boneSetup == null) return null;
            return boneSetup.Posing;
        }

        Dictionary<Transform, PosingBone> _GetBoneDict = null;
        /// <summary> Using dictionary to get posing bone by animator transform bone </summary>
        public PosingBone GetRagdollDummyBoneByAnimatorBoneDict(Transform animatorTransform)
        {
            if (_GetBoneDict == null) _GetBoneDict = new Dictionary<Transform, PosingBone>();
            PosingBone bone;

            if (_GetBoneDict.TryGetValue(animatorTransform, out bone)) return bone;
            else
            {
                bone = GetRagdollDummyBoneControllerByAnimatorBone(animatorTransform);
                if (bone != null) _GetBoneDict.Add(animatorTransform, bone);
            }

            return bone;
        }



        public PosingBone GetRagdollBoneControllerByHumanoidBone(HumanBodyBones bone)
        {
            if (bone == HumanBodyBones.Hips) return GetPelvisBone();
            if (bone == HumanBodyBones.Spine) return GetSpineStartBone();
            if (bone == HumanBodyBones.Chest) return GetChestBone();
            if (bone == HumanBodyBones.UpperChest) return GetChestBone();
            if (bone == HumanBodyBones.Neck) return GetHeadBone();
            if (bone == HumanBodyBones.Head) return GetHeadBone();

            if (bone == HumanBodyBones.LeftUpperArm) return posingLeftUpperArm;
            if (bone == HumanBodyBones.RightUpperArm) return posingRightUpperArm;
            if (bone == HumanBodyBones.LeftLowerArm) return posingLeftForeArm;
            if (bone == HumanBodyBones.RightLowerArm) return posingRightForeArm;

            if (bone == HumanBodyBones.LeftUpperLeg) return posingLeftUpperLeg;
            if (bone == HumanBodyBones.RightUpperLeg) return posingRightUpperLeg;
            if (bone == HumanBodyBones.LeftLowerLeg) return posingLeftLowerLeg;
            if (bone == HumanBodyBones.RightLowerLeg) return posingRightLowerLeg;

            if (bone == HumanBodyBones.LeftFoot) return posingLeftFoot;
            if (bone == HumanBodyBones.RightFoot) return posingRightFoot;
            if (bone == HumanBodyBones.LeftShoulder) return posingLeftShoulder;
            if (bone == HumanBodyBones.RightShoulder) return posingRightShoulder;

            return null;
        }

        private List<ToAnimateBone> toReanimateBones = new List<ToAnimateBone>();

        public Vector3 DebugV3 = Vector3.zero;

        public class ToAnimateBone
        {
            public Transform animatorVisibleBone;
            public Transform dummyBone;
            public PosingBone childRagdollBone;

            /// <summary> When zero then not used </summary>
            public float InternalRagdollToAnimatorOverride = 0f;

            private Quaternion latestAnimatorBoneLocalRot;

            public ToAnimateBone(Transform animatorVisibleBone, Transform ragdollBone, PosingBone child)
            {
                this.animatorVisibleBone = animatorVisibleBone;
                this.dummyBone = ragdollBone;
                childRagdollBone = child;
                childRagdollBone.parentFixer = this;
                latestAnimatorBoneLocalRot = animatorVisibleBone.localRotation;
            }

            public void CaptureAnimation()
            {
                latestAnimatorBoneLocalRot = animatorVisibleBone.localRotation;
            }

            internal bool wasSyncing = false;
            internal void SyncRagdollBone(float ragdolledBlend, bool animatorEnabled)
            {
                wasSyncing = false;

                if (childRagdollBone.FullAnimatorSync == ESyncMode.AnimatorToRagdoll)
                {
                    if (animatorEnabled)
                    {
                        if (ragdolledBlend <= 0f)
                        {
                            dummyBone.localRotation = animatorVisibleBone.localRotation;
                        }
                        else
                        {
                            dummyBone.localRotation =
                                Quaternion.LerpUnclamped(
                                animatorVisibleBone.localRotation,
                                dummyBone.localRotation,
                                ragdolledBlend);
                        }


                        Quaternion rotDiff = animatorVisibleBone.localRotation * Quaternion.Inverse(childRagdollBone.initialParentLocalRotation);
                        childRagdollBone.animatorLocalRotation = childRagdollBone.animatorLocalRotation * (rotDiff);

                        wasSyncing = true;
                    }
                }
                else if (childRagdollBone.FullAnimatorSync == ESyncMode.RagdollToAnimator)
                {
                    if (InternalRagdollToAnimatorOverride > 0f)
                        SyncAnimatorBone(Mathf.Max(InternalRagdollToAnimatorOverride, ragdolledBlend));
                    else
                        SyncAnimatorBone(ragdolledBlend);
                }
                else if (childRagdollBone.FullAnimatorSync == ESyncMode.SyncRagdollWithParentBones)
                {
                    if (animatorEnabled)
                    {
                        dummyBone.localRotation = latestAnimatorBoneLocalRot;

                        Quaternion rotDiff = animatorVisibleBone.localRotation * Quaternion.Inverse(latestAnimatorBoneLocalRot);
                        childRagdollBone.animatorLocalRotation = childRagdollBone.animatorLocalRotation * (rotDiff);

                        wasSyncing = true;
                    }
                }

                // Useful for getup animations
                if (childRagdollBone.FullAnimatorSync != ESyncMode.RagdollToAnimator)
                    if (InternalRagdollToAnimatorOverride > 0f)
                    {
                        SyncAnimatorBone(InternalRagdollToAnimatorOverride);
                    }
            }

            internal void SyncAnimatorBone(float ragdollBlend)
            {
                wasSyncing = true;

                if (ragdollBlend >= 1f)
                {
                    animatorVisibleBone.localRotation = dummyBone.localRotation;
                }
                else
                {
                    animatorVisibleBone.localRotation = Quaternion.LerpUnclamped(
                        animatorVisibleBone.localRotation, dummyBone.localRotation,
                        ragdollBlend);
                }
            }
        }

    }
}