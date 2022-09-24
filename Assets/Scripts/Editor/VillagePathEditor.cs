using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VillagePathfinder))]
public class VillagePathEditor : Editor
{
    VillagePathfinder tgt;

    void OnSceneGUI() 
    {
        if(tgt.center != null)
        {
            for (int i = 0; i < tgt.outerPoints.Length; i++)
            {
                var c = tgt.outerPoints[i];
                if(c != null)
                {
                    Debug.DrawLine(tgt.center.position, c.position, Color.blue, 2.0f);
                }
            }
        }

    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

    }
    private void OnEnable() {
        tgt = target as VillagePathfinder;
    }
}
