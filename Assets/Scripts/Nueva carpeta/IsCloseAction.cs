using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "IsClose", story: "[Self] is [Close] to [Target]", category: "Action", id: "6c6825cac2d50eb62085df820045c2a0")]
public partial class IsCloseAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<float> Close;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
       
        float currentDistance = Vector3.Distance(Self.Value.transform.position, Target.Value.transform.position);
        
        if (currentDistance <= Close.Value)
        {
            return Status.Success;
        }
        
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

