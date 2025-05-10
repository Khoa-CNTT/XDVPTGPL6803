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
        [SerializeField] float jumpCooldown = 10f; // thời gian hồi chiêu

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
            // Apply gravity
            verticalVelocity -= gravity * Time.deltaTime;

            // Create movement vector
            Vector3 moveDirection = jumpDirection * jumpForce;
            moveDirection.y = verticalVelocity;

            // Move the character
            if (characterController == null) characterController = GetComponentInChildren<CharacterController>();
            if (characterController != null && characterController.enabled && characterController.gameObject.activeInHierarchy)
            {
                characterController.Move(moveDirection * Time.deltaTime * jumpOffet);
            }
            else
            {
                // Handle the case when controller is inactive
                isJumping = false;
                CompleteJumpAttack();
            }

            // Đánh dấu khi vừa nhảy và đặt thời gian bắt đầu nhảy
            if (hasJustJumped == false && verticalVelocity > 0)
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
                    // dừng lại ngay lập tức khi mới chạm đất 
                    jumpOffet = 0;
                    animator.SetBool("FreeFall", false);
                    animator.SetBool("Grounded", true);
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



