using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Mundo : MonoBehaviour
{
    public List<Bloque> DatosBloques = new List<Bloque>();

    public static Mundo main;

    private void Awake()
    {
        main = this;
    }

}
