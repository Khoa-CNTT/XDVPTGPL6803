using UnityEngine;
using StarterAssets;
using System.Collections;

namespace KLTNLongKhoi
{
    public class PlayerAttackCombo : MonoBehaviour
    {
        [SerializeField] private float nextAttackPressTime = 1f;
        [SerializeField] private int maxAttackAnimation = 3;
        [SerializeField] float timeAttack = 1f;
        [SerializeField] private AudioClip[] attackSounds;

        private CharacterAnimationEvents characterAnimEvents;
        private ActorHitbox actorHitbox;
        private Animator animator;
        private StarterAssetsInputs input;
        private ThirdPersonController controller;
        private float countdownTimeAttack;
        private int currentAnimationAttack = 0;
        private bool isAttacking = false;
        private float lastAttackTime = 0f;
        private PlayerStatsManager playerStatsManager;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            input = GetComponent<StarterAssetsInputs>();
            controller = GetComponent<ThirdPersonController>();
            actorHitbox = GetComponentInChildren<ActorHitbox>();
            playerStatsManager = FindFirstObjectByType<PlayerStatsManager>();
            characterAnimEvents = GetComponentInChildren<CharacterAnimationEvents>();
        }

        void OnEnable()
        {
            input.Attack += OnPlayerAttack;
            characterAnimEvents.onAttackComplete += OnAttackComplete;
        }

        void OnDisable()
        {
            input.Attack -= OnPlayerAttack;
            characterAnimEvents.onAttackComplete -= OnAttackComplete;
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
                actorHitbox.damage = playerStatsManager.GetPhysicsDamage;

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
            isAttacking = false;
            controller.CanMove = true;
            animator.SetInteger("Attack", 0);
            lastAttackTime = Time.time;
        }
    }
}
