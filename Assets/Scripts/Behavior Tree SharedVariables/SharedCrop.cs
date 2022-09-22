using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

[System.Serializable]
public class SharedCrop : SharedVariable<Crop>
{
    public static implicit operator SharedCrop(Crop value) { return new SharedCrop { Value = value }; }
}
