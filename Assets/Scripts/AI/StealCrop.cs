using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class StealCrop : Node {

    public StealCrop() {
    }

    public override NodeState Evaluate() {
        // if no crops left
        Crop targetCrop = (Crop)GetData("targetCrop");
        if (targetCrop == null) {
            state = NodeState.FAILURE;
            return state;
        }
        
        // add to counter 
        TomatoCounter.Instance.TorbalanStoleTomato();
        
        // steal crop
        targetCrop.MakeEmpty();
        
        state = NodeState.SUCCESS;
        return state;
    }
}