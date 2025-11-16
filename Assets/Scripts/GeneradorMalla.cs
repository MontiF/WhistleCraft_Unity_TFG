using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public static class GeneradorMalla
{
    public static void GenerarCaraFrontal(Chunk chunk, Vector3 posicion)
    {
        int ultimoVertice = chunk.vertices.Count;

        chunk.vertices.Add(posicion + new Vector3(0, 0, 1));
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
        int ultimoVertice = chunk.vertices.Count;

        chunk.vertices.Add(posicion + new Vector3(1, 0, 0));
        chunk.vertices.Add(posicion + new Vector3(1, 1, 0));
        chunk.vertices.Add(posicion + new Vector3(1, 1, 1));
        chunk.vertices.Add(posicion + new Vector3(1, 0, 1));


        chunk.triangulos.Add(ultimoVertice);
        chunk.triangulos.Add(ultimoVertice + 1);
        chunk.triangulos.Add(ultimoVertice + 2);
        chunk.triangulos.Add(ultimoVertice + 2);
        chunk.triangulos.Add(ultimoVertice + 3);
        chunk.triangulos.Add(ultimoVertice);
    }

    public static void GenerarCaraDerecha(Chunk chunk, Vector3 posicion)
    {
        int ultimoVertice = chunk.vertices.Count;

        chunk.vertices.Add(posicion);
        chunk.vertices.Add(posicion + new Vector3(0, 1, 0));
        chunk.vertices.Add(posicion + new Vector3(0, 1, 1));
        chunk.vertices.Add(posicion + new Vector3(0, 0, 1));


        chunk.triangulos.Add(ultimoVertice + 2);
        chunk.triangulos.Add(ultimoVertice + 1);
        chunk.triangulos.Add(ultimoVertice);
        chunk.triangulos.Add(ultimoVertice);
        chunk.triangulos.Add(ultimoVertice + 3);
        chunk.triangulos.Add(ultimoVertice + 2);
    }

    public static void GenerarCaraAbajo(Chunk chunk, Vector3 posicion)
    {
        int ultimoVertice = chunk.vertices.Count;

        chunk.vertices.Add(posicion + new Vector3(1, 1, 0));
        chunk.vertices.Add(posicion + new Vector3(1, 1, 1));
        chunk.vertices.Add(posicion + new Vector3(0, 1, 1));
        chunk.vertices.Add(posicion + new Vector3(0, 1, 0));


        chunk.triangulos.Add(ultimoVertice + 2);
        chunk.triangulos.Add(ultimoVertice + 1);
        chunk.triangulos.Add(ultimoVertice);
        chunk.triangulos.Add(ultimoVertice);
        chunk.triangulos.Add(ultimoVertice + 3);
        chunk.triangulos.Add(ultimoVertice + 2);
    }

    public static void GenerarCaraArriba(Chunk chunk, Vector3 posicion)
    {
        int ultimoVertice = chunk.vertices.Count;

        chunk.vertices.Add(posicion);
        chunk.vertices.Add(posicion + new Vector3(0, 0, 1));
        chunk.vertices.Add(posicion + new Vector3(1, 0, 1));
        chunk.vertices.Add(posicion + new Vector3(1, 0, 0));


        chunk.triangulos.Add(ultimoVertice + 2);
        chunk.triangulos.Add(ultimoVertice + 1);
        chunk.triangulos.Add(ultimoVertice);
        chunk.triangulos.Add(ultimoVertice);
        chunk.triangulos.Add(ultimoVertice + 3);
        chunk.triangulos.Add(ultimoVertice + 2);
    }

    public static void AplicarMallas(Chunk chunk)
    {
        // 1. Crear una NUEVA instancia de malla
        Mesh malla = new Mesh();

        // 2. Asignar los datos a la nueva malla
        malla.vertices = chunk.vertices.ToArray();
        malla.triangles = chunk.triangulos.ToArray();

        // 3. ¡Importante! Recalcular las normales para que la luz funcione
        malla.RecalculateNormals();

        // 4. Asignar la malla completa al MeshFilter
        chunk.filtradorMalla.mesh = malla;
    }
}
