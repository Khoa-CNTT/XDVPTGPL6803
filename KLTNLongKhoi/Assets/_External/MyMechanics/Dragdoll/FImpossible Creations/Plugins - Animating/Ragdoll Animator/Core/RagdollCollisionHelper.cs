using System;
using System.Collections.Generic;
using UnityEngine;


namespace FIMSpace.FProceduralAnimation
{
    [AddComponentMenu("", 0)]
    /// <summary>
    /// Ragdoll limb indication + collision handler
    /// </summary>
    public class RagdollCollisionHelper : RagdollIndicatorHelper
    {
        private bool CollectCollisions = false;
        /// <summary> Requires to call EnableSavingEnteredCollisionsList </summary>
        public List<Collision> CollectedCollisions { get; private set; }
        public void EnableSavingEnteredCollisionsList()
        {
            CollectedCollisions = new List<Collision>();
            CollectCollisions = true;
        }

        public bool Colliding = false;
        public bool DebugLogs = false;

        public override RagdollIndicatorHelper Initialize(RagdollProcessor owner, RagdollProcessor.PosingBone c, bool isAnimatorBone = false)
        {
            LatestEnterCollision = null;
            LatestExitCollision = null;

            return base.Initialize(owner, c, isAnimatorBone);
        }

        [NonSerialized] public List<Transform> EnteredCollisions = new List<Transform>();
        [NonSerialized] public List<Transform> EnteredSelfCollisions = null;
        [NonSerialized] public List<Transform> ignores = new List<Transform>();
        internal bool CollidesJustWithSelf = false;

        public Collision LatestEnterCollision { get; private set; }
        public ContactPoint LatestContact { get; private set; }

        private void OnCollisionEnter(Collision collision)
        {
            if (ignores.Contains(collision.transform)) return;
            if (DebugLogs) UnityEngine.Debug.Log(name + " collides with " + collision.transform.name);

            LatestEnterCollision = collision;
            if (collision.contactCount > 0) LatestContact = collision.GetContact(0);
            EnteredCollisions.Add(collision.transform);
            if (CollectCollisions)
            {
                CollectedCollisions.Add(collision);
            }

            //if ( parent.IgnoreSelfCollision)
            //{
            //    if (EnteredSelfCollisions == null) EnteredSelfCollisions = new List<Transform>();
            //    if ( parent.Limbs.Contains(collision.transform) ) EnteredSelfCollisions.Add(collision.transform);
            //}

            Colliding = true;

            if (Parent != null)
            {
                Parent.OnCollisionEnterEvent(this);
            }
        }

        public Collision LatestExitCollision { get; private set; }
        private void OnCollisionExit(Collision collision)
        {
            LatestExitCollision = collision;
            EnteredCollisions.Remove(collision.transform);

            if (Parent.IgnoreSelfCollision)
            {
                if (EnteredSelfCollisions == null) EnteredSelfCollisions = new List<Transform>();
                if (Parent.Limbs.Contains(collision.transform)) EnteredSelfCollisions.Remove(collision.transform);
            }


            if (CollectCollisions)
            {
                CollectedCollisions.Remove(collision);
            }

            if (EnteredCollisions.Count == 0) Colliding = false;

            if (Parent != null)
            {
                Parent.OnCollisionExitEvent(this);
            }

        }
    }


    public partial class RagdollProcessor
    {
        /// <summary>
        /// Implement it to some MonoBehaviour to call this interface methods instead of using gameObject.SendMessage()
        /// </summary>
        public interface IRagdollAnimatorReceiver
        {
            void RagdAnim_OnCollisionExitEvent(RagdollCollisionHelper c);

            void RagdAnim_OnCollisionEnterEvent(RagdollCollisionHelper c);
        }


        [Tooltip("Game object which has component with 'IRagdollAnimatorReceiver' interface implented   OR  has public methods like 'ERagColl(RagdollCollisionHelper c)' or 'ERagCollExit(RagdollCollisionHelper c)' to handle collisions (gameObject.SendMessage approach)")]
        public GameObject SendCollisionEventsTo = null;
        public bool SendOnlyOnFreeFall = true;


        [Tooltip("Adding extra - helper components to each ragdoll collider. (it's added anyway if using collision events or blend on collision but if you don't use mentioned features, you can force adding helper enabling this toggle)")]
        public bool AlwaysAddCollisionHelpers = false;

        [Space(3)]
        [Tooltip("Not destroying colliders on the original animator skeleton")]
        public bool KeepCollidersOnAnimator = false;
        [FPD_Layers] public int ChangeAnimatorCollidersLayerTo = 0;

        [Tooltip("Removing colliders on only ragdoll representation bones on the animator")]
        public bool RemoveOnlyTargetRagdollCollidersOnAnimator = false;
        [Tooltip("Triggering physics collision ignore between the bones left on the animator and ragdoll bones")]
        public bool IgnoreAnimatorToRagdollCollision = true;
        [Tooltip("By default search is checking if some bones names starts with target name, but you can switch it to check full name to prevent issues (probably will be enabled by default in the future)")]
        public bool CheckFullBoneNamesWhenSearching = false;

        [Tooltip("Ragdoll animator calculations are required even if blend is zero, but for performance rasons you can try disabling it (it's switching ragdoll dummy colliders isKinematic!)\nNot Working with 'Blend On Collision' feature!")]
        public bool AllowDisableCalculationsOnZeroBlend = false;

        [Tooltip("Extra algorithm for syncing main ragdoll dummy rigidbodies (spine, upper arms) to animator pose on collision happening.")]
        public bool HardSyncOnCollision = true;

        bool triedFindingReceiver = false;
        IRagdollAnimatorReceiver receiveDetected = null;
        
        internal void OnCollisionEnterEvent(RagdollCollisionHelper c)
        {
            if (SendOnlyOnFreeFall) if (FreeFallRagdoll == false) return;
            if (SendCollisionEventsTo == null) return;

            if (!triedFindingReceiver) { receiveDetected = SendCollisionEventsTo.GetComponent<IRagdollAnimatorReceiver>(); triedFindingReceiver = true; }
            if (receiveDetected != null)
                receiveDetected.RagdAnim_OnCollisionEnterEvent(c);
            else
                SendCollisionEventsTo.SendMessage("ERagColl", c, SendMessageOptions.DontRequireReceiver);
        }

        internal void OnCollisionExitEvent(RagdollCollisionHelper c)
        {
            if (SendOnlyOnFreeFall) if (FreeFallRagdoll == false) return;
            if (SendCollisionEventsTo == null) return;

            if (!triedFindingReceiver) { receiveDetected = SendCollisionEventsTo.GetComponent<IRagdollAnimatorReceiver>(); triedFindingReceiver = true; }

            if (receiveDetected != null)
                receiveDetected.RagdAnim_OnCollisionExitEvent(c);
            else
                SendCollisionEventsTo.SendMessage("ERagCollExit", c, SendMessageOptions.DontRequireReceiver);
        }


    }

}