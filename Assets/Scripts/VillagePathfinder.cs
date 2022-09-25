using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagePathfinder : MonoBehaviour
{
    [SerializeField] public Transform[] outerPoints = new Transform[6];
    [SerializeField] public Transform center;

    public List<Vector3> GetPath(int inDir, int outDir)
    {
        List<Vector3> path = new List<Vector3>();
        
        Transform current = outerPoints[inDir];
        while(true)
        {
            path.Add(current.position);
            if(current.childCount > 0)
            {
                current = current.GetChild(0);
            }
            else 
            {
                break;
            }
        }

        path.Add(center.position);

        current = outerPoints[outDir];

        List<Vector3> pathOut = new List<Vector3>();
        
        while(true)
        {
            pathOut.Add(current.position);
            if(current.childCount > 0)
            {
                current = current.GetChild(0);
            }
            else 
            {
                break;
            }
        }

        pathOut.Reverse();
        path.AddRange(pathOut);
        return path;
    }
}
