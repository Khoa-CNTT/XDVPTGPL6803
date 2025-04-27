using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace KLTNLongKhoi
{
    public class EnemyCtrl : MonoBehaviour
    {
        [SerializeField] List<Transform> waypoints;
        [SerializeField] float hitRange; // Khoảng cách để tấn công
        private CharacterStatus characterStatus;
        private CharacterVision characterVision;
        private NavMeshAgent agent;
        private Animator animator;
        private bool isSeePlayer = false;
        private ActorHitbox actorHitbox;
        private bool isAttacking = false;
        private PlayerStatsManager playerStatsManager;

        // Animation IDs

        [SerializeField] private float movementSpeed = 3f;
        [SerializeField] private float idleTimeAtWaypoint = 2f; // Time to wait at each waypoint
        private float idleTimer = 0f;
        private bool isWaitingAtWaypoint = false;

        [SerializeField] private float pathCheckTimeout = 1f; // Thời gian chờ trước khi skip waypoint không đến được
        private float pathCheckTimer = 0f;

        private void Awake()
        {
            // Get required components
            characterStatus = GetComponent<CharacterStatus>();
            characterVision = GetComponentInChildren<CharacterVision>();
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            actorHitbox = GetComponentInChildren<ActorHitbox>();

            // Find PlayerStatsManager
            playerStatsManager = FindFirstObjectByType<PlayerStatsManager>();
        }
        private void Start()
        {
            // Initialize NavMeshAgent settings
            agent.speed = movementSpeed;
        }

        private void FixedUpdate()
        {
            if (characterStatus.IsDead()) return;

            if (isAttacking) return;

            isSeePlayer = characterVision.Target != null;

            // Check if the enemy is within hit range of the player
            if (isSeePlayer && Vector3.Distance(transform.position, characterVision.Target.position) <= hitRange)
            {
                SetStateAttack();
                return;
            }

            // If the enemy sees the player, move towards them
            if (isSeePlayer)
            {
                agent.SetDestination(characterVision.Target.position);
                SetStateMove();
            }
            else
            {
                Patrol();
            }
        }

        private void OnSendDamage(AnimationEvent animationEvent)
        {
            if (animationEvent.intParameter == 1)
            {
                actorHitbox.IsAttacking = true;
            }
            else
            {
                actorHitbox.IsAttacking = false;
            }
        }

        private void OnAttackComplete(AnimationEvent animationEvent)
        {
            isAttacking = false;
        }

        private void Patrol()
        {
            // Nếu đã đến gần waypoint hiện tại (trong khoảng stopping distance)
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                SetStateIdle();

                if (!isWaitingAtWaypoint)
                {
                    isWaitingAtWaypoint = true;
                    idleTimer = 0f;
                }

                idleTimer += Time.deltaTime;

                if (idleTimer >= idleTimeAtWaypoint)
                {
                    isWaitingAtWaypoint = false;
                    MoveToNextWaypoint();
                }
                return;
            }

            // Kiểm tra nếu đang có path nhưng không di chuyển được
            if (agent.hasPath && !agent.pathStatus.Equals(NavMeshPathStatus.PathComplete))
            {
                pathCheckTimer += Time.deltaTime;
                if (pathCheckTimer >= pathCheckTimeout)
                {
                    MoveToNextWaypoint();
                    pathCheckTimer = 0f;
                }
                return;
            }

            // Nếu không có path hoặc chưa di chuyển
            if (!agent.hasPath)
            {
                MoveToNextWaypoint();
            }
            else
            {
                SetStateMove();
            }
        }

        private void MoveToNextWaypoint()
        {
            if (waypoints.Count == 0) return;

            int currentIndex = waypoints.FindIndex(w => Vector3.Distance(w.position, agent.destination) < 0.1f);
            int nextWaypointIndex = (currentIndex + 1) % waypoints.Count;

            // Thử tìm waypoint tiếp theo có thể đi được
            int attempts = 0;
            while (attempts < waypoints.Count)
            {
                NavMeshPath path = new NavMeshPath();
                if (NavMesh.CalculatePath(transform.position, waypoints[nextWaypointIndex].position, NavMesh.AllAreas, path))
                {
                    if (path.status == NavMeshPathStatus.PathComplete)
                    {
                        agent.SetDestination(waypoints[nextWaypointIndex].position);
                        SetStateMove();
                        return;
                    }
                }

                // Nếu không tìm được path, thử waypoint tiếp theo
                nextWaypointIndex = (nextWaypointIndex + 1) % waypoints.Count;
                attempts++;
            }

            Debug.LogWarning("không tìm được waypoint nào có thể đi được!");
            SetStateIdle();
        }

        // Animation
        public void SetStateAttack()
        {
            if (isAttacking) return; // Prevent attack animation interruption

            isAttacking = true;
            animator.SetTrigger("Attack");
        }

        public void SetStateMove()
        {
            animator.SetFloat("Speed", movementSpeed);
            animator.SetFloat("MotionSpeed", 1f);
        }

        public void SetStateIdle()
        {
            animator.SetFloat("Speed", 0f);
            animator.SetFloat("MotionSpeed", 1f);
        }
    }
}
