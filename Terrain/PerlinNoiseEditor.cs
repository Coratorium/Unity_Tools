#if (UNITY_EDITOR) 
using UnityEngine;
using System.Collections;
using UnityEditor;




[CustomEditor(typeof(PerlinNoiseTerrain))]
public class adjustBoxEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PerlinNoiseTerrain myScript = (PerlinNoiseTerrain)target;
        if (GUILayout.Button("Save Terrain"))
        {
            myScript.saveAsFile();
        }
    }
}
#endif
