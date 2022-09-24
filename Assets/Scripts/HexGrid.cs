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

    [SerializeField] LayerMask hexGridLayerMask, squirrelLayerMask;

    [SerializeField]
    public Village villagePrefab;
    public MainBase mainBasePrefab;
    public OakTree oakTreePrefab;
    public Lake lakePrefab;
    public BeaverTradePost beaverTradePostPrefab;
    public MoleTradePost moleTradePostPrefab;
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
		//label.text = cell.coordinates.ToStringOnSeparateLines();  
        label.gameObject.SetActive(false);
        cell.label = label;  
    }
    
    void Update() 
    {
        if (Input.GetMouseButtonDown(0)) 
        {
			HandleLeftMousePressed();
		}

        if(Input.GetMouseButtonUp(0))
        {
            HandleLeftMouseReleased();
        }

        if(mouseButtonCurrentlyPressed)
        {
            if(beginSelectionPoint != Vector3.zero)
            {
                Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(inputRay, out hit, 1000.0f, hexGridLayerMask)) 
                {
                    if(selectionRectangleInstance == null)
                    {
                        selectionRectangleInstance = Instantiate(selectionRectanglePrefab);
                        
                    }
                    selectionRectangleInstance.UpdateRectangle(beginSelectionPoint, hit.point);
                }
            }
        }
        else
        {
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(inputRay, out hit, 1000.0f, squirrelLayerMask)) 
            {
                var squirrelHit = hit.collider.gameObject.GetComponent<SquirrelBehaviour>();
                if(currentlyDisplayingSquirrel == null)
                {
                    squirrelHit.ShowInventoryUI();
                    currentlyDisplayingSquirrel = squirrelHit;
                }
            }
            else
            {
                if(currentlyDisplayingSquirrel != null)
                {
                    currentlyDisplayingSquirrel.HideInventoryUI();
                    currentlyDisplayingSquirrel = null;
                }
            }
        }
    }

    SquirrelBehaviour currentlyDisplayingSquirrel;

    bool mouseButtonCurrentlyPressed = false;
    void HandleLeftMousePressed () 
    {
        mouseButtonCurrentlyPressed = true;
        beginSelectionPoint = Vector3.zero;
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit, 1000.0f, hexGridLayerMask)) 
        {
			TouchCell(hit.point);
		}
    }

    public SelectionRectangle selectionRectanglePrefab;
    public SelectionRectangle selectionRectangleInstance;

    void HandleLeftMouseReleased()
    {
        mouseButtonCurrentlyPressed = false;

        if(selectionRectangleInstance != null)
            Destroy(selectionRectangleInstance.gameObject);

        //try to select units
        if(UIManagement.Instance.selectionMode == SelectionMode.CommandUnits)
        {
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(inputRay, out hit, 1000.0f, hexGridLayerMask)) 
            {
                if(beginSelectionPoint == Vector3.zero)
                {
                    return;
                }

                endSelectionPoint = hit.point;

                Vector3 min = new Vector3(
                    beginSelectionPoint.x < endSelectionPoint.x ? beginSelectionPoint.x : endSelectionPoint.x,
                    0.0f,
                    beginSelectionPoint.z < endSelectionPoint.z ? beginSelectionPoint.z : endSelectionPoint.z);
                Vector3 max = new Vector3(
                    beginSelectionPoint.x > endSelectionPoint.x ? beginSelectionPoint.x : endSelectionPoint.x,
                    0.0f,
                    beginSelectionPoint.z > endSelectionPoint.z ? beginSelectionPoint.z : endSelectionPoint.z);

                for (int i = 0; i < allSquirrels.Count; i++)
                {
                    var current = allSquirrels[i];
                    Vector3 position = current.transform.position;
                    if(position.x + 3 > min.x && position.x - 3 < max.x && position.z + 3 > min.z && position.z - 3 < max.z)
                    {
                        current.SetHighlight();
                        selectedSquirrels.Add(current);
                    }
                }

                beginSelectionPoint = Vector3.zero;
                endSelectionPoint = Vector3.zero;
            
                if(selectedSquirrels.Count > 0)
                {
                    foreach (var cell in cells)
                    {
                        if(cell.content != null && cell.content.IsInteractable && cell.isAvailable)
                        {
                            highlightedCells.Add(cell);
                            cell.SetSelected();
                        }
                    }
                }
            }
        }
    }

    List<HexCell> highlightedCells = new List<HexCell>();

    void ClearSquirrelHighlights()
    {
        foreach (var s in selectedSquirrels)
        {
            s.RemoveHighlight();
        }
        foreach (var cell in highlightedCells)
        {
            cell.SetSelected();
        }
        selectedSquirrels.Clear();
        highlightedCells.Clear();
    }

    Vector3 beginSelectionPoint = Vector3.zero;
	Vector3 endSelectionPoint = Vector3.zero;
    public List<SquirrelBehaviour> selectedSquirrels = new List<SquirrelBehaviour>();
    public List<SquirrelBehaviour> allSquirrels = new List<SquirrelBehaviour>();

    public HexCell CellFromPosition(Vector3 wsPosition)
    {
        var position = transform.InverseTransformPoint(wsPosition);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
		return cells[index];
    }

    void TouchCell (Vector3 position) 
    {
		HexCell cell = CellFromPosition(position);

        var selectionMode = UIManagement.Instance.selectionMode;
        switch(selectionMode)
        {
            case SelectionMode.ToggleAvailable:
            {
                cell.ToggleAvailable(true);
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
            case SelectionMode.CommandUnits:
            {
                //if player clicks on unavailable cell, then try to buy it
                if(!cell.isAvailable && cell.buyLandMenuInstance != null)
                {
                    if(Global.BuyContent(cell.buyLandMenuInstance.currentlyDisplayingInventory))
                        cell.ToggleAvailable(true);
                        
                    return;
                }
                if(selectedSquirrels.Count != 0 && cell.isOccupied && cell.content.IsInteractable && cell.isAvailable)
                {
                    Debug.Log("Clicked on something valid!");
                    foreach (var squirrel in selectedSquirrels)
                    {
                        squirrel.IssueWork(mainBaseInstance, cell.content);
                    }
                    //Issue commands on the squirrels
                }
                ClearSquirrelHighlights();
                beginSelectionPoint = position;
                break;
            }
        }
    }

    public void OnUpdateSelectionMode(SelectionMode newMode)
    {   
        ClearSquirrelHighlights();
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

        if(path.Count > 1)
            path.RemoveAt(path.Count-1);
            
        path.Reverse();
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

                if(neighbor == toCell) //does not matter if the goal cell is unwalkable
                {
                    toCell.previous = current;
                    toCell.Distance = current.Distance + 1;
                    return TracePath(fromCell, toCell);                                    
                }
                
                
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
