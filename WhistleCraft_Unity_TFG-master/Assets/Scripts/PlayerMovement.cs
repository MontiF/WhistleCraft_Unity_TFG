using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{
    public float Speed = 1.0f;
    public float CameraSpeed = 1.0f;
    private float xRotation = 0f;
    public float JumpForce = 1.0f;
    private Rigidbody Physics;

    public Transform cameraTransform;
    
    // Se llama al principio de la ejecuion del objeto
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Se llama con cada Frame
    void Update()
    {
        //Movimiento del jugador (w a s d)
        float horizontal = Input.GetAxis("Horizontal"); // a = +1  d = -1 
        float vertical = Input.GetAxis("Vertical"); // w = +1   s = -1

        transform.Translate(new Vector3(horizontal, 0.0f, vertical) * Time.deltaTime * Speed); //delta time calcula hace cuanto se ha movido por ultima vez para que no vaya tan rapido y se multiplica por la velocidad que queramos.
        
        //Movimiento de la camara (raton)
        float rotationY = Input.GetAxis("Mouse X");
        float rotationX = Input.GetAxis("Mouse Y");

        transform.Rotate(new Vector3(0, rotationY * Time.deltaTime * CameraSpeed, 0));
        xRotation -= rotationX;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //impide dar la vuelat entera        

        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
        
        //Salto
        if(Input.GetKeyDown(KeyCode.Space)){
            Physics.AddForce(new Vector3(JumpForce,0,0), ForceMode.Impulse);
        }
    }
}
