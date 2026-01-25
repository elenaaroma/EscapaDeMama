using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BuscandoHijo", story: "[Self] is searching for [Target]", category: "Action", id: "9d1f55350458eeb38e6a5ac532c98373")]
public partial class BuscandoHijoAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> Radius;
    [SerializeReference] public BlackboardVariable<float> Angle;
    [SerializeReference] public BlackboardVariable<string> WhatIsTarget;
    [SerializeReference] public BlackboardVariable<string> WhatIsObstacle;
    
    private int whatIsTargetLayer; 
    private int whatIsObstacleLayer;
    
    private Collider[] results = new Collider[1];
    
    protected override Status OnStart()
    {
        whatIsTargetLayer = 1 << LayerMask.NameToLayer(WhatIsTarget.Value.Trim());
        whatIsObstacleLayer = 1 << LayerMask.NameToLayer(WhatIsObstacle.Value.Trim());
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Physics.OverlapSphereNonAlloc(Self.Value.transform.position, Radius, results, whatIsTargetLayer) <= 0) 
            return Status.Running;

        // 1. Definimos una posición de "ojos" (subimos 1.5 metros, por ejemplo)
        Vector3 eyePosition = Self.Value.transform.position + Vector3.up * 1.5f;
    
        // 2. La dirección ahora se calcula desde la nueva posición de ojos hacia el centro del objetivo
        Vector3 directionToTarget = results[0].bounds.center - eyePosition;

        if (Vector3.Angle(Self.Value.transform.forward, directionToTarget) > Angle) 
            return Status.Running;

        // 3. El Raycast sale desde eyePosition
        if (Physics.Raycast(eyePosition, directionToTarget, directionToTarget.magnitude, whatIsObstacleLayer)) 
            return Status.Running;

        Target.Value = results[0].gameObject;
        return Status.Success;
    }
    
}

