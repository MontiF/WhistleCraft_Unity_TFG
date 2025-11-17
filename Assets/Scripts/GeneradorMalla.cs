using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public static class GeneradorMalla
{
    public static void GenerarCaraFrontal(Chunk chunk, Vector3 posicion)
    {
        int ultimoVertice = chunk.vertices.Count;

        chunk.vertices.Add(posicion + new Vector3(0, 0, 1)); // z = 1
        chunk.vertices.Add(posicion + new Vector3(0, 1, 1));
        chunk.vertices.Add(posicion + new Vector3(1, 1, 1));
        chunk.vertices.Add(posicion + new Vector3(1, 0, 1));

        chunk.triangulos.Add(ultimoVertice + 2);
        chunk.triangulos.Add(ultimoVertice + 1);
        chunk.triangulos.Add(ultimoVertice);

        chunk.triangulos.Add(ultimoVertice);
        chunk.triangulos.Add(ultimoVertice + 3);
        chunk.triangulos.Add(ultimoVertice + 2);
    }

    public static void GenerarCaraTrasera(Chunk chunk, Vector3 posicion)
    {
        int ultimoVertice = chunk.vertices.Count;

        chunk.vertices.Add(posicion);
        chunk.vertices.Add(posicion + new Vector3(0, 1, 0));
        chunk.vertices.Add(posicion + new Vector3(1, 1, 0));
        chunk.vertices.Add(posicion + new Vector3(1, 0, 0));

        chunk.triangulos.Add(ultimoVertice);
        chunk.triangulos.Add(ultimoVertice + 1);
        chunk.triangulos.Add(ultimoVertice + 2);

        chunk.triangulos.Add(ultimoVertice + 2);
        chunk.triangulos.Add(ultimoVertice + 3);
        chunk.triangulos.Add(ultimoVertice);
    }

    public static void GenerarCaraIzquierda(Chunk chunk, Vector3 posicion)
{
    // Esta es la cara que mira hacia X = 0
    int ultimoVertice = chunk.vertices.Count;

    // Vértices en el plano X = 0
    chunk.vertices.Add(posicion + new Vector3(0, 0, 0)); // V0 (esquina abajo, atrás)
    chunk.vertices.Add(posicion + new Vector3(0, 1, 0)); // V1 (esquina arriba, atrás)
    chunk.vertices.Add(posicion + new Vector3(0, 1, 1)); // V2 (esquina arriba, adelante)
    chunk.vertices.Add(posicion + new Vector3(0, 0, 1)); // V3 (esquina abajo, adelante)


    // Triángulos: Aseguran que la normal apunte hacia AFUERA (X-)
    // El orden de los triángulos debe ser opuesto al de la Cara Derecha
    chunk.triangulos.Add(ultimoVertice + 0);
    chunk.triangulos.Add(ultimoVertice + 2);
    chunk.triangulos.Add(ultimoVertice + 1);

    chunk.triangulos.Add(ultimoVertice + 0);
    chunk.triangulos.Add(ultimoVertice + 3);
    chunk.triangulos.Add(ultimoVertice + 2);
}

    public static void GenerarCaraDerecha(Chunk chunk, Vector3 posicion)
{
    // Esta es la cara que mira hacia X = +1
    int ultimoVertice = chunk.vertices.Count;

    // Vértices en el plano X = 1
    chunk.vertices.Add(posicion + new Vector3(1, 0, 0)); // V0 (esquina abajo, atrás)
    chunk.vertices.Add(posicion + new Vector3(1, 1, 0)); // V1 (esquina arriba, atrás)
    chunk.vertices.Add(posicion + new Vector3(1, 1, 1)); // V2 (esquina arriba, adelante)
    chunk.vertices.Add(posicion + new Vector3(1, 0, 1)); // V3 (esquina abajo, adelante)

    // Triángulos: Aseguran que la normal apunte hacia AFUERA (X+)
    chunk.triangulos.Add(ultimoVertice + 0);
    chunk.triangulos.Add(ultimoVertice + 1);
    chunk.triangulos.Add(ultimoVertice + 2);

    chunk.triangulos.Add(ultimoVertice + 0);
    chunk.triangulos.Add(ultimoVertice + 2);
    chunk.triangulos.Add(ultimoVertice + 3);
}

    public static void GenerarCaraArriba(Chunk chunk, Vector3 posicion)
    {
        int ultimoVertice = chunk.vertices.Count;
        
        // Vértices en el plano Y=1
        chunk.vertices.Add(posicion + new Vector3(0, 1, 0)); // V0
        chunk.vertices.Add(posicion + new Vector3(0, 1, 1)); // V1
        chunk.vertices.Add(posicion + new Vector3(1, 1, 1)); // V2
        chunk.vertices.Add(posicion + new Vector3(1, 1, 0)); // V3

        chunk.triangulos.Add(ultimoVertice + 0);
        chunk.triangulos.Add(ultimoVertice + 2);
        chunk.triangulos.Add(ultimoVertice + 3);

        chunk.triangulos.Add(ultimoVertice + 0);
        chunk.triangulos.Add(ultimoVertice + 1);
        chunk.triangulos.Add(ultimoVertice + 2);

    }

    // Genera la cara que mira hacia ABAJO (Y negativa)
    public static void GenerarCaraAbajo(Chunk chunk, Vector3 posicion)
    {
        int ultimoVertice = chunk.vertices.Count;

        // Vértices en el plano Y=0
        chunk.vertices.Add(posicion + new Vector3(0, 0, 0)); // V0
        chunk.vertices.Add(posicion + new Vector3(1, 0, 0)); // V1
        chunk.vertices.Add(posicion + new Vector3(1, 0, 1)); // V2
        chunk.vertices.Add(posicion + new Vector3(0, 0, 1)); // V3

        chunk.triangulos.Add(ultimoVertice + 0);
        chunk.triangulos.Add(ultimoVertice + 2);
        chunk.triangulos.Add(ultimoVertice + 3);

        chunk.triangulos.Add(ultimoVertice + 0);
        chunk.triangulos.Add(ultimoVertice + 1);
        chunk.triangulos.Add(ultimoVertice + 2);

    }

    public static void AplicarMallas(Chunk chunk)
    {
        // 1. Crear una NUEVA instancia de malla
        Mesh malla = new Mesh();

        // 2. Asignar los datos a la nueva malla
        malla.vertices = chunk.vertices.ToArray();
        malla.triangles = chunk.triangulos.ToArray();

        // 3.Recalcular las normales para que la luz funcione
        malla.RecalculateNormals();

        // 4. Asignar la malla completa al MeshFilter
        chunk.filtradorMalla.mesh = malla;

        chunk.colisionadorMalla.sharedMesh = malla;
    }
}