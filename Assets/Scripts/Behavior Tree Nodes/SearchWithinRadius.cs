using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Tasks.Movement {

    public class SearchWithinRadius : NavMeshMovement {

        public SharedVector3 center;
        public SharedFloat radius;
        public SharedFloat searchTime;

        public SharedFloat pauseDuration;

        private float searchTimer;

        private float reachedTime;

        public override void OnStart() {
            base.OnStart();

            SetDestination(GetRandomPosition());
            searchTimer = 0;
            reachedTime = -1;
        }


        public override TaskStatus OnUpdate() {
            searchTimer += Time.deltaTime;
            if (searchTimer >= searchTime.Value) return TaskStatus.Success;

            if (HasArrived()) {
                if (reachedTime == -1) {
                    reachedTime = Time.time;
                }

                if (reachedTime + pauseDuration.Value <= Time.time) {
                    SetDestination(GetRandomPosition());
                    reachedTime = -1;
                }
            }
            
            return TaskStatus.Running;
        }

        private Vector3 GetRandomPosition() {
            Vector2 randForward = Random.insideUnitCircle.normalized;
            float randDist = Random.value * radius.Value;
            Vector3 targetPos = center.Value + (new Vector3(randForward.x, center.Value.y, randForward.y) * randDist);
            return targetPos;
        }
    }
}