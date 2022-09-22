// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using BehaviorTree;
// using UnityEngine.AI;
//
// public class MoveToNearestCrop : Node {
//     private NavMeshAgent agent;
//     private float speed;
//
//     public MoveToNearestCrop(NavMeshAgent agent, float speed) {
//         this.agent = agent;
//         this.speed = speed;
//     }
//
//     public override NodeState Evaluate() {
//         Crop targetCrop = InteractableManager.Instance.GetClosestHarvestableCropTo(agent.transform.position);
//
//         // if no crops left
//         if (targetCrop == null) {
//             state = NodeState.FAILURE;
//             return state;
//         }
//         
//         // save target crop to blackboard
//         parent.parent.SetData("targetCrop", targetCrop);
//
//         // set destination
//         agent.speed = speed;
//         agent.SetDestination(targetCrop.transform.position);
//         
//         // check if close enough
//         Vector3 toDestination = agent.destination - agent.transform.position;
//         toDestination.y = 0; // ignore vertical component
//         float distance = toDestination.magnitude;
//         if (distance <= agent.stoppingDistance) {
//             state = NodeState.SUCCESS;
//             return state;
//         }
//
//         state = NodeState.RUNNING;
//         return state;
//     }
//
// }