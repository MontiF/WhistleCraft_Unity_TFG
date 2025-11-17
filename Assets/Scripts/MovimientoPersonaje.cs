using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Se asegura que el Rigidbody siempre exista el personaje
[RequireComponent(typeof(Rigidbody))]
public class MovimientoPersonaje : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 7.0f;
    public float correr = 1.5f; //multiplicador de velocidad para correr
    public float andar = 1.0f; //multiplicador de velocidad para andar
    float multiplicadorVelocidad = 1.0f;
    
    [Header("Salto")]
    public float fuerzaSalto = 10.0f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool tocandoSuelo;
    
    [Header("Camara")]
    public float velocidadCamara = 400.0f;
    public Transform cameraTransform;
    public Transform cabezaSteve;
    private float yRotation = 0f;

    [Header("Agacharse")]
    public float velocidadAgachado = 0.5f;
    public float alturaCamaraAgachado = 0.75f; // Multiplicador de altura de la camara al agacharse
    public Transform torsoSteve;
    private bool agachado; // Almacenara si esta agachado o no

    private Rigidbody fisicas;
    private Vector3 inputMovimiento; // Almacenara el input de WASD
    private Vector3 posicionOriginalCamara;
    private bool saltoPresionado;  // Almacenara si se ha pulsado salto 

    // Awake se usa para inicializar componentes antes de iniciar el programa
    void Awake()
    {
        fisicas = GetComponent<Rigidbody>();
        
        // Bloquear el cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (cameraTransform != null)
        {
            posicionOriginalCamara = cameraTransform.localPosition;
        }

    }

    // Update se llama en cada frame. Ideal para capturar inputs
    void Update()
    {
        // Input de movimiento (WASD)
        float horizontal = Input.GetAxis("Horizontal"); // d+1, a-1
        float vertical = Input.GetAxis("Vertical"); // w+1, s-1
        
        // Comprobar si esta corriendo
        if(Input.GetKey(KeyCode.LeftShift) && !agachado){
            multiplicadorVelocidad = correr;
        }else if(!agachado){
            multiplicadorVelocidad = andar;
        }else{
            multiplicadorVelocidad = velocidadAgachado;
        }

        // Almacenamos el input para usarlo en FixedUpdate
        // Lo multiplicamos por la velocidad aqui
        inputMovimiento = new Vector3(horizontal, 0.0f, vertical) * velocidad * multiplicadorVelocidad;
        inputMovimiento = Vector3.ClampMagnitude(inputMovimiento, velocidad * multiplicadorVelocidad); // Evita ir mas rapido en diagonal

        // Input de Salto (espacio)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            saltoPresionado = true;
        }

        // Movimiento Camara (Raton)
        float rotationX = Input.GetAxis("Mouse X") * Time.deltaTime * velocidadCamara;
        float rotationY = Input.GetAxis("Mouse Y") * Time.deltaTime * velocidadCamara;

        // Rotar el cuerpo del jugador (horizontalmente)
        fisicas.MoveRotation(fisicas.rotation * Quaternion.Euler(new Vector3(0, rotationX, 0)));
        
        // Rotar la camara (verticalmente)
        yRotation -= rotationY;
        yRotation = Mathf.Clamp(yRotation, -90f, 90f); //impide dar la vuelta entera 

        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
        }

        if (cabezaSteve != null)
        {
            // Solo aplicamos la rotacion vertical (yRotation)
            cabezaSteve.localRotation = Quaternion.Euler(0f, 0f, yRotation); 
        }

        // Input de Agacharse (Ctrl izquierdo)
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (torsoSteve != null){
                torsoSteve.localRotation = Quaternion.Euler(0f, 90f, 125f);
            }
            if (cameraTransform != null)
            {
                cameraTransform.localPosition = new Vector3(posicionOriginalCamara.x, posicionOriginalCamara.y * alturaCamaraAgachado, posicionOriginalCamara.z);
            }
            agachado = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            if (torsoSteve != null){
                torsoSteve.localRotation = Quaternion.Euler(0f, 90f, 90f);
            }
            if (cameraTransform != null)
            {
                cameraTransform.localPosition = posicionOriginalCamara;
            }
            agachado = false;
        }

    }

    // FixedUpdate se llama en un ritmo fijo. Ideal para fisicas.
    void FixedUpdate()
    {
        // Convertir el input a direccion del mundo
        // Hacia donde esta mirando el jugador
        Vector3 direccionMovimiento = transform.TransformDirection(inputMovimiento);

        // Aplicar la velocidad al Rigidbody
        // Mantenemos la velocidad vertical (gravedad/salto)
        fisicas.linearVelocity  = new Vector3(direccionMovimiento.x, fisicas.linearVelocity.y, direccionMovimiento.z);

        // Aplicar salto
        tocandoSuelo = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (saltoPresionado && tocandoSuelo)
        {
            fisicas.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        }
        
        // Reseteamos la flag de salto
        saltoPresionado = false;
    }
}