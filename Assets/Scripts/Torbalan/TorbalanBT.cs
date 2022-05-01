using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;
using Tree = BehaviorTree.Tree;

public class TorbalanBT : Tree {
    private NavMeshAgent agent;

    void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }

    protected override Node SetupTree() {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new FindNearestCrop(transform),
                new MoveToTarget(agent),
            }),
        });

        return root;
    }
}
