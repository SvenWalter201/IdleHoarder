using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HexCell))]
public class HexCellEditor : Editor
{
    HexCell tgt;
     EContentType c;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        c = (EContentType)EditorGUILayout.EnumPopup(c);
        if(GUILayout.Button("Create Content"))
        {
            if(tgt.content != null)
                tgt.DestroyContent();

            switch(c)
            {
                case EContentType.MainBase: 
                {
                    
                    tgt.SetContent(HexGrid.Instance.mainBasePrefab);
                    break;
                }
                case EContentType.Lake: 
                {
                    tgt.SetContent(HexGrid.Instance.lakePrefab);
                    break;
                }
                case EContentType.OakTree: 
                {
                    tgt.SetContent(HexGrid.Instance.oakTreePrefab);
                    break;
                }
                case EContentType.BeaverTradingPost:
                {
                    tgt.SetContent(HexGrid.Instance.beaverTradePostPrefab);
                    break;
                }
                case EContentType.MoleTradingPost:
                {
                    tgt.SetContent(HexGrid.Instance.moleTradePostPrefab);
                    break;
                }
                case EContentType.None: 
                {
                    break;
                }
            }
        }
    }
    
    void OnEnable() 
    {
        tgt = target as HexCell;
    }
}
