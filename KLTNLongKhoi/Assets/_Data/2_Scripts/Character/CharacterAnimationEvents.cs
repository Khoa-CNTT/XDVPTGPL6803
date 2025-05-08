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
        Animator animator;
        
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            CheckAnimationEnd();
        }

        private void CheckAnimationEnd()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.98f)
            {
                onEndAnimation?.Invoke();
            }
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
    }
}