using UnityEngine;
using StarterAssets;
using System.Collections;

namespace KLTNLongKhoi
{
    public class PlayerAttackCombo : MonoBehaviour
    {
        [Header("Combo Settings")]
        [SerializeField] private float nextAttackPressTime = 1f;
        [SerializeField] private int maxAttackAnimation = 3;

        private Animator animator;
        private StarterAssetsInputs input;
        private ThirdPersonController controller;
        private ActorHitbox actorHitbox;

        private int currentAnimationAttack = 0;
        private bool isAttacking = false;
        private float lastAttackTime = 0f;

        private static readonly int AttackParam = Animator.StringToHash("Attack");

        public bool IsAttacking
        {
            get => isAttacking;
            private set
            {
                isAttacking = value;
                controller.CanMove = !value;
            }
        }

        private void Start()
        {
            InitializeComponents();
            RegisterInputEvents();
            IsAttacking = false;
        }

        private void OnDestroy()
        {
            UnregisterInputEvents();
        }

        private void InitializeComponents()
        {
            animator = GetComponent<Animator>();
            input = GetComponent<StarterAssetsInputs>();
            controller = GetComponent<ThirdPersonController>();
            actorHitbox = GetComponentInChildren<ActorHitbox>();
        }

        private void RegisterInputEvents()
        {
            if (input != null)
            {
                input.Attack.AddListener(OnPlayerAttack);
            }
        }

        private void UnregisterInputEvents()
        {
            if (input != null)
            {
                input.Attack.RemoveListener(OnPlayerAttack);
            }
        }

        private void OnPlayerAttack()
        {
            if (Time.time - lastAttackTime > nextAttackPressTime || currentAnimationAttack >= maxAttackAnimation)
            {
                currentAnimationAttack = 1;
            }
            else
            {
                currentAnimationAttack++; 
            }

            if (CanPerformAttack())
            {
                ExecuteAttack();
            }
        }

        private bool CanPerformAttack()
        {
            return controller.Grounded &&
                   !controller.IsDead &&
                   !controller.IsHurt &&
                   !IsAttacking;
        }

        private void ExecuteAttack()
        {
            
            IsAttacking = true;
            animator.SetInteger(AttackParam, currentAnimationAttack);
        }

        // Animation Event Handlers
        private void OnAttackComplete(AnimationEvent animationEvent)
        {
            if (actorHitbox != null)
            {
                actorHitbox.IsAttacking = false;
            }
            IsAttacking = false;

            animator.SetInteger(AttackParam, 0);
            IsAttacking = false;
            lastAttackTime = Time.time;
        }

        private void OnSendDamage(AnimationEvent animationEvent)
        {
            if (actorHitbox != null)
            {
                actorHitbox.IsAttacking = true;
            }
        }
    }
}
