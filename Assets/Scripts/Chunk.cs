using UnityEngine;
using System.Collections.Generic; // Esta librería nos ofrece recursos como las List<>
using System.Collections;
using UnityEngine.U2D;

public class Chunk : MonoBehaviour
{
    public static readonly int Anchura = 16;
    public static readonly int Altura = 128; // Reducido para pruebas
    public static readonly int Profundidad = 16;
    public TipoBloque[,,] bloques = new TipoBloque[Anchura, Altura, Profundidad];

    public SpriteAtlas atlas;

    //Creamos un filtrado de malla que nos ayudará a ver la malla por el lado que nos interesa
    public MeshFilter filtradorMalla;
    public MeshCollider colisionadorMalla;
    //Creamos dos listas de tipo Vector3 para los vectores en R3 y otra para los triángulos que formarán las caras del cuadrado
    public List<Vector3> vertices = new List<Vector3>();
    public List<int> triangulos = new List<int>();

    public List<Vector2> uvs = new List<Vector2>();

    //Esta función sirve para que, antes de que el programa inicie con `Start()`, primero busque su malla.
    private void Awake()
    {
        filtradorMalla = GetComponent<MeshFilter>();
        colisionadorMalla = GetComponent<MeshCollider>();
    }

    //Creamos las mallas a partir de los métodos de la clase GeneradorMalla con el chunk correspondiente (this) y sus coordenadas (Vector3.zero)
    private void Start()
    {
        if (atlas != null)
        {   
            // Para obtener la textura del atlas, es mejor no depender de un sprite específico.
            // Obtenemos el primer sprite del atlas para asegurarnos de que tenemos la textura correcta.
            Sprite[] sprites = new Sprite[atlas.spriteCount];
            atlas.GetSprites(sprites);

            if (sprites.Length > 0)
            {
                GetComponent<MeshRenderer>().sharedMaterial.mainTexture = sprites[0].texture;
            }
        }

        GenerarDatosVoxel(); 
        GenerarMallaOptimizada();
        GeneradorMalla.AplicarMallas(this);
    }
    void GenerarDatosVoxel(){
        for (int x = 0; x < Anchura; x++){
            for (int z = 0; z < Profundidad; z++){
                // Dejamos 32 bloques de aire por encima
                for (int y = 0; y < Altura; y++){
                    if(y == 32){
                        bloques[x, y, z] = TipoBloque.HIERBA;
                    }else if (y < 32 && y > 12){
                        bloques[x, y, z] = TipoBloque.TIERRA;
                    }else if(y <= 12){
                        bloques[x, y, z] = TipoBloque.PIEDRA;
                    }else{
                        bloques[x, y, z] = TipoBloque.AIRE;
                    }
                }
            }
        }
    }
    void GenerarMallaOptimizada(){
        vertices.Clear();
        triangulos.Clear();

        uvs.Clear();

        for (int x = 0; x < Anchura; x++){
            for (int y = 0; y < Altura; y++){
                for (int z = 0; z < Profundidad; z++){
                    if (bloques[x, y, z] != TipoBloque.AIRE){
                        // Si es un bloque sólido, chequeamos sus 6 caras
                        GenerarCarasBloque(x, y, z, bloques[x, y, z]);
                    }
                }
            }
        }
    }
    void GenerarCarasBloque(int x, int y, int z, TipoBloque tipo){
        Vector3 posicion = new Vector3(x, y, z);

        // Chequeo de las 6 caras (Hidden Face Culling)
        
        // Cara Superior (+Y)
        if (GetTipoBloque(x, y + 1, z) == TipoBloque.AIRE){
            GeneradorMalla.GenerarCaraArriba(this, posicion, tipo);
        }
        // Cara Inferior (-Y)
        if (GetTipoBloque(x, y - 1, z) == TipoBloque.AIRE){
            GeneradorMalla.GenerarCaraAbajo(this, posicion, tipo);
        }
        // Cara Frontal (+Z)
        if (GetTipoBloque(x, y, z + 1) == TipoBloque.AIRE){
            GeneradorMalla.GenerarCaraFrontal(this, posicion, tipo);
        }
        // Cara Trasera (-Z)
        if (GetTipoBloque(x, y, z - 1) == TipoBloque.AIRE){
            GeneradorMalla.GenerarCaraTrasera(this, posicion, tipo);
        }
        // Cara Derecha (+X)
        if (GetTipoBloque(x + 1, y, z) == TipoBloque.AIRE){
            GeneradorMalla.GenerarCaraDerecha(this, posicion, tipo);
        }
        // Cara Izquierda (-X)
        if (GetTipoBloque(x - 1, y, z) == TipoBloque.AIRE){
            GeneradorMalla.GenerarCaraIzquierda(this, posicion, tipo);
        }
    }
    public TipoBloque GetTipoBloque(int x, int y, int z) {
        if (x < 0 || x >= Anchura || y < 0 || y >= Altura || z < 0 || z >= Profundidad){
            // Si accedemos fuera del chunk (por ejemplo, para chequear un vecino del borde), 
            // asumimos que es aire o un bloque sólido (depende de cómo manejes los chunks vecinos).
            // Para el chunk inicial, asumir que todo fuera es Aire es más fácil.
            return TipoBloque.AIRE;
        }
        return bloques[x, y, z];
    }

    public void ModificarBloque(int x, int y, int z, TipoBloque tipo)
    {
        // Convertimos las coordenadas del mundo a coordenadas locales del chunk
        int localX = x - Mathf.FloorToInt(transform.position.x);
        int localY = y - Mathf.FloorToInt(transform.position.y);
        int localZ = z - Mathf.FloorToInt(transform.position.z);

        // Comprobamos si la posición está dentro de los límites de este chunk
        if (localX < 0 || localX >= Anchura ||
            localY < 0 || localY >= Altura ||
            localZ < 0 || localZ >= Profundidad)
        {
            // El bloque no pertenece a este chunk, no hacemos nada.
            // (En un futuro, aquí podrías buscar el chunk correcto)
            return;
        }

        // Actualizamos el tipo de bloque en nuestra matriz de datos
        bloques[localX, localY, localZ] = tipo;

        // Regeneramos la malla para que el cambio sea visible
        GenerarMallaOptimizada();
        GeneradorMalla.AplicarMallas(this);
    }
}
