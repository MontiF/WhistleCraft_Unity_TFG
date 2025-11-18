using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.U2D;

public static class GeneradorMalla
{
    private static void AsignarUVs(Chunk chunk, TipoBloque tipo, string sufijo)
    {
        string nombreBase = tipo.ToString();          // Ej: "HIERBA"
        string nombreConSufijo = nombreBase + sufijo; // Ej: "HIERBA_TOP"

        Sprite s = null;

        if (chunk.atlas != null)
        {
            // 1. Intentamos buscar la versión específica (Ej: HIERBA_TOP)
            s = chunk.atlas.GetSprite(nombreConSufijo);

            // 2. Si no existe (es null), buscamos la versión normal (Ej: TIERRA)
            // Esto sirve para bloques que son iguales por todos lados.
            if (s == null)
            {
                s = chunk.atlas.GetSprite(nombreBase);
            }
        }

        if (s != null)
        {
            chunk.uvs.AddRange(s.uv);
        }
        else
        {
            // Si falla todo, coordenadas vacías para evitar errores
            // Debug.LogWarning("Falta textura para: " + nombreConSufijo);
            chunk.uvs.Add(Vector2.zero);
            chunk.uvs.Add(Vector2.zero);
            chunk.uvs.Add(Vector2.zero);
            chunk.uvs.Add(Vector2.zero);
        }
    }
    
    public static void GenerarCaraFrontal(Chunk chunk, Vector3 posicion, TipoBloque tipo)
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

        AsignarUVs(chunk, tipo, "_LADO");
    }

    public static void GenerarCaraTrasera(Chunk chunk, Vector3 posicion, TipoBloque tipo)
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

        AsignarUVs(chunk, tipo, "_LADO");
    }

    public static void GenerarCaraIzquierda(Chunk chunk, Vector3 posicion, TipoBloque tipo)
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

    AsignarUVs(chunk, tipo, "_LADO");
}

    public static void GenerarCaraDerecha(Chunk chunk, Vector3 posicion, TipoBloque tipo)
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

    AsignarUVs(chunk, tipo, "_LADO");
}

    public static void GenerarCaraArriba(Chunk chunk, Vector3 posicion, TipoBloque tipo)
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

        AsignarUVs(chunk, tipo, "_TOP");

    }

    // Genera la cara que mira hacia ABAJO (Y negativa)
    public static void GenerarCaraAbajo(Chunk chunk, Vector3 posicion, TipoBloque tipo)
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

        AsignarUVs(chunk, tipo, "");

    }

    public static void AplicarMallas(Chunk chunk)
    {
        // 1. Crear una NUEVA instancia de malla
        Mesh malla = new Mesh();

        // 2. Asignar los datos a la nueva malla
        malla.vertices = chunk.vertices.ToArray();
        malla.triangles = chunk.triangulos.ToArray();

        // 3. Asignar las coordenadas UV
        malla.uv = chunk.uvs.ToArray();
        
        // 4. Recalcular las normales para que la luz funcione
        malla.RecalculateNormals();

        // 5. Asignar la malla completa al MeshFilter
        chunk.filtradorMalla.mesh = malla;

        chunk.colisionadorMalla.sharedMesh = malla;
    }
}