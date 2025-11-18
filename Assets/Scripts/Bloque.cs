using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum TipoBloque : byte{
    AIRE = 0,
    TIERRA = 1,
    HIERBA = 2,
    PIEDRAROTA = 3,
    PIEDRA = 4,
    TRONCO = 5,
    MADERA = 6,
    HOJAS = 7,
    AGUA = 8
}
    [CreateAssetMenu(menuName = "Bloque")]
public class Bloque : ScriptableObject{
    public Texture2D texturaLateral;
    public Texture2D texturaArriba;
    public Texture2D texturaAbajo;
    
}