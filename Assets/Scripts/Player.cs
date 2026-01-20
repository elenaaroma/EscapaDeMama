using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float gravityScale = -9.81f;

    [Header("Cámara")]
    [SerializeField] private Transform cameraTransform; // Arrastra aquí tu Main Camera
    [SerializeField] private float sensitivity = 0.1f;
    private float xRotation = 0f;

    private CharacterController controller;
    private Vector2 movementInput;
    private Vector2 mouseInput;
    private Vector3 verticalMovement;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Bloquea el ratón en el centro
    }

    // Recibe movimiento (WASD)
    public void OnMove(InputValue value) => movementInput = value.Get<Vector2>();

    // Recibe movimiento del ratón (Look)
    public void OnLook(InputValue value) => mouseInput = value.Get<Vector2>();

    void Update()
    {
        ManejarRotacion();
        ManejarMovimiento();
    }

    private void ManejarRotacion()
    {
        // 1. Girar el cuerpo a los lados (Eje Y)
        transform.Rotate(Vector3.up * mouseInput.x * sensitivity);

        // 2. Girar la cámara arriba y abajo (Eje X)
        xRotation -= mouseInput.y * sensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Evita que la cámara dé la vuelta completa

        if (cameraTransform != null)
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void ManejarMovimiento()
    {
        // Movimiento Horizontal
        Vector3 move = transform.right * movementInput.x + transform.forward * movementInput.y;
        controller.Move(move * movementSpeed * Time.deltaTime);

        // Gravedad
        if (controller.isGrounded && verticalMovement.y < 0)
            verticalMovement.y = -1f;
        else
            verticalMovement.y += gravityScale * Time.deltaTime;

        controller.Move(verticalMovement * Time.deltaTime);
    }
}