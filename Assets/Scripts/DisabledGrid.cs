using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisabledGrid : MonoBehaviour
{    
    public int width = 6, height = 6;
    public GameObject cellPrefab;
    public GameObject[] cells;

    public void Generate()
    {
        cells = new GameObject[height * width];

		for (int z = 0, i = 0; z < height; z++) 
        {
			for (int x = 0; x < width; x++) 
            {
				CreateCell(x, z, i++);
			}
		}
    }

    void CreateCell(int x, int z, int i)
    {
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) *  HexMetrics.innerRadius * 2.0f;
		position.y = 0f;
		position.z = z * HexMetrics.outerRadius * 1.5f;

		GameObject cell = cells[i] = Instantiate(cellPrefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
    }
}
