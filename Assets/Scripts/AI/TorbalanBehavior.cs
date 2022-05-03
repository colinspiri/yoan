using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;
using Tree = BehaviorTree.Tree;

[RequireComponent(typeof(NavMeshAgent), typeof(TorbalanSenses))]
public class TorbalanBehavior : Tree {
    // components
    private NavMeshAgent agent;
    private TorbalanSenses senses;

    // constants
    public float walkSpeed;
    public float chaseSpeed;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        senses = GetComponent<TorbalanSenses>();
    }

    protected override Node SetupTree() {
        Node root = new Selector(new List<Node> {
            // chase player
            new Sequence(new List<Node> {
                new CanSeePlayer(senses),
                new MoveToPlayer(agent, chaseSpeed),
                new KillPlayer()
            }),
            // steal crops
            new Sequence(new List<Node> {
                new MoveToNearestCrop(agent, walkSpeed),
                new StealCrop()
            }),
        });

        return root;
    }
}
