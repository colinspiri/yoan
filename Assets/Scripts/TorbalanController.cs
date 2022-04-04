using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(TorbalanSenses))]
public class TorbalanController : MonoBehaviour {
    // components
    private NavMeshAgent agent;
    private TorbalanSenses senses;
    
    // public constants
    public List<Transform> passiveRoute;
    public float closeEnoughDistance;
    public float passiveSpeed;
    public float chaseSpeed;
    [Range(0, 360)] public float searchLookAngle;
    public float searchLookTime;
    
    // state
    private enum AIState { Passive, Search, Chase }
    private AIState state;
    // passive
    private int nextPassiveNode;
    // search
    private Vector3 searchLocation;
    private Coroutine searchCoroutine;
    // chase
    
    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        senses = GetComponent<TorbalanSenses>();
    }

    // Start is called before the first frame update
    void Start() {
        ChangeState(AIState.Passive);

        senses.onPlayerEnterSight += () => {
            if (state == AIState.Passive || state == AIState.Search) ChangeState(AIState.Search);
        };
    }

    // Update is called once per frame
    void Update() {
        // DEBUG
        /*if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeState(AIState.Passive);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeState(AIState.Search);
        else if(Input.GetKeyDown(KeyCode.Alpha3)) ChangeState(AIState.Chase);*/

        // state-specific updates
        if (state == AIState.Passive) UpdatePassive();
        else if (state == AIState.Search) UpdateSearch();
        else if (state == AIState.Chase) UpdateChase();
    }

    private void UpdatePassive() {
        // set next node as destination
        agent.SetDestination(passiveRoute[nextPassiveNode].position);

        // if close enough, go to next node
        if (CloseEnoughToDestination()) {
            nextPassiveNode++;
            nextPassiveNode %= passiveRoute.Count;
        }
            
        // if player noticed, chase
        if(senses.PlayerNoticed()) ChangeState(AIState.Chase);
    }

    private void UpdateSearch() {

        // if player noticed, chase
        if(senses.PlayerNoticed()) ChangeState(AIState.Chase);
    }

    private IEnumerator SearchCoroutine() {
        // go towards last known player location
        agent.SetDestination(searchLocation);

        // wait until arrived
        while (!CloseEnoughToDestination()) {
            yield return null;
        }
        
        // look left
        float t = 0;
        float startingAngle = transform.rotation.eulerAngles.y;
        float endingAngle = startingAngle + searchLookAngle;
        while (t < searchLookTime) {
            float angle = Mathf.Lerp(startingAngle, endingAngle, t / searchLookTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            t += Time.deltaTime;
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0, endingAngle, 0);

        // look right
        t = 0;
        startingAngle = transform.rotation.eulerAngles.y;
        endingAngle = startingAngle - 2 * searchLookAngle;
        while (t < searchLookTime) {
            float angle = Mathf.Lerp(startingAngle, endingAngle, t / searchLookTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            t += Time.deltaTime;
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0, endingAngle, 0);
        
        // if nothing found, go back to passive
        ChangeState(AIState.Passive);
    }

    private void UpdateChase() {
        // follow the player
        agent.SetDestination(PlayerController.Instance.transform.position);
        // if close enough to the player, game over
        if (CloseEnoughToDestination()) {
            // game over
            Debug.Log("GAME OVER");
        }
            
        // if player no longer within line of sight, search for player
        if(!senses.PlayerNoticed()) ChangeState(AIState.Search);
    }

    private void ChangeState(AIState newState) {
        // clean up old state
        if (state == AIState.Passive) {
            
        }
        else if (state == AIState.Search) {
            StopCoroutine(searchCoroutine);
            searchCoroutine = null;
        }
        else if (state == AIState.Chase) {
            
        }
        // set new state
        Debug.Log("Torbalan AI switched to " + newState);
        state = newState;
        if(state == AIState.Passive) InitializePassiveState();
        else if(state == AIState.Search) InitializeSearchState();
        else if (state == AIState.Chase) InitializeChaseState();
    }

    private void InitializePassiveState() {
        // set next passive node equal to closest node in the path
        for (int i = 0; i < passiveRoute.Count; i++) {
            var distance = Vector3.Distance(transform.position, passiveRoute[i].position);
            if (distance < Vector3.Distance(transform.position, passiveRoute[nextPassiveNode].position)) {
                nextPassiveNode = i;
            }
        }
        // set speed
        agent.speed = passiveSpeed;
    }
    private void InitializeSearchState() {
        // store last known player location
        searchLocation = PlayerController.Instance.transform.position;
        // set speed
        agent.speed = passiveSpeed;
        // start coroutine
        searchCoroutine = StartCoroutine(SearchCoroutine());
    }
    private void InitializeChaseState() {
        // set speed
        agent.speed = chaseSpeed;
    }

    private bool CloseEnoughToDestination(bool debug = false) {
        Vector3 toDestination = agent.destination - transform.position;
        // ignore vertical component
        toDestination.y = 0;
        float distance = toDestination.magnitude;
        if(debug) Debug.Log("distance to destination = " + distance + ", compared to closeEnoughDistance = " + closeEnoughDistance);
        return distance <= closeEnoughDistance;
    }
}
