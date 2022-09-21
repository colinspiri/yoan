using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class EatCrop : Node {
    private SheepSpriteManager spriteManager;
    private float eatTime;
    
    // state
    private float eatTimer;
    
    public EatCrop(SheepSpriteManager spriteManager, float eatTime) {
        this.spriteManager = spriteManager;
        this.eatTime = eatTime;
    }

    public override NodeState Evaluate() {
        // if no crops left
        Crop targetCrop = (Crop)GetData("targetCrop");
        if (targetCrop == null) {
            eatTimer = 0;
            state = NodeState.FAILURE;
            return state;
        }
        
        spriteManager.ShowEating();
        
        // wait for timer
        eatTimer += Time.deltaTime;
        if (eatTimer < eatTime) {
            state = NodeState.RUNNING;
            return state;
        }
        eatTimer = 0;
        
        // steal crop
        spriteManager.ShowIdle();
        TomatoCounter.Instance.TorbalanStoleTomato();
        targetCrop.MakeEmpty();
        
        state = NodeState.SUCCESS;
        return state;
    }
}