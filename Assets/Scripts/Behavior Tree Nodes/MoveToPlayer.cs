// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using BehaviorTree;
// using UnityEngine.AI;
//
// public class MoveToPlayer : Node {
//     private NavMeshAgent agent;
//     private float speed;
//
//     public MoveToPlayer(NavMeshAgent agent, float speed) {
//         this.agent = agent;
//         this.speed = speed;
//     }
//
//     public override NodeState Evaluate() {
//         GameObject player = PlayerController.Instance.gameObject;
//
//         // save last known location to blackboard
//         parent.parent.SetData("lastKnownPlayerLocation", player.transform.position);
//         
//         // play stinger
//         AudioManager.Instance.PlayChaseSound();
//         
//         // set destination
//         agent.speed = speed;
//         agent.SetDestination(player.transform.position);
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