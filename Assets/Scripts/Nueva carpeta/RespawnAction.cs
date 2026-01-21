using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI; // Añadimos esto para el NavMeshAgent
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Respawn", story: "[Self] spawn [SpawnPoint]", category: "Action", id: "873693f227d97c00fe6f7ed5cd8f4981")]
public partial class RespawnAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> SpawnPoint;

    public static event System.Action OnRespawn;
    
    protected override Status OnStart()
    {
        Self.Value.transform.position = SpawnPoint.Value.transform.position;
        Self.Value.transform.rotation = SpawnPoint.Value.transform.rotation;
        
        OnRespawn?.Invoke();
        
        return Status.Success;
    }

    protected override Status OnUpdate() => Status.Success;
}