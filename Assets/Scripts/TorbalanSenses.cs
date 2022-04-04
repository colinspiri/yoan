using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorbalanSenses : MonoBehaviour {
    // public constants
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public float viewRadius;
    [Range(0, 360)] public float viewAngle;
    public float timeToNoticePlayer;

    // state
    private float noticeTimer;
    private bool playerWithinSight;
    private bool playerNoticed;
    
    // callback functions
    public delegate void OnPlayerEnterSight();
    public OnPlayerEnterSight onPlayerEnterSight;

    private void Start() {
        // calculate line of sight only occasionally
        StartCoroutine(LookForPlayerOnDelay(0.2f));
    }

    private void Update() {
        // player not within sight
        if (!playerWithinSight) {
            // Debug.Log("player not detected");
            playerNoticed = false;
            noticeTimer -= Time.deltaTime;
            if (noticeTimer < 0) noticeTimer = 0;

        }
        // player is within sight but not noticed
        else if (!playerNoticed) {
            // Debug.Log("player within sight... " + noticeTimer);
            noticeTimer += Time.deltaTime;
            if (noticeTimer >= timeToNoticePlayer) playerNoticed = true;
        }
        // player has been noticed
        else {
            // Debug.Log("player NOTICED!");

            // if line of sight broken
            if (!playerWithinSight) playerNoticed = false;
        }
    }

    public bool PlayerWithinSight() {
        return playerWithinSight;
    }
    public bool PlayerNoticed() {
        return playerNoticed;
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

    public Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if (!angleIsGlobal) {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
