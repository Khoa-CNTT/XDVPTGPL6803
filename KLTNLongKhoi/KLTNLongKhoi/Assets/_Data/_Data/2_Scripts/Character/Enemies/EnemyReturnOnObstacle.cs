using UnityEngine;
using UnityEngine.AI;

public class EnemyReturnOnObstacle : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 startPosition;

    [SerializeField] private string obstacleTag = "Obstacle"; 

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        startPosition = transform.position; 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(obstacleTag))
        {
            ReturnToStart();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(obstacleTag))
        {
            ReturnToStart();
        }
    }

    void ReturnToStart()
    {
        agent.SetDestination(startPosition);
    }
}
