using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class MoveToTarget : Node {
    private NavMeshAgent agent;

    public MoveToTarget(NavMeshAgent agent) {
        this.agent = agent;
    }

    public override NodeState Evaluate() {
        Transform target = (Transform)GetData("target");

        // set destination
        agent.SetDestination(target.position);
        
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