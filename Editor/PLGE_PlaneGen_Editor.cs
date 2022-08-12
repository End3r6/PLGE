using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PLGE_PlaneGenerator)), CanEditMultipleObjects]
public class PLGE_PlaneGen_Editor : Editor
{
   public override void OnInspectorGUI()
   {
        DrawDefaultInspector();

        PLGE_PlaneGenerator planeGen = (PLGE_PlaneGenerator)target;
        if(GUILayout.Button("Build Plane"))
        {
            planeGen.GenerateMesh();
        }
   }
}
