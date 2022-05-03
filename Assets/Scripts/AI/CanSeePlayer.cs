using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class CanSeePlayer : Node {
    private TorbalanSenses senses;

    public CanSeePlayer(TorbalanSenses senses) {
        this.senses = senses;
    }

    public override NodeState Evaluate() {
        if (senses.CanSeePlayer()) {
            state = NodeState.SUCCESS;
            return state;
        }
        
        state = NodeState.FAILURE;
        return state;
    }

}