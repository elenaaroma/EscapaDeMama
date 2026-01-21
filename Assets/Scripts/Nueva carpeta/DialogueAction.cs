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

    private TextMeshProUGUI _textUI;
    private float _timer;
    private int _step;

    protected override Status OnStart()
    {
        if (Talk.Value == null || Player.Value == null) return Status.Failure;

        Talk.Value.SetActive(true);
        _textUI = Talk.Value.GetComponentInChildren<TextMeshProUGUI>();
        
        _step = 0;
        _timer = 0;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        _timer += Time.deltaTime;

        // CORRECCIÓN AQUÍ: Usamos GameObject.transform para obtener la posición del NPC
        Vector3 npcPosition = GameObject.transform.position;
        Vector3 playerPosition = Player.Value.transform.position;

        Vector3 direction = npcPosition - playerPosition;
        direction.y = 0; // Evitamos que el jugador rote hacia arriba/abajo

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Player.Value.transform.rotation = Quaternion.Slerp(Player.Value.transform.rotation, targetRotation, Time.deltaTime * 5f);
        }

        // Lógica de frases
        if (_step == 0)
        {
            if (_textUI != null) _textUI.text = "Hola, te veo perdido.";
        }
        else
        {
            if (_textUI != null) _textUI.text = "Sígueme, te enseño la salida.";
        }

        if (_timer > 3.0f)
        {
            _timer = 0;
            _step++;
        }

        return _step > 1 ? Status.Success : Status.Running;
    }

    protected override void OnEnd()
    {
        if (Talk.Value != null) Talk.Value.SetActive(false);
    }
}