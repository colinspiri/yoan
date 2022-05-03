using System;
using System.Collections.Generic;

namespace BehaviorTree
{
    public class Selector : Node {
        private Func<bool> modifierCondition;

        public Selector(Func<bool> modifierCondition, List<Node> children) : base(children) {
            this.modifierCondition = modifierCondition;
        }

        public override NodeState Evaluate() {
            // check modifier condition first
            if (!modifierCondition.Invoke()) {
                state = NodeState.FAILURE;
                return state;
            }
            
            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        continue;
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        return state;
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;
                    default:
                        continue;
                }
            }

            state = NodeState.FAILURE;
            return state;
        }

    }

}
