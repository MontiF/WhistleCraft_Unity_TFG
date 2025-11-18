using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
using System.IO;

[CustomEditor(typeof(Mundo))]

public class MundoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Paquete Texture Atlas"))
        {
            Mundo mundo = (Mundo)target;
            GeneradorTexturasAtlas.Generate(mundo);
            byte[] pngBytes = GeneradorTexturasAtlas.textureAtlas.EncodeToPNG();
            File.WriteAllBytes(Path.Combine(Application.dataPath, "textureAtlas.png"), pngBytes);
            AssetDatabase.Refresh(); // Le decimos a Unity que refresque para que vea el nuevo fichero
        }
        base.OnInspectorGUI();
    }

}
