using UnityEngine;
using UnityEngine.AI;

namespace KLTNLongKhoi
{
    [RequireComponent(typeof(CharacterStatus))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public class EnemyCtrl : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform playerTransform;
        private CharacterStatus characterStatus;
        private NavMeshAgent agent;
        private Animator animator;
        private State currentState;

        [Header("Enemy Settings")]
        [SerializeField] private float detectionRange = 10f;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float movementSpeed = 3f;
        [SerializeField] private float attackDamage = 10f;

        private void Awake()
        {
            // Get required components
            characterStatus = GetComponent<CharacterStatus>();
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();

            // Configure NavMeshAgent
            agent.speed = movementSpeed;
            agent.stoppingDistance = attackRange;
        }

        private void Start()
        {
            // Find player if not assigned
            if (playerTransform == null)
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                    playerTransform = player.transform;
            }

            // Initialize with Idle state
            currentState = new Idle(gameObject, agent, animator, playerTransform);
        }

        private void Update()
        {
            if (characterStatus.IsDead()) return;

            // Update current state
            currentState = currentState.Process();
        }

        // Called by animation events or other systems
        public void PerformAttack()
        {
            if (playerTransform == null) return;

            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= attackRange)
            {
                Vector3 direction = (playerTransform.position - transform.position).normalized;
                var playerStatus = playerTransform.GetComponent<CharacterStatus>();
                if (playerStatus != null)
                {
                    playerStatus.TakeDamage(attackDamage, direction);
                }
            }
        }

        // Public getters for state conditions
        public float GetDetectionRange() => detectionRange;
        public float GetAttackRange() => attackRange;
        public Transform GetPlayerTransform() => playerTransform;

        // Optional: Visualization of ranges in editor
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
