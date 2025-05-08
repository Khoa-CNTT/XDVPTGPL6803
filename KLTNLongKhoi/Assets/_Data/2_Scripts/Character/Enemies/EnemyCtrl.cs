using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.Serialization;

namespace KLTNLongKhoi
{
    public class EnemyCtrl : MonoBehaviour
    {
        [SerializeField] private float hitRange; // Khoảng cách để tấn công
        [SerializeField] private float movementSpeed = 3f;
        [SerializeField] private float idleTimeAtWaypoint = 2f; // Time to wait at each waypoint
        [SerializeField] private float pathCheckTimeout = 1f; // Thời gian chờ trước khi skip waypoint không đến được
        [SerializeField] private ActorHitbox actorHitbox;
        [SerializeField] private List<Transform> waypoints;
        [SerializeField] private float timeEndAnimAttack = 2f;
        [SerializeField] private float damageOnAttack = 100f;
        [SerializeField] private AudioClip[] attackSounds;
        [SerializeField] private AudioClip[] footStepSounds;

        private CharacterAnimationEvents playerAnimationEvents;
        private CharacterStatus characterStatus;
        private CharacterVision characterVision;
        private Animator animator;
        private NavMeshAgent agent;
        private PlayerStatsManager playerStatsManager;
        private CharacterController characterController;
        private bool canMove = true;
        private bool isWaitingAtWaypoint = false;
        private float idleTimer = 0f;
        private float pathCheckTimer = 0f;

        public bool CanMove { get => canMove; set => canMove = value; }

        private void Awake()
        {
            characterStatus = GetComponent<CharacterStatus>();
            characterVision = GetComponentInChildren<CharacterVision>();
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
            playerStatsManager = FindFirstObjectByType<PlayerStatsManager>();
            characterController = GetComponent<CharacterController>();
            playerAnimationEvents = GetComponentInChildren<CharacterAnimationEvents>();
        }

        private void Start()
        {
            agent.speed = movementSpeed;
            playerAnimationEvents.onFootstep += OnFootStep;
        }

        private void FixedUpdate()
        {
            if (characterStatus.IsDead()) return;
            if (CanMove == false) return;

            bool isSeePlayer = characterVision.Target != null;

            // Check if the enemy is within hit range of the player
            if (isSeePlayer && Vector3.Distance(transform.position, characterVision.Target.position) <= hitRange && CanMove)
            {
                CanMove = false;
                animator.SetTrigger("Attack");
                actorHitbox.damage = damageOnAttack;
                PlayAttackSound();
                Invoke("OnEndAttack", timeEndAnimAttack);
                return;
            }

            if (agent.enabled == false) return;

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


        // Animation
        public void OnEndAttack()
        {
            CanMove = true;
        }

        public bool IsSeePlayer()
        {
            return characterVision.Target;
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

        private void OnFootStep()
        {
            if (footStepSounds != null && footStepSounds.Length > 0)
            {
                // Chọn ngẫu nhiên một âm thanh từ mảng
                int soundIndex = Random.Range(0, footStepSounds.Length);
                AudioClip soundToPlay = footStepSounds[soundIndex];

                if (soundToPlay != null)
                {
                    AudioSource.PlayClipAtPoint(soundToPlay, transform.position);
                }
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

    }
}
