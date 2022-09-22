using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
// public class MoveToSearchLocation : Node {
//     private NavMeshAgent agent;
//     private TorbalanSenses senses;
//     private float speed;
//
//     public MoveToSearchLocation(NavMeshAgent agent, TorbalanSenses senses, float speed) {
//         this.agent = agent;
//         this.senses = senses;
//         this.speed = speed;
//     }
//
//     public override NodeState Evaluate() {
//         // get player's last known location
//         Vector3 playerLastKnownLocation = senses.GetPlayerLastKnownLocation();
//         
//         // play stinger
//         AudioManager.Instance.PlaySearchSound();
//         
//         // set destination
//         agent.speed = speed;
//         agent.SetDestination(playerLastKnownLocation);
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
