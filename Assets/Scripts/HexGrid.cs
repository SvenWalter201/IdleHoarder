using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum ETradePost
{
    Beaver,
    Mole
}

public enum EContentType
{
    None, OakTree, MainBase, Village, Lake, BeaverTradingPost, MoleTradingPost
}

public class HexGrid : Singleton<HexGrid>
{
    public int width = 6, height = 6;

    public HexCell cellPrefab;
	public TMP_Text cellLabelPrefab;

    public HexCell[] cells;

    Canvas gridCanvas;

    MeshCollider meshCollider;

    [SerializeField]
    public Village villagePrefab;
    public MainBase mainBasePrefab;
    public OakTree oakTreePrefab;
    public Lake lakePrefab;
    [Space]
    public MainBase mainBaseInstance;
    public List<HexCellContent> productionSites = new List<HexCellContent>();
    public BeaverTradePost beaverTradePost;
    public MoleTradePost moleTradePost;

    void Awake() 
    {

        gridCanvas = GetComponentInChildren<Canvas>();
        meshCollider = GetComponent<MeshCollider>();      
        //Generate();
    }

    public void AddProductionSite(HexCellContent productionSite)
    {
        productionSites.Add(productionSite);
    }

    public void RemoveProductionSite(HexCellContent productionSite)
    {
        productionSites.Remove(productionSite);
    }

    public void Generate()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        meshCollider = GetComponent<MeshCollider>();
        cells = new HexCell[height * width];

		for (int z = 0, i = 0; z < height; z++) 
        {
			for (int x = 0; x < width; x++) 
            {
				CreateCell(x, z, i++);
			}
		}
    }

    public HexCell GetRandom()
    {
        while(true)
        {
            int r = Random.Range(0, cells.Length - 1);
            if(cells[r].isAvailable && !cells[r].isOccupied)
                return cells[r];
        }
    }



    public HexCell GetDepositLocation()
    {
        return null;
    }

    public HexCell GetRandomProductionSite()
    {
        return null;
    }

    void CreateCell(int x, int z, int i)
    {
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) *  HexMetrics.innerRadius * 2.0f;
		position.y = 0f;
		position.z = z * HexMetrics.outerRadius * 1.5f;

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);

        if (x > 0) 
        {
			cell.SetNeighbor(HexDirection.W, cells[i - 1]);
		}

	    if (z > 0) 
        {
			if ((z & 1) == 0) 
            {
				cell.SetNeighbor(HexDirection.SE, cells[i - width]);
                if (x > 0) 
                {
					cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
				}
			}
            else {
				cell.SetNeighbor(HexDirection.SW, cells[i - width]);
				if (x < width - 1) 
                {
					cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
				}
			}
		}

		TMP_Text label = Instantiate<TMP_Text>(cellLabelPrefab);
		label.rectTransform.SetParent(gridCanvas.transform, false);
		label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
		label.text = cell.coordinates.ToStringOnSeparateLines();  

        cell.label = label;  
    }
    
    void Update() 
    {
        if (Input.GetMouseButtonDown(0)) 
        {
			HandleInput();
		}
    }
    void HandleInput () 
    {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit)) 
        {
			TouchCell(hit.point);
		}
    }
	
    void TouchCell (Vector3 position) 
    {
        position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
		HexCell cell = cells[index];

        var selectionMode = UIManagement.Instance.selectionMode;
        switch(selectionMode)
        {
            case SelectionMode.ToggleAvailable:
            {
                cell.ToggleAvailable();
                OnGridUpdate?.Invoke();
                break;
            }
            case SelectionMode.Build:
            {
                if(cell.isOccupied)
                    return;
                
                if(cell.SetContent(villagePrefab))
                    OnGridUpdate?.Invoke();
                
                break;
            }
            case SelectionMode.Destroy:
            {
                if(!cell.isOccupied)
                    return;

                if(cell.DestroyContent())
                    OnGridUpdate?.Invoke();

                break;
            }
        }
    }

    public delegate void Notify();  // delegate

    public event Notify OnGridUpdate;

    List<HexCell> TracePath(HexCell fromCell, HexCell toCell)
    {
        List<HexCell> path = new List<HexCell>();
        path.Add(toCell);
        int maxIter = 100;
        int it = 0;
        while(it < maxIter)
        {
            it++;
            if(path[path.Count - 1].previous != null)
            {
                path.Add(path[path.Count - 1].previous);
            }
            else 
            {
                break;
            }
        }
        if(it >= maxIter)
        {
            Debug.Log("Max Length Error!");
        }
        return path;
    }

    public List<HexCell> FindPath(HexCell fromCell, HexCell toCell)
    {
        for (int i = 0; i < cells.Length; i++) 
        {
			cells[i].Distance = int.MaxValue;
            cells[i].previous = null;
		}

        Queue<HexCell> frontier = new Queue<HexCell>();
		fromCell.Distance = 0;
		frontier.Enqueue(fromCell);
        while (frontier.Count > 0) 
        {
			HexCell current = frontier.Dequeue();
            if(current == toCell)
            {
                return TracePath(fromCell, toCell);
            }

            for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++) 
            {
				HexCell neighbor = current.GetNeighbor(d);
				if (neighbor == null || neighbor.Distance != int.MaxValue) 
                    continue;
                
                if(neighbor.isOccupied)
                    continue;

                if(!neighbor.isAvailable)
                    continue;

                neighbor.Distance = current.Distance + 1;
                neighbor.previous = current;
                frontier.Enqueue(neighbor);
				
			}
		}
        Debug.LogWarning("No Path found");
        return null;
    }
}
