using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DisabledGrid))]
public class DisabledGridEditor : Editor
{
    DisabledGrid tgt;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Generate"))
        {
            tgt.Generate();
        }
    }
    void OnEnable() 
    {
        tgt = target as DisabledGrid;
    }
}
