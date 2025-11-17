using UnityEngine;
using UnityEngine.UI;

public class HotbarManager : MonoBehaviour
{
    public RectTransform selector; 
    
    // El ancho de un slot + el espacio entre ellos
    public float anchoTotalSlot = 101f; 

    private int slotSeleccionado = 0; // Donde se encuentra el primer slot

    void Update()
    {
        //Detectar Rueda del Raton
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f) // Rueda hacia arriba
        {
            slotSeleccionado--;
        }
        else if (scroll < 0f) // Rueda hacia abajo
        {
            slotSeleccionado++;
        }

        // Detectar Teclas Numericas (1-9)
        if (Input.GetKeyDown(KeyCode.Alpha1)) slotSeleccionado = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) slotSeleccionado = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) slotSeleccionado = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4)) slotSeleccionado = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5)) slotSeleccionado = 4;
        if (Input.GetKeyDown(KeyCode.Alpha6)) slotSeleccionado = 5;
        if (Input.GetKeyDown(KeyCode.Alpha7)) slotSeleccionado = 6;
        if (Input.GetKeyDown(KeyCode.Alpha8)) slotSeleccionado = 7;
        if (Input.GetKeyDown(KeyCode.Alpha9)) slotSeleccionado = 8;


        // Asegurarse de que el slot estee en el rango (0-8)
        // Esto hace que la rueda del raton sea ciclica
        if (slotSeleccionado > 8) slotSeleccionado = 0;
        if (slotSeleccionado < 0) slotSeleccionado = 8;
        
        // Mover el Selector
        // Calculamos la nueva posicion X
        float nuevaPosX = slotSeleccionado * anchoTotalSlot; 

        // selector.anchoredPosition es la posicion relativa dentro del contenedor
        Vector3 posicionActual = selector.anchoredPosition;

        // Usamos Lerp para un movimiento mas suave 
        selector.anchoredPosition = Vector3.Lerp(
            posicionActual, 
            new Vector3(nuevaPosX, posicionActual.y, 0), // Mantenemos la Y por si acaso
            Time.deltaTime * 20f // Velocidad del movimiento
        );
    }
}
