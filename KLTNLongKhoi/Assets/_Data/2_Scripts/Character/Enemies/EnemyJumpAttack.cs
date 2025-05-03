using UnityEngine;
using UnityEngine.AI;

namespace KLTNLongKhoi
{
    public class EnemyJumpAttack : MonoBehaviour
    {
        [SerializeField] private float jumpForce = 3f;
        [SerializeField] private float jumpHeight = 8f;
        [SerializeField][Tooltip("Thời gian delay để nhân vật bay")] private float delayBeforeJump = 1.5f;
        [SerializeField] private float maxDistanceToJump = 10f;
        [SerializeField] private float minDistanceToJump = 5f;
        [SerializeField] private float jumpCooldown = 10f; // thời gian hồi chiêu 
        [SerializeField] private float gravity = 10f;
        [SerializeField] private bool isJumping = false;
        [SerializeField] ActorHitbox actorHitbox2;
        private CharacterVision characterVision;
        private Animator animator;
        private CharacterController characterController;
        private EnemyCtrl enemyCtrl;
        private NavMeshAgent agent;
        private float nextJumpTime = 0f; 
        private Vector3 jumpDirection;
        private float verticalVelocity = 0f;
        private Vector3 targetPosition;

        private void Start()
        {
            characterVision = GetComponentInChildren<CharacterVision>();
            animator = GetComponent<Animator>();
            enemyCtrl = GetComponent<EnemyCtrl>();
            characterController = GetComponent<CharacterController>();
            agent = GetComponent<NavMeshAgent>();
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
                if (IsPlayerInRange() && enemyCtrl.IsAttacking == false)
                {
                    ReadyToJump();
                }
            }
        }

        private void HandleJumpMovement()
        {
            // Apply gravity
            verticalVelocity -= gravity * Time.deltaTime;

            // Create movement vector (horizontal movement)
            Vector3 moveDirection = jumpDirection * jumpForce;
            moveDirection.y = verticalVelocity;

            // Move the character
            characterController.Move(moveDirection * Time.deltaTime);

            // Check if landed
            if (characterController.isGrounded && isJumping)
            {
                CompleteJumpAttack();
            }
        }

        private void ReadyToJump()
        {
            isJumping = true;
            enemyCtrl.IsAttacking = true;
            nextJumpTime = Time.time + jumpCooldown;
            animator.SetTrigger("JumpAttack");
            agent.enabled = false;
            targetPosition = characterVision.Target.position;
            Invoke("ApplyJumpForce", delayBeforeJump);
        }

        private void ApplyJumpForce()
        {
            // Calculate jump direction

            if (characterVision.Target == null)
            {
                CompleteJumpAttack();
                return;
            }
            Vector3 directionToPlayer = (targetPosition - transform.position).normalized;
            jumpDirection = directionToPlayer;
            verticalVelocity = Mathf.Sqrt(2 * gravity * jumpHeight);
        }

        private void CompleteJumpAttack()
        {
            verticalVelocity = 0f;
            isJumping = false;
            enemyCtrl.IsAttacking = false;

            // Re-enable NavMeshAgent
            agent.enabled = true;
        }

        private void OnJumpAttackSendDamage(AnimationEvent animationEvent)
        {
            if (animationEvent.intParameter == 1)
            {
                actorHitbox2.IsAttacking = true;
            }
        }

        private bool IsPlayerInRange()
        {
            if (characterVision.Target == null) return false;
            float distanceToPlayer = Vector3.Distance(transform.position, characterVision.Target.position);
            return enemyCtrl.IsSeePlayer() && distanceToPlayer <= maxDistanceToJump && distanceToPlayer >= minDistanceToJump;
        }
    }
}



