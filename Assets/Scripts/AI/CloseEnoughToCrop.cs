using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class CloseEnoughToCrop : Node {
    private NavMeshAgent agent;

    public CloseEnoughToCrop(NavMeshAgent agent) {
        this.agent = agent;
    }

    public override NodeState Evaluate() {
        Crop targetCrop = InteractableManager.Instance.GetClosestHarvestableCropTo(agent.transform.position);
        
        // if no crops left
        if (targetCrop == null) {
            state = NodeState.FAILURE;
            return state;
        }

        // save target crop to blackboard
        parent.parent.SetData("targetCrop", targetCrop);

        // check if close enough
        agent.SetDestination(targetCrop.transform.position);
        Vector3 toDestination = agent.destination - agent.transform.position;
        toDestination.y = 0; // ignore vertical component
        float distance = toDestination.magnitude;
        if (distance <= agent.stoppingDistance) {
            state = NodeState.SUCCESS;
            return state;
        }
        
        state = NodeState.FAILURE;
        return state;

    }
}