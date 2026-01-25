using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float velocidad = 5f;
    [SerializeField] private float gravedad = -9.81f;
    [SerializeField] private float sensibilidad = 0.1f;
    [SerializeField] private Camera camaraOjos;
    [SerializeField] private GameObject RespawnPoint;
    [SerializeField] private GameObject canvasVictoria;
  
    private CharacterController controller;
    private Vector2 inputMovimiento; 
    private Vector2 inputRaton; 
    private Vector3 caida;
    private float rotacionVertical = 0f;
    
    

    private void OnEnable()
    {
        RespawnAction.OnRespawn += Respawn;
    }
    
    private void OnDisable()
    {
        RespawnAction.OnRespawn -= Respawn;
    }

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
    
    public void OnLook(InputValue value)
    {
        inputRaton = value.Get<Vector2>();
    }

    void Update()
    {
        RotarCamara(); 
        ManejarMovimiento();
        AplicarGravedad();
    }

    private void RotarCamara()
    {
        transform.Rotate(Vector3.up * inputRaton.x * sensibilidad);
        
        rotacionVertical -= inputRaton.y * sensibilidad;
        rotacionVertical = Mathf.Clamp(rotacionVertical, -90f, 90f); 
        
        camaraOjos.transform.localRotation = Quaternion.Euler(rotacionVertical, 0, 0);
    }

    private void ManejarMovimiento()
    {
        
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

    private void Respawn()
    {
        controller.enabled = false;
        
        transform.position = RespawnPoint.transform.position;
        transform.rotation = RespawnPoint.transform.rotation;
        
        controller.enabled = true;
        
        Debug.Log("Respawn");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Puerta")
        {
            SceneManager.LoadScene("Victoria");
        }
    }
}