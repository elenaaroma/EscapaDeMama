using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float velocidad = 5f;
    [SerializeField] private float gravedad = -9.81f;
    [SerializeField] private float sensibilidad = 0.1f;
    [SerializeField] private Camera camaraOjos;
  
    private CharacterController controller;
    private Vector2 inputMovimiento; 
    private Vector2 inputRaton; 
    private Vector3 caida;
    private float rotacionVertical = 0f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (camaraOjos == null) camaraOjos = Camera.main;
        
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    public void OnMove(InputValue value)
    {
        inputMovimiento = value.Get<Vector2>();
    }

    // Debes tener una acción "Look" en tu Input Action Asset
    public void OnLook(InputValue value)
    {
        inputRaton = value.Get<Vector2>();
    }

    void Update()
    {
        RotarCamara(); // Primero rotamos para saber hacia dónde mirar
        ManejarMovimiento();
        AplicarGravedad();
    }

    private void RotarCamara()
    {
        // 1. Girar el cuerpo a los lados (Eje Y) con el movimiento X del ratón
        transform.Rotate(Vector3.up * inputRaton.x * sensibilidad);

        // 2. Girar la cámara arriba/abajo (Eje X) con el movimiento Y del ratón
        rotacionVertical -= inputRaton.y * sensibilidad;
        rotacionVertical = Mathf.Clamp(rotacionVertical, -90f, 90f); // Límite para no desnucarse
        
        camaraOjos.transform.localRotation = Quaternion.Euler(rotacionVertical, 0, 0);
    }

    private void ManejarMovimiento()
    {
        // Convertimos el input en dirección relativa al personaje
        // transform.forward es "hacia adelante" según la rotación del cuerpo
        Vector3 movimiento = transform.right * inputMovimiento.x + transform.forward * inputMovimiento.y;
        
        controller.Move(movimiento * velocidad * Time.deltaTime);
    }
    
    private void AplicarGravedad()
    {
        if (controller.isGrounded && caida.y < 0)
        {
            caida.y = -2f;
        }
        caida.y += gravedad * Time.deltaTime;
        controller.Move(caida * Time.deltaTime);
    }
}