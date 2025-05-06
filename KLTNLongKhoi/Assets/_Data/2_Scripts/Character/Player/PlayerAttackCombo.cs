using UnityEngine;
using StarterAssets;
using System.Collections;

namespace KLTNLongKhoi
{
    public class PlayerAttackCombo : MonoBehaviour
    {
        [SerializeField] private PlayerAnimationEvents playerAnimationEvents;
        [SerializeField] private float nextAttackPressTime = 1f;
        [SerializeField] private int maxAttackAnimation = 3;
        [SerializeField] float timeAttack = 1f;

        [Header("Sound Effects")]
        [SerializeField] private AudioClip[] attackSounds;

        private Animator animator;
        private StarterAssetsInputs input;
        private ThirdPersonController controller;
        private ActorHitbox actorHitbox;
        private float countdownTimeAttack;

        private int currentAnimationAttack = 0;
        private bool isAttacking = false;
        private float lastAttackTime = 0f;

        private void Start()
        {
            InitializeComponents();
            RegisterInputEvents();
            playerAnimationEvents = GetComponentInChildren<PlayerAnimationEvents>();

            playerAnimationEvents.onAttackComplete += OnAttackComplete;
            playerAnimationEvents.onSendDamage += OnSendDamage;
        }

        private void OnDestroy()
        {
            UnregisterInputEvents();
        }

        private void Update()
        {
            if (isAttacking)
            {
                countdownTimeAttack -= Time.deltaTime;
                if (countdownTimeAttack <= 0f)
                {
                    OnAttackComplete();
                }
            }
        }

        private void InitializeComponents()
        {
            animator = GetComponentInChildren<Animator>();
            input = GetComponent<StarterAssetsInputs>();
            controller = GetComponent<ThirdPersonController>();
            actorHitbox = GetComponentInChildren<ActorHitbox>();
        }

        private void RegisterInputEvents()
        {
            if (input != null)
            {
                input.Attack += OnPlayerAttack;
            }
        }

        private void UnregisterInputEvents()
        {
            if (input != null)
            {
                input.Attack -= OnPlayerAttack;
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

            if (controller.CanMove && !isAttacking && controller.Grounded)
            {
                isAttacking = true;
                controller.CanMove = false;
                animator.SetInteger("Attack", currentAnimationAttack);
                countdownTimeAttack = timeAttack;

                // Play attack sound
                PlayAttackSound();
            }
        }

        private void PlayAttackSound()
        {
            if (attackSounds != null && attackSounds.Length > 0)
            {
                // Chọn ngẫu nhiên một âm thanh từ mảng
                int soundIndex = Random.Range(0, attackSounds.Length);
                AudioClip soundToPlay = attackSounds[soundIndex];

                if (soundToPlay != null)
                {
                    AudioSource.PlayClipAtPoint(soundToPlay, transform.position);
                }
            }
        }

        // Animation Event Handlers
        private void OnAttackComplete()
        {
            if (actorHitbox != null)
            {
                actorHitbox.IsAttacking = false;
            }
            isAttacking = false;
            controller.CanMove = true;

            animator.SetInteger("Attack", 0);
            lastAttackTime = Time.time;
        }

        private void OnSendDamage()
        {
            if (actorHitbox != null)
            {
                actorHitbox.IsAttacking = true;
            }
        }
    }
}
