using System;
using System.Collections.Generic;

namespace BehaviorTree {
    public class Sequence : Node {
        private Func<bool> modifierCondition;

        public Sequence(Func<bool> modifierCondition, List<Node> children) : base(children) {
            this.modifierCondition = modifierCondition; 
        }

        public override NodeState Evaluate() {
            // check modifier condition first
            if (!modifierCondition.Invoke()) {
                state = NodeState.FAILURE;
                return state;
            }
            
            bool anyChildIsRunning = false;

            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        return state;
                    case NodeState.SUCCESS:
                        continue;
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;
                        // anyChildIsRunning = true;
                        // continue;    
                    default:
                        state = NodeState.SUCCESS;
                        return state;
                }
            }

            state = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return state;
        }

    }

}
