using UnityEngine;
using UnityEngine.AI;

namespace KLTNLongKhoi
{
    public class EnemyJumpAttack : MonoBehaviour
    {
        [SerializeField] float damageOnAttack = 100f;
        [SerializeField] float jumpForce = 3f;
        [SerializeField] float jumpHeight = 5f;
        [SerializeField] float maxDistanceToJump = 10f;
        [SerializeField] float minDistanceToJump = 5f;
        [SerializeField] float jumpCooldown = 10f;

        [SerializeField] private AudioClip jumpSound;
        [SerializeField] private AudioClip landSound;
        private AudioSource audioSource;

        private float nextJumpTime = 10f;
        private float gravity = 10f;
        private bool isJumping = false;
        private float verticalVelocity = 0f;
        private CharacterVision characterVision;
        private Animator animator;
        private CharacterController characterController;
        private EnemyCtrl enemyCtrl;
        private NavMeshAgent agent;
        private Vector3 jumpDirection;
        private Vector3 targetPosition;
        private bool hasJustJumped = false;
        private float jumpStartTime = 0f;
        private float jumpOffet = 1;
        private ActorHitbox actorHitbox;

        private void Start()
        {
            characterVision = GetComponentInChildren<CharacterVision>();
            animator = GetComponentInChildren<Animator>();
            enemyCtrl = GetComponent<EnemyCtrl>();
            characterController = GetComponent<CharacterController>();
            agent = GetComponent<NavMeshAgent>();
            actorHitbox = GetComponentInChildren<ActorHitbox>();

            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                audioSource.spatialBlend = 0f;
                audioSource.volume = 1f;
            }
        }

        private void Update()
        {
            if (isJumping)
            {
                HandleJumpMovement();
                return;
            }

            if (Time.time >= nextJumpTime)
            {
                if (IsPlayerInRange() && enemyCtrl.CanMove)
                {
                    ReadyToJump();
                }
            }
        }

        private void HandleJumpMovement()
        {
            verticalVelocity -= gravity * Time.deltaTime;

            Vector3 moveDirection = jumpDirection * jumpForce;
            moveDirection.y = verticalVelocity;

            if (characterController == null) characterController = GetComponentInChildren<CharacterController>();
            if (characterController != null && characterController.enabled && characterController.gameObject.activeInHierarchy)
            {
                characterController.Move(moveDirection * Time.deltaTime * jumpOffet);
            }
            else
            {
                isJumping = false;
                CompleteJumpAttack();
            }

            if (!hasJustJumped && verticalVelocity > 0)
            {
                hasJustJumped = true;
                jumpStartTime = Time.time;
                jumpOffet = 1;
                animator.SetBool("FreeFall", true);
                animator.SetBool("Grounded", false);
            }

            if (hasJustJumped && Time.time - jumpStartTime > 0.5f)
            {
                if (characterController.isGrounded && verticalVelocity < 0)
                {
                    jumpOffet = 0;
                    animator.SetBool("FreeFall", false);
                    animator.SetBool("Grounded", true);
                    if (landSound != null && audioSource != null)
                    {
                        audioSource.PlayOneShot(landSound);
                    }

                    Invoke("CompleteJumpAttack", 1f);
                }
            }
        }

        private void ReadyToJump()
        {
            if (isJumping) return;
            isJumping = true;
            enemyCtrl.CanMove = false;
            nextJumpTime = Time.time + jumpCooldown;
            animator.SetBool("JumpAttack", true);
            agent.enabled = false;
            targetPosition = characterVision.GetPlayer().transform.position;
            Invoke("ApplyJumpForce", 0.3f);
        }

        private void ApplyJumpForce()
        {
            Vector3 directionToPlayer = (targetPosition - transform.position).normalized;
            jumpDirection = directionToPlayer;
            verticalVelocity = Mathf.Sqrt(2 * gravity * jumpHeight);
            hasJustJumped = false;
            actorHitbox.damage = damageOnAttack;

            if (jumpSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(jumpSound);
            }
        }

        private void CompleteJumpAttack()
        {
            isJumping = false;
            hasJustJumped = false;
            enemyCtrl.CanMove = true;
            agent.enabled = true;
            animator.SetBool("JumpAttack", false);
        }

        private bool IsPlayerInRange()
        {
            if (characterVision.GetPlayer() == null) return false;
            float distanceToPlayer = Vector3.Distance(transform.position, characterVision.GetPlayer().transform.position);
            return enemyCtrl.IsSeePlayer() && distanceToPlayer <= maxDistanceToJump && distanceToPlayer >= minDistanceToJump;
        }
    }
}
