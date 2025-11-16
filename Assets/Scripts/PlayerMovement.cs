using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Asegurarse de que el Rigidbody siempre exista
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement1 : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 7.0f;
    public float correr = 1.5f; // Esto ahora es un multiplicador (correr = 1.5x de velocidad)
    
    [Header("Salto")]
    public float fuerzaSalto = 10.0f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool tocandoSuelo;
    
    [Header("Cámara")]
    public float velocidadCamara = 400.0f;
    public Transform cameraTransform;
    public Transform cabezaSteve;
    private float yRotation = 0f;

    // Variables privadas
    private Rigidbody fisicas;
    private Vector3 moveInput; // Almacenará el input de WASD
    private bool jumpPressed;  // Almacenará si se ha pulsado salto

    // Awake se usa para inicializar componentes (es más seguro que Start)
    void Awake()
    {
        fisicas = GetComponent<Rigidbody>();
        
        // Bloquear el cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // NOTA: He quitado la lógica de "primeraEjecucion", 
        // ya que al mover la física a FixedUpdate no suele ser necesaria.
    }

    // Update se llama en cada frame. Ideal para capturar inputs.
    void Update()
    {
        // --- CAPTURA DE INPUT DE MOVIMIENTO ---
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        // Comprobar si está corriendo
        bool estaCorriendo = Input.GetKey(KeyCode.LeftShift);
        float multiplicadorVelocidad = estaCorriendo ? correr : 1.0f;

        // Almacenamos el input para usarlo en FixedUpdate
        // Lo multiplicamos por la velocidad aquí
        moveInput = new Vector3(horizontal, 0.0f, vertical) * velocidad * multiplicadorVelocidad;
        moveInput = Vector3.ClampMagnitude(moveInput, velocidad * multiplicadorVelocidad); // Evita ir más rápido en diagonal

        // --- CAPTURA DE INPUT DE SALTO ---
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpPressed = true;
        }

        // --- MOVIMIENTO DE LA CÁMARA (RATÓN) ---
        // Esto puede quedarse en Update porque no es física
        float rotationX = Input.GetAxis("Mouse X") * Time.deltaTime * velocidadCamara;
        float rotationY = Input.GetAxis("Mouse Y") * Time.deltaTime * velocidadCamara;

        // Rotar el cuerpo del jugador (horizontalmente)
        // OJO: Le pedimos al Rigidbody que rote, no al transform
        fisicas.MoveRotation(fisicas.rotation * Quaternion.Euler(new Vector3(0, rotationX, 0)));
        
        // Rotar la cámara (verticalmente)
        yRotation -= rotationY;
        yRotation = Mathf.Clamp(yRotation, -90f, 90f); //impide dar la vuelta entera 

        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
        }

        if (cabezaSteve != null)
        {
            // Solo aplicamos la rotación vertical (yRotation)
            // (Asegúrate de que el eje 'Z' es el correcto para la cabeza de Steve)
            cabezaSteve.localRotation = Quaternion.Euler(0f, 0f, yRotation); 
            // Si la cabeza rota mal, prueba: Quaternion.Euler(0f, 0f, yRotation) o Quaternion.Euler(yRotation, 0f, 0f)
        }
    }

    // FixedUpdate se llama en un ritmo fijo. Ideal para FÍSICA.
    void FixedUpdate()
    {
        // --- APLICAR MOVIMIENTO ---
        
        // 1. Convertir el input (que es local) a dirección del mundo
        // Hacia donde está mirando el jugador
        Vector3 moveDirection = transform.TransformDirection(moveInput);

        // 2. Aplicar la velocidad al Rigidbody
        // Mantenemos la velocidad vertical (gravedad/salto) que ya tuviera
        fisicas.linearVelocity = new Vector3(moveDirection.x, fisicas.linearVelocity.y, moveDirection.z);

        // --- APLICAR SALTO ---

        // Comprobamos el suelo en FixedUpdate (es una consulta de física)
        tocandoSuelo = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (jumpPressed && tocandoSuelo)
        {
            fisicas.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        }
        
        // Reseteamos el flag de salto
        jumpPressed = false;
    }
}