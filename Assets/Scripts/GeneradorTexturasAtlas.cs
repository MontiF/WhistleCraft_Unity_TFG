using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GeneradorTexturasAtlas
{
    public static int tamanoTexura = 16;
    public static Texture2D textureAtlas;
    

    public static void Generate(Mundo mundo){
        textureAtlas = new Texture2D(3 * tamanoTexura, mundo.DatosBloques.Count * tamanoTexura);

        for (int i = 0; i < Mundo.main.DatosBloques.Count; i++)
        {
            AddTextura(mundo.DatosBloques[i].texturaLateral, 0, i);
            AddTextura(mundo.DatosBloques[i].texturaArriba, 1, i);
            AddTextura(mundo.DatosBloques[i].texturaAbajo, 2, i);
        }

        textureAtlas.Apply();
    }

    public static void AddTextura(Texture2D textura, int cara, int indice){
        for(int x = 0; x < tamanoTexura; x++){
            for(int y = 0; y < tamanoTexura; y++){
                textureAtlas.SetPixel(x + (cara * tamanoTexura), y + (indice * tamanoTexura), textura.GetPixel(x, y));
            }
        }
    }
}
