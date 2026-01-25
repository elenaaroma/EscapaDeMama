using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using TMPro;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Dialogue", story: "[Talk] to [Player]", category: "Action", id: "ba58f807a511082f7454350ac6396985")]
public partial class DialogueAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Talk;
    [SerializeReference] public BlackboardVariable<GameObject> Player;

    private TextMeshProUGUI textUI;
    private float timer;
    private int step;

    protected override Status OnStart()
    {
     
        Talk.Value.SetActive(true);
        textUI = Talk.Value.GetComponentInChildren<TextMeshProUGUI>();
        
        step = 0;
        timer = 0;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        timer += Time.deltaTime;
        
        Vector3 npcPosition = GameObject.transform.position;
        Vector3 playerPosition = Player.Value.transform.position;

        Vector3 direction = npcPosition - playerPosition;
        direction.y = 0; 

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Player.Value.transform.rotation = Quaternion.Slerp(Player.Value.transform.rotation, targetRotation, Time.deltaTime * 5f);
        }

        
        if (step == 0)
        {
            if (textUI != null) textUI.text = "Hola, te veo perdido";
        }
        else
        {
            if (textUI != null) textUI.text = "Sígueme, te enseño la salida";
        }

        if (timer > 3.0f)
        {
            timer = 0;
            step++;
        }

        return step > 1 ? Status.Success : Status.Running;
    }

    protected override void OnEnd()
    {
        if (Talk.Value != null)
        {
            Talk.Value.SetActive(false);
        }
    }
}