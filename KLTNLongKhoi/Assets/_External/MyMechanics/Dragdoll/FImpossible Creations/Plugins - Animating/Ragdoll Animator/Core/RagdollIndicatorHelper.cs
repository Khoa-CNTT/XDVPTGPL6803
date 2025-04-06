using UnityEngine;


namespace FIMSpace.FProceduralAnimation
{
    [AddComponentMenu("", 0)]
    /// <summary>
    /// Just ragdoll limb indication (it's added on the animator bones when using 'KeepCollidersOnAnimator'
    /// </summary>
    public class RagdollIndicatorHelper : MonoBehaviour
    {
        public RagdollProcessor Parent { get; private set; }
        /// <summary> Same as .Parent </summary>
        public RagdollProcessor ParentRagdollProcessor { get { return Parent; } }
        /// <summary> If using custom ragdoll handler, it will be null </summary>
        public RagdollAnimator ParentRagdollAnimator { get { return Parent.OwnerRagdollAnimatorComponent; } }
        /// <summary> If it's bone which is attached on animator, not on the ragdoll dummy (it happens when using 'KeepCollidersOnAnimator')</summary>
        public bool IsAnimatorBone { get; private set; }

        public RagdollProcessor.PosingBone RagdollBone { get; private set; }

        /// <summary> Assigned only when using humanoid bone setup </summary>
        public HumanBodyBones LimbID { get; private set; }

        /// <summary> Assigned only when using custom bone chains </summary>
        public string CustomBoneChainName { get; private set; }
        /// <summary> Assigned only when using custom bone chains </summary>
        public int CustomChainBoneIndex { get; private set; }
        /// <summary> Assigned only when using custom bone chains </summary>
        public RagdollProcessor.BonesChain CustomBoneChain { get; private set; }

        public void CustomBoneChainApplyInfo(RagdollProcessor.BonesChain chain, int boneIndex)
        {
            if (chain == null) return;

            CustomBoneChain = chain;
            CustomBoneChainName = chain.ChainName;
            CustomChainBoneIndex = boneIndex;
        }


        public virtual RagdollIndicatorHelper Initialize(RagdollProcessor owner, RagdollProcessor.PosingBone c, bool isAnimatorBone = false)
        {
            Parent = owner;
            LimbID = HumanBodyBones.LastBone;
            RagdollBone = c;

            if (c != null)
            {
                #region Identify Limb

                if (c == owner.GetPelvisBone()) LimbID = HumanBodyBones.Hips;
                else if (c == owner.GetSpineStartBone()) LimbID = HumanBodyBones.Spine;
                else if (c == owner.GetHeadBone()) LimbID = HumanBodyBones.Head;
                else if (c == owner.GetLeftForeArm()) LimbID = HumanBodyBones.LeftLowerArm;
                else if (c == owner.GetRightForeArm()) LimbID = HumanBodyBones.RightLowerArm;
                else if (c == owner.GetLeftUpperArm()) LimbID = HumanBodyBones.LeftUpperArm;
                else if (c == owner.GetRightUpperArm()) LimbID = HumanBodyBones.RightUpperArm;
                else if (c == owner.GetLeftUpperLeg()) LimbID = HumanBodyBones.LeftUpperLeg;
                else if (c == owner.GetLeftLowerLeg()) LimbID = HumanBodyBones.LeftLowerLeg;
                else if (c == owner.GetRightUpperLeg()) LimbID = HumanBodyBones.RightUpperLeg;
                else if (c == owner.GetRightLowerLeg()) LimbID = HumanBodyBones.RightLowerLeg;
                else if (c == owner.GetLeftShoulder()) LimbID = HumanBodyBones.LeftShoulder;
                else if (c == owner.GetRightShoulder()) LimbID = HumanBodyBones.RightShoulder;
                else if (c == owner.GetLeftHand()) LimbID = HumanBodyBones.LeftHand;
                else if (c == owner.GetRightHand()) LimbID = HumanBodyBones.RightHand;
                else if (c == owner.GetLeftFoot()) LimbID = HumanBodyBones.LeftFoot;
                else if (c == owner.GetRightFoot()) LimbID = HumanBodyBones.RightFoot;
                else if (owner.HasChest()) if (c == owner.GetChestBone()) LimbID = HumanBodyBones.Chest;

                #endregion
            }

            IsAnimatorBone = isAnimatorBone;

            return this;
        }

    }

}