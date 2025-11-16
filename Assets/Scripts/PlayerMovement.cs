using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement1 : MonoBehaviour {
    public float velocidad = 1.0f;
    public float velocidadCamara = 1.0f;
    private float yRotation = 0f;
    public Transform cameraTransform;

    public float fuerzaSalto = 1.0f;

    private bool tocandoSuelo;
    public Transform groundCheck; 
    public float groundDistance = 0.1f; 
    public LayerMask groundMask;

    private Rigidbody fisicas;

    public float correr = 1.0f;

    public Transform cabezaSteve;
    
    public bool primeraEjecucion = true;
    // Se llama al principio de la ejecuion del objeto
    void Start(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        fisicas = GetComponent<Rigidbody>();
    }

    // Se llama con cada Frame
    void Update() {
        if (primeraEjecucion)
        {
            // Resetear la bandera después del primer frame de Update
            primeraEjecucion = false; 
            return; // Salta el resto del Update() en este frame.
        }
        //Movimiento del jugador (w a s d)
        float horizontal = Input.GetAxis("Horizontal"); // a = +1 d = -1 
        float vertical = Input.GetAxis("Vertical"); // w = +1 s = -1

        transform.Translate(new Vector3(horizontal, 0.0f, vertical) * Time.deltaTime * velocidad); //delta time calcula hace cuanto se ha movido por ultima vez para que no vaya tan rapido y se multiplica por la velocidad que queramos.
    
        //Movimiento de la camara (raton)
        float rotationX = Input.GetAxis("Mouse X");
        float rotationY = Input.GetAxis("Mouse Y");

        transform.Rotate(new Vector3(0, rotationX * Time.deltaTime * velocidadCamara, 0));
        
        yRotation -= rotationY * Time.deltaTime * velocidadCamara; 
    
        yRotation = Mathf.Clamp(yRotation, -90f, 90f); //impide dar la vuelta entera 

        if (cameraTransform != null) {
            cameraTransform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
        }

        if (cabezaSteve != null){
            // Solo aplicamos la rotación vertical (yRotation)
            cabezaSteve.localRotation = Quaternion.Euler(0f, 0f, yRotation);
        }

        //Salto
        tocandoSuelo = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(Input.GetKeyDown(KeyCode.Space) && tocandoSuelo){
            fisicas.AddForce(new Vector3(0,fuerzaSalto,0), ForceMode.Impulse);
        }

        //Correr
        if(Input.GetKey(KeyCode.LeftShift)){
            transform.Translate(new Vector3(horizontal, 0.0f, vertical) * Time.deltaTime * velocidad * correr); 
        }
    }
}
