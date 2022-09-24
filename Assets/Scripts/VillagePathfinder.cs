using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagePathfinder : MonoBehaviour
{
    [SerializeField] public Transform[] outerPoints = new Transform[6];
    [SerializeField] public Transform center;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Vector3> GetPath(HexDirection inDir, HexDirection outDir)
    {
        List<Vector3> path = new List<Vector3>();
        
        Transform current = outerPoints[(int)inDir];
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

        current = outerPoints[(int)outDir];

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
