using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Vector3 = UnityEngine.Vector3;

public class TorbalanSenses : MonoBehaviour {
    public static TorbalanSenses Instance;
    
    // public constants
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public float viewRadius;
    [Range(0, 360)] public float viewAngle;

    // state
    private bool playerWithinSight;
    
    // callback functions
    public delegate void OnPlayerEnterSight();
    public OnPlayerEnterSight onPlayerEnterSight;
    public UnityEvent onHearPlayer;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        // calculate line of sight only occasionally
        StartCoroutine(LookForPlayerOnDelay(0.5f));
    }

    public void ReportSound(Vector3 soundOrigin, float loudness) {
        var path = new NavMeshPath();
        bool pathFound = NavMesh.CalculatePath(transform.position, soundOrigin, NavMesh.AllAreas, path);
        if (!pathFound) return;
        var length = GetPathLength(path);

        if (length <= loudness) {
            onHearPlayer?.Invoke();
        }
    }

    public bool CanSeePlayer() {
        return playerWithinSight;
    }

    private IEnumerator LookForPlayerOnDelay(float delay) {
        while (true) {
            yield return new WaitForSeconds(delay);
            LookForPlayer();
        }
    }

    private void LookForPlayer() {
        playerWithinSight = false;
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        // search through all targets in the radius
        foreach (var t in targetsInViewRadius) {
            Transform target = t.transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            // if within angle
            if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2) {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                // if no obstacles between self and player
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask)) {
                    if (!playerWithinSight) {
                        playerWithinSight = true;
                        onPlayerEnterSight?.Invoke();
                    }
                }
            }
        }
    }
    
    private float GetPathLength(NavMeshPath path) {
        if (path.status != NavMeshPathStatus.PathComplete) return 0;
        
        float length = 0;
        for (int i = 1; i < path.corners.Length; i++) {
            length += Vector3.Distance(path.corners[i - 1], path.corners[i]);
        }

        return length;
    }

    private Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if (!angleIsGlobal) {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    
    // private void OnSceneGUI() {
    //     Handles.color = Color.white;
    //     Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, viewRadius);
    //     Vector3 viewAngleA = DirectionFromAngle(-viewAngle / 2, false);
    //     Vector3 viewAngleB = DirectionFromAngle(viewAngle / 2, false);
    //     
    //     Handles.DrawLine(transform.position, transform.position + viewAngleA * viewRadius);
    //     Handles.DrawLine(transform.position, transform.position + viewAngleB * viewRadius);
    //
    //     Handles.color = Color.red;
    //     if (CanSeePlayer()) {
    //         Handles.DrawLine(transform.position, PlayerController.Instance.transform.position);
    //     }
    // }
    
    private void OnDrawGizmos() {
        Vector3 viewAngleA = DirectionFromAngle(-viewAngle / 2, false);
        Vector3 viewAngleB = DirectionFromAngle(viewAngle / 2, false);
        
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * viewRadius);

        Gizmos.color = Color.red;
        if (CanSeePlayer()) {
            Gizmos.DrawLine(transform.position, PlayerController.Instance.transform.position);
        }
    }
}
