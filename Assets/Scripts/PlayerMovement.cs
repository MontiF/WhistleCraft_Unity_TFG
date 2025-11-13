using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{
    public float velocidad = 1.0f;
    public float velocidadCamara = 1.0f;
    private float xRotation = 0f;
    public Transform cameraTransform;

    public float fuerzaSalto = 1.0f;

    private bool tocandoSuelo;
    public Transform groundCheck; 
    public float groundDistance = 0.1f; 
    public LayerMask groundMask;

    private Rigidbody fisicas;

    public float sprint = 1.0f;

    // Se llama al principio de la ejecuion del objeto
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        fisicas = GetComponent<Rigidbody>();
    }

    // Se llama con cada Frame
    void Update()
    {
        //Movimiento del jugador (w a s d)
        float horizontal = Input.GetAxis("Horizontal"); // a = +1  d = -1 
        float vertical = Input.GetAxis("Vertical"); // w = +1   s = -1

        transform.Translate(new Vector3(horizontal, 0.0f, vertical) * Time.deltaTime * velocidad); //delta time calcula hace cuanto se ha movido por ultima vez para que no vaya tan rapido y se multiplica por la velocidad que queramos.
        
        //Movimiento de la camara (raton)
        float rotationY = Input.GetAxis("Mouse X");
        float rotationX = Input.GetAxis("Mouse Y");

        transform.Rotate(new Vector3(0, rotationY * Time.deltaTime * velocidadCamara, 0));
        xRotation -= rotationX;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //impide dar la vuelat entera        

        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
        
        //Salto
        tocandoSuelo = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(Input.GetKeyDown(KeyCode.Space) && tocandoSuelo){
            fisicas.AddForce(new Vector3(0,fuerzaSalto,0), ForceMode.Impulse);
        }

        if(Input.GetKey(KeyCode.LeftShift)){
        transform.Translate(new Vector3(horizontal, 0.0f, vertical) * Time.deltaTime * velocidad * sprint);      
    }
    }
}
