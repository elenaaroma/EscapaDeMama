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
        
        whatIsTargetLayer = 1 << LayerMask.NameToLayer(WhatIsTarget.Value);
        whatIsObstacleLayer = 1 << LayerMask.NameToLayer(WhatIsObstacle.Value);
            
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        
                
        if(Physics.OverlapSphereNonAlloc(Self.Value.transform.position, Radius, results , whatIsTargetLayer) <=0) return Status.Running;
        
        //destino menos origen
        Vector3 diretionToTarget = results[0].transform.position -  Self.Value.transform.position;
        
        // Angulo, si esta fuera del angulo sigue detectando
        if(Vector3.Angle(Self.Value.transform.forward, diretionToTarget) > Angle) return Status.Running;
        //forward es mi frontal 
        //
        if(Physics.Raycast(Self.Value.transform.position, diretionToTarget, diretionToTarget.magnitude, whatIsObstacleLayer
           
           )) return Status.Running;

        
        //modificamos el valor de Target, Target es el gameoject introducido en el array 
        Target.Value = results[0].gameObject;
       
        return Status.Success;
        
    }
    
}

