using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class PlaySearchStinger : Action
{

    public override TaskStatus OnUpdate()
    {
        AudioManager.Instance.PlaySearchSound();
        return TaskStatus.Success;
    }
}