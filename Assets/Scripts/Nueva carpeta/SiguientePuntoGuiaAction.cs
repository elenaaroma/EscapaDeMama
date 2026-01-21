using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SiguientePuntoGuia", story: "Actualizar [Destino] usando [Lista] en la [Posicion]", category: "Action", id: "72854a209f781c547e3cf447c34d5bbf")]
public partial class SiguientePuntoGuiaAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Destino;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Lista;
    [SerializeReference] public BlackboardVariable<int> Posicion;
    
    protected override Status OnStart()
    {
        if (Lista.Value == null || Lista.Value.Count == 0)
        {
            return Status.Failure;
        }
        
        if (Posicion.Value >= Lista.Value.Count)
        {
            return Status.Failure; 
        }
        
        Destino.Value = Lista.Value[Posicion.Value];
        
        Posicion.Value++;

        return Status.Success;
    }
    
}

