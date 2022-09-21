using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class MoveToRandomLocation : Node {
    private NavMeshAgent agent;
    private float speed;
    private float radius;

    public MoveToRandomLocation(NavMeshAgent agent, float speed, float radius) {
        this.agent = agent;
        this.speed = speed;
        this.radius = radius;
    }

    public override NodeState Evaluate() {
        // pick random position on navmesh
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += agent.transform.position;
        NavMesh.SamplePosition(randomDirection, out var hit, radius, 1);
        
        // set destination
        agent.speed = speed;
        agent.SetDestination(hit.position);

        // check if close enough
        Vector3 toDestination = agent.destination - agent.transform.position;
        toDestination.y = 0; // ignore vertical component
        float distance = toDestination.magnitude;
        if (distance <= agent.stoppingDistance) {
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.RUNNING;
        return state;
    }

}