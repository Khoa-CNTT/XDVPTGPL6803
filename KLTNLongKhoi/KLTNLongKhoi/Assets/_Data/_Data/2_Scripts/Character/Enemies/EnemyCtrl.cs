using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KLTNLongKhoi
{
    public class EnemyCtrl : MonoBehaviour
    {
        [Header("Combat")]
        [SerializeField] private float hitRange;
        [SerializeField] private float timeEndAnimAttack = 2f;
        [SerializeField] private float damageOnAttack = 100f;

        [Header("Movement & Patrol")]
        [SerializeField] private float movementSpeed = 3f;
        [SerializeField] private float idleTimeAtWaypoint = 2f;
        [SerializeField] private float pathCheckTimeout = 1f;
        [SerializeField] private List<Transform> waypoints;

        [Header("Audio")]
        [SerializeField] private AudioClip[] attackSounds;
        [SerializeField] private AudioClip[] footStepSounds;
        [SerializeField] private AudioClip roarSound;
        [SerializeField, Range(0f, 1f)] private float attackVolume = 0.7f;
        [SerializeField, Range(0f, 3f)] private float footstepVolume = 0.5f;
        [SerializeField, Range(0f, 1f)] private float roarVolume = 0.8f;

        [Header("VFX")]
        [SerializeField] private GameObject attackVFXPrefab;
        [SerializeField] private Transform attackVFXSpawnPoint;
        [SerializeField] private float destroyVFXAfter = 2f;

        [Header("References")]
        [SerializeField] private ActorHitbox actorHitbox;

        private CharacterAnimationEvents playerAnimationEvents;
        private CharacterStatus characterStatus;
        private CharacterVision characterVision;
        private Animator animator;
        private NavMeshAgent agent;
        private PlayerStatsManager playerStatsManager;
        private CharacterController characterController;
        private AudioSource audioSource;

        private bool canMove = true;
        private bool isWaitingAtWaypoint = false;
        private float idleTimer = 0f;
        private float pathCheckTimer = 0f;
        private bool hasRoared = false;

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

        private void Start()
        {
            agent.speed = movementSpeed;

            if (playerAnimationEvents != null)
            {
                playerAnimationEvents.onFootstep += OnFootStep;
            }
        }

        private void FixedUpdate()
        {
            if (characterStatus.IsDead() || !CanMove) return;

            bool isSeePlayer = characterVision.GetPlayer() != null;

            if (isSeePlayer && !hasRoared)
            {
                PlayRoarSound();
                hasRoared = true;
            }

            if (!isSeePlayer)
            {
                hasRoared = false;
            }

            if (isSeePlayer && Vector3.Distance(transform.position, characterVision.GetPlayer().transform.position) <= hitRange)
            {
                CanMove = false;
                animator.SetTrigger("Attack");
                actorHitbox.damage = damageOnAttack;
                PlayAttackSound();
                SpawnAttackVFX();
                Invoke(nameof(OnEndAttack), timeEndAnimAttack);
                return;
            }

            if (!agent.enabled) return;

            if (isSeePlayer)
            {
                agent.SetDestination(characterVision.GetPlayer().transform.position);
                SetStateMove();
            }
            else
            {
                Patrol();
            }
        }

        public void OnEndAttack()
        {
            CanMove = true;
        }

        public bool IsSeePlayer()
        {
            return characterVision.GetPlayer() != null;
        }

        private void Patrol()
        {
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

                nextWaypointIndex = (nextWaypointIndex + 1) % waypoints.Count;
                attempts++;
            }

            Debug.LogWarning("Không tìm được waypoint nào có thể đi được!");
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
                int index = Random.Range(0, footStepSounds.Length);
                AudioClip clip = footStepSounds[index];
                if (clip != null && audioSource != null)
                {
                    audioSource.PlayOneShot(clip, footstepVolume);
                }
            }
        }

        private void PlayAttackSound()
        {
            if (attackSounds != null && attackSounds.Length > 0)
            {
                int index = Random.Range(0, attackSounds.Length);
                AudioClip clip = attackSounds[index];
                if (clip != null && audioSource != null)
                {
                    audioSource.PlayOneShot(clip, attackVolume);
                }
            }
        }

        private void PlayRoarSound()
        {
            if (roarSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(roarSound, roarVolume);
            }
        }

        private void SpawnAttackVFX()
        {
            if (attackVFXPrefab == null) return;

            Vector3 spawnPosition;

            if (attackVFXSpawnPoint != null)
            {
                spawnPosition = attackVFXSpawnPoint.position;
            }
            else
            {
                spawnPosition = transform.position + transform.forward * 1f + Vector3.up * 1f;
            }

            GameObject vfx = Instantiate(attackVFXPrefab, spawnPosition, Quaternion.identity);
            Destroy(vfx, destroyVFXAfter);
        }
    }
}
