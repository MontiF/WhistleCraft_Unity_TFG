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
            // Obtenemos el script del Chunk del objeto golpeado
            Chunk chunk = hit.transform.GetComponent<Chunk>();
            if (chunk == null) 
            {
                return;
            }

            // Dependiendo de la accion, hacemos una cosa u otra
            if (accion == "romper") {
                // Calculamos la posicion del bloque a romper
                // Nos movemos un poco hacia adentro del bloque desde el punto de impacto
                Vector3 posicionBloque = hit.point - hit.normal * 0.5f;

                // Le pedimos al chunk que actualice el bloque en esa posicion
                chunk.ModificarBloque(Mathf.FloorToInt(posicionBloque.x), Mathf.FloorToInt(posicionBloque.y), Mathf.FloorToInt(posicionBloque.z), TipoBloque.AIRE);
            }
            else if (accion == "poner") {
                // Calculamos la posicion donde queremos poner el nuevo bloque
                // Nos movemos un poco hacia afuera desde el punto de impacto
                Vector3 posicionBloque = hit.point + hit.normal * 0.5f;

                // Le pedimos al chunk que actualice el bloque en esa posicion
                chunk.ModificarBloque(Mathf.FloorToInt(posicionBloque.x), Mathf.FloorToInt(posicionBloque.y), Mathf.FloorToInt(posicionBloque.z), TipoBloque.TIERRA);
            }
        }
    }
}
