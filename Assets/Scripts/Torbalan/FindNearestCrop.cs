using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class FindNearestCrop : Node {
    private Transform transform;

    public FindNearestCrop(Transform transform) {
        this.transform = transform;
    }

    public override NodeState Evaluate() {
        Crop targetCrop = InteractableManager.Instance.GetClosestHarvestableCropTo(transform.position);
        
        // if crop doesn't exist, return failure
        if (targetCrop == null) {
            state = NodeState.FAILURE;
            return state;
        }

        // set data in blackboard
        parent.parent.SetData("targetCrop", targetCrop);
        parent.parent.SetData("target", targetCrop.transform);
        state = NodeState.SUCCESS;
        return state;

    }
}