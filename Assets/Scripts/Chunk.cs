using UnityEngine;
using System.Collections.Generic; // Esta librería nos ofrece recursos como las List<>
using System.Collections;
public class Chunk : MonoBehaviour
{

    //Creamos un filtrado de malla que nos ayudará a ver la malla por el lado que nos interesa
    public MeshFilter filtradorMalla;

    //Creamos dos listas de tipo Vector3 para los vectores en R3 y otra para los triángulos que formarán las caras del cuadrado
    public List<Vector3> vertices = new List<Vector3>();
    public List<int> triangulos = new List<int>();

    //Esta función sirve para que, antes de que el programa inicie con `Start()`, primero busque su malla.
    private void Awake()
    {
        filtradorMalla = GetComponent<MeshFilter>();
    }

    //Creamos las mallas a partir de los métodos de la clase GeneradorMalla con el chunk correspondiente (this) y sus coordenadas (Vector3.zero)
    private void Start()
    {
        GeneradorMalla.GenerarCaraFrontal(this, Vector3.zero);
        GeneradorMalla.GenerarCaraTrasera(this, Vector3.zero);

        GeneradorMalla.GenerarCaraDerecha(this, Vector3.zero);
        GeneradorMalla.GenerarCaraIzquierda(this, Vector3.zero);

        GeneradorMalla.GenerarCaraAbajo(this, Vector3.zero);
        GeneradorMalla.GenerarCaraArriba(this, Vector3.zero);

        GeneradorMalla.AplicarMallas(this);
    }
}
