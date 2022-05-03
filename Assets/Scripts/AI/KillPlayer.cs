using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class KillPlayer : Node {

    public KillPlayer() { }

    public override NodeState Evaluate() {
        MenuManager.Instance.GameOver(false);
        
        state = NodeState.SUCCESS;
        return state;
    }

}