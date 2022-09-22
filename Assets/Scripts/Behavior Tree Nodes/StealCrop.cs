// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using BehaviorTree;
//
// public class StealCrop : Node {
//
     // public StealCrop() {
     // }
     //
     // public override NodeState Evaluate() {
     //     // if no crops left
     //     Crop targetCrop = (Crop)GetData("targetCrop");
     //     if (targetCrop == null) {
     //         state = NodeState.FAILURE;
     //         return state;
     //     }
     //     
     //     // add to counter 
     //     TomatoCounter.Instance.TorbalanStoleTomato();
     //     
     //     // steal crop
     //     targetCrop.MakeEmpty();
     //     
     //     state = NodeState.SUCCESS;
     //     return state;
     // }
// }

using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class StealCrop : Action {
     public SharedCrop targetCrop;
     
     public override TaskStatus OnUpdate() {
          if (targetCrop == null) {
               return TaskStatus.Failure;
          }
          
          // add to counter
          TomatoCounter.Instance.TorbalanStoleTomato();
          
          // steal crop
          targetCrop.Value.MakeEmpty();
          
          return TaskStatus.Success;
     }
}