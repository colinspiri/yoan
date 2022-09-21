using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;
using Tree = BehaviorTree.Tree;

[RequireComponent(typeof(NavMeshAgent))]
public class SheepBehavior : Tree {
    // components
    private NavMeshAgent agent;
    private SheepSpriteManager spriteManager;

    // constants
    public float walkSpeed;
    public float cropNoticeDistance; // 20
    public float eatTime; // 3

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        spriteManager = GetComponent<SheepSpriteManager>();
    }

    protected override Node SetupTree() {
        Node root = new Selector(() => true, new List<Node> {
            // steal crops
            new Sequence(CropNearby, new List<Node> {
                new MoveToNearestCrop(agent, walkSpeed),
                new EatCrop(spriteManager, eatTime),
            }),
            // move randomly
            // new MoveToRandomLocation(agent, walkSpeed),
        });

        return root;
    }

    private bool CropNearby() {
        var nearestCrop = InteractableManager.Instance.GetClosestHarvestableCropTo(transform.position);
        if (nearestCrop == null) return false;
        
        float distance = Vector3.Distance(transform.position, nearestCrop.transform.position);
        return distance <= cropNoticeDistance;
    }
}