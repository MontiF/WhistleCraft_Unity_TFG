using UnityEngine;

public class InteraccionBloque : MonoBehaviour
{
    public float distanciaInteraccion = 5f;
    public Camera PrimeraPersona;

    void Start()
    {
        if (PrimeraPersona == null){
            PrimeraPersona = Camera.main;
        }
    }

    void Update()
    {
        // Detecta si se ha presionado el boton izquierdo del raton (0) para romper un bloque
        if (Input.GetMouseButtonDown(0)){
            ManejarInteraccion("romper");
        }

        // Detecta si se ha presionado el boton derecho del raton (1) para poner un bloque
        if (Input.GetMouseButtonDown(1)){
            ManejarInteraccion("poner");
        }
    }

    // Logica del Raycast
    void ManejarInteraccion(string accion)
    {
        Ray rayo = new Ray(PrimeraPersona.transform.position, PrimeraPersona.transform.forward);
        if (Physics.Raycast(rayo, out RaycastHit hit, distanciaInteraccion))
        {
            // Dependiendo de la accion, hacemos una cosa u otra.
            if (accion == "romper") {
                // Usamos Destroy() para eliminar el GameObject que el rayo ha golpeado
                // hit.transform.gameObject nos da la referencia al objeto impactado
                Destroy(hit.transform.gameObject);
            }
            else if (accion == "poner")
                // Usamos "" para crear el GameObject en donde a apuntuado el rayo
                Debug.Log("Intentando PONER un bloque cerca de: " + hit.transform.name);
        }
    }
}
