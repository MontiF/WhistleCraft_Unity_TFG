using UnityEngine;

public class InteraccionBloque : MonoBehaviour
{
    public float distanciaInteraccion = 5f;
    public Camera PrimeraPersona;
    public Color colorResaltado = Color.grey;

    private Transform bloqueResaltado;
    private Color colorOriginal;
    private Renderer rendererBloqueResaltado;

    void Start()
    {
        if (PrimeraPersona == null){
            PrimeraPersona = Camera.main;
        }
    }

    void Update()
    {
        ManejarResaltado();
        ManejarEntradaUsuario();
    }

    void ManejarEntradaUsuario() {
        // Detecta si se ha presionado el boton izquierdo del raton (0) para romper un bloque
        if (Input.GetMouseButtonDown(0)){
            ManejarInteraccion("romper");
        }

        // Detecta si se ha presionado el boton derecho del raton (1) para poner un bloque
        if (Input.GetMouseButtonDown(1)){
            ManejarInteraccion("poner");
        }
    }

    void ManejarResaltado()
    {
        Ray rayo = new Ray(PrimeraPersona.transform.position, PrimeraPersona.transform.forward);

        // Si ya habia un bloque resaltado, le devolvemos su color original antes de hacer nada.
        if (bloqueResaltado != null)
        {
            rendererBloqueResaltado.material.color = colorOriginal;
            bloqueResaltado = null;
            rendererBloqueResaltado = null;
        }

        // Lanzamos un rayo para ver si estamos mirando un bloque.
        if (Physics.Raycast(rayo, out RaycastHit hit, distanciaInteraccion))
        {
            Transform seleccion = hit.transform;
            Renderer rendererSeleccion = seleccion.GetComponent<Renderer>();

            // Si el objeto tiene un componente Renderer, lo resaltamos.
            if (rendererSeleccion != null)
            {
                bloqueResaltado = seleccion;
                rendererBloqueResaltado = rendererSeleccion;
                colorOriginal = rendererSeleccion.material.color; // Guardamos el color original
                rendererSeleccion.material.color = colorResaltado; // Aplicamos el color de resaltado
            }
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
