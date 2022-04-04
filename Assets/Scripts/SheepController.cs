using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class SheepController : MonoBehaviour {
    // components
    private NavMeshAgent agent;
    
    // public constants
    public float walkRadius;
    public float minIdleTime, maxIdleTime;
    
    // state
    private enum AIState { Idle, Walking }
    private AIState state = AIState.Idle;
    private Vector3 targetPosition;
    private float idleTime;
    private float idleTimer;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start() {
        StartIdle();
    }

    // Update is called once per frame
    void Update() {
        if (state == AIState.Idle) {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleTime) {
                StartWalkingSomewhere();
            }
        }
        else if (state == AIState.Walking) {
            if (HasArrived()) {
                StartIdle();
            }
        }
    }

    private void StartIdle() {
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        idleTimer = 0;
        state = AIState.Idle;
    }

    private void StartWalkingSomewhere() {
        // pick random position on navmesh
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;
        NavMesh.SamplePosition(randomDirection, out var hit, walkRadius, 1);
        
        // set destination
        agent.SetDestination(hit.position);
        
        // switch state
        state = AIState.Walking;
    }

    private bool HasArrived(bool debug = false) {
        Vector3 toDestination = agent.destination - transform.position;
        toDestination.y = 0; // ignore vertical component
        float distance = toDestination.magnitude;
        if(debug) Debug.Log("distance is " + distance + ", with stoppingDistance = " + agent.stoppingDistance);
        return distance <= agent.stoppingDistance;
    }
}