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