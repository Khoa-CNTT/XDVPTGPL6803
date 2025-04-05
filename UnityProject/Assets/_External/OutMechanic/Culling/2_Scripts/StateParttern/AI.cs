using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    [SerializeField] Transform player;
    NavMeshAgent agent;
    Animator anim;
    State currentStage;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentStage = new Idle(gameObject, agent, anim, player);
    }

    void Update()
    {
        currentStage = currentStage.Process();
    }
}
