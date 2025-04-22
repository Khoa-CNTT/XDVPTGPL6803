using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace KLTNLongKhoi
{
    public class EnemyCtrl : MonoBehaviour
    {
        [SerializeField] List<Transform> waypoints;
        [SerializeField] float hitRange;
        private CharacterStatus characterStatus;
        private CharacterVision characterVision;
        private NavMeshAgent agent;
        private Animator animator;
        private bool isSeePlayer = false;
        private ActorHitbox actorHitbox;

        // Animation IDs
        private int _animIDSpeed;
        private int _animIDMotionSpeed;
        
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
            
            // Khởi tạo Animation IDs
            AssignAnimationIDs();
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void Start()
        {
            // Initialize NavMeshAgent settings
            agent.speed = movementSpeed;
            agent.stoppingDistance = 1f; // Stop before reaching the target
            agent.updateRotation = true; // Rotate towards the target
            agent.updatePosition = true; // Move towards the target

            // Set initial destination to the first waypoint
            if (waypoints.Count > 0)
            {
                agent.SetDestination(waypoints[0].position);
            }
        }

        private void FixedUpdate()
        {
            if (characterStatus.IsDead()) return;

            isSeePlayer = characterVision.Target != null;

            // Check if the enemy is within hit range of the player
            if (isSeePlayer && Vector3.Distance(transform.position, characterVision.Target.position) <= hitRange)
            {
                animator.SetTrigger("Attack");
                agent.isStopped = true;
                Debug.Log("Attack");
                return;
            }

            // If the enemy sees the player, move towards them
            if (isSeePlayer)
            {
                agent.isStopped = false;
                agent.SetDestination(characterVision.Target.position);
                animator.SetFloat(_animIDSpeed, movementSpeed);
                animator.SetFloat(_animIDMotionSpeed, 1f);
            }
            else
            {
                Patrol();
            }
        }

        private void OnSendDamage(AnimationEvent animationEvent)
        {
            if (actorHitbox != null)
            {
                actorHitbox.IsAttacking = true;
            }
        }

        private void OnAttackComplete(AnimationEvent animationEvent)
        {
            agent.isStopped = false;
            if (actorHitbox != null)
            {
                actorHitbox.IsAttacking = false;
            }
        }

        private void OnAttack(AnimationEvent animationEvent)
        {
            agent.isStopped = true;
        }

        private void Patrol()
        {
            // Nếu đã đến gần waypoint hiện tại (trong khoảng stopping distance)
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!isWaitingAtWaypoint)
                {
                    isWaitingAtWaypoint = true;
                    idleTimer = 0f;
                }

                idleTimer += Time.deltaTime;
                animator.SetFloat(_animIDSpeed, 0f);
                animator.SetFloat(_animIDMotionSpeed, 0f);

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
                // Đang di chuyển, cập nhật animation
                animator.SetFloat(_animIDSpeed, movementSpeed);
                animator.SetFloat(_animIDMotionSpeed, 1f);
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
                        animator.SetFloat(_animIDSpeed, movementSpeed);
                        animator.SetFloat(_animIDMotionSpeed, 1f);
                        return;
                    }
                }

                // Nếu không tìm được path, thử waypoint tiếp theo
                nextWaypointIndex = (nextWaypointIndex + 1) % waypoints.Count;
                attempts++;
            }

            // Nếu không tìm được waypoint nào có thể đi được
            Debug.LogWarning("No reachable waypoints found!");
            animator.SetFloat(_animIDSpeed, 0f);
            animator.SetFloat(_animIDMotionSpeed, 0f);
        }
    }
}
