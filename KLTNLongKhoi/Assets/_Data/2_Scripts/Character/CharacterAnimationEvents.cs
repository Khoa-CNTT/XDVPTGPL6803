using UnityEngine;
using UnityEngine.Events;
using System;

namespace KLTNLongKhoi
{
    public class CharacterAnimationEvents : MonoBehaviour
    {
        public event Action onAttackComplete;
        public event Action<int> onSendDamage;
        public event Action<float> onJumpLand;
        public event Action onEndAnimation;
        public event Action onFootstep;
        public event Action onDrinkBottle;
        Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void CheckAnimationEnd(AnimationEvent animationEvent)
        {
            onEndAnimation?.Invoke();
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            onFootstep?.Invoke();
        }

        private void OnJumpLand(AnimationEvent animationEvent)
        {
            onJumpLand?.Invoke(animationEvent.animatorClipInfo.weight);
        }

        // Animation Event Handlers
        private void OnAttackComplete(AnimationEvent animationEvent)
        {
            onAttackComplete?.Invoke();
        }

        private void OnSendDamage(AnimationEvent animationEvent)
        {
            onSendDamage?.Invoke(animationEvent.intParameter);
        }

        private void OnDrinkBottle(AnimationEvent animationEvent)
        {
            onDrinkBottle?.Invoke();
        }
    }
}