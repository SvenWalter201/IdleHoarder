using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HexCell : MonoBehaviour
{
    MeshRenderer mR;
    bool selected = false;

    public bool isOccupied = false;
    public bool isAvailable = true;

    [SerializeField]
	HexCell[] neighbors;
    public HexCellContent content;
    public HexCell previous;
    int distance;

    public int Distance {
		get {
			return distance;
		}
		set {
			distance = value;
			//UpdateDistanceLabel();
		}
	}

	public HexCoordinates coordinates;
    public TMP_Text label;
    void Awake() 
    {
        mR = GetComponent<MeshRenderer>();
        mR.material.SetInt("_Available", isAvailable ? 1 : 0);
        transform.position += Vector3.up * 2.0f * (isAvailable ? 0 : 1);
        if(content != null)
        {
            content.SetAvailable(isAvailable);
        }
    }

    void Start() 
    {
        //transform.position += Vector3.up * Random.Range(-1f, 1f);
        SetBuyLandMenu(true);
    }

    public void SetHovered(Color color)
    {
        mR.material.SetColor("_HoverColor", color);
    }

    public void ResetHovered()
    {
        mR.material.SetColor("_HoverColor", Color.black);
    }

    public void ToggleAvailable(bool updateGraphics)
    {
        isAvailable = !isAvailable;
        if(updateGraphics)
        {
            mR.material.SetInt("_Available", isAvailable ? 1 : 0);
            transform.position += Vector3.up * 2.0f * (isAvailable ? -1 : 1);
            if(content != null)
            {
                content.SetAvailable(isAvailable);
            }
        }

        if(Application.isPlaying)
        {
            SetBuyLandMenu(true);
        }
    }

    public void SetBuyLandMenu(bool cascade)
    {
        if(!isAvailable)
        {
            bool foundAvailableNeighbor = false;
            for (int i = 0; i < neighbors.Length; i++)
            {
                if(neighbors[i] != null && neighbors[i].isAvailable)
                {
                    foundAvailableNeighbor = true;

                    break;
                }
            }

            if(foundAvailableNeighbor)
            {
                var cost = new ResourceContainer();

                const int beginwood = 45, beginWater = 65, beginStone = 80;
                Vector3 baseLocation = HexGrid.Instance.mainBaseInstance.transform.position;
                float distance = (baseLocation - transform.position).magnitude;
                int acornCost = Mathf.RoundToInt((distance / 15.0f) * (distance / 15.0f) * (distance / 15.0f));
                int woodCost = Mathf.RoundToInt(Mathf.Max(0.0f, ((distance - beginwood) / 15.0f) * ((distance - beginwood)  / 15.0f) * ((distance - beginwood)  / 15.0f)));
                int waterCost = Mathf.RoundToInt(Mathf.Max(0.0f, ((distance - beginWater)  / 15.0f) * ((distance - beginWater)  / 15.0f) * ((distance - beginWater)  / 15.0f)));
                int stoneCost = Mathf.RoundToInt(Mathf.Max(0.0f, ((distance - beginStone)  / 15.0f) * ((distance - beginStone)  / 15.0f) * ((distance - beginStone)  / 15.0f)));

                cost.capacities[0] = acornCost;
                cost.storedResources[0] = acornCost;
                cost.capacities[1] = woodCost;
                cost.storedResources[1] = woodCost;                
                cost.capacities[2] = waterCost;
                cost.storedResources[2] = waterCost;                
                cost.capacities[3] = stoneCost;
                cost.storedResources[3] = stoneCost;
                if(buyLandMenuInstance == null)
                {
                    buyLandMenuInstance = Instantiate(buyLandMenuPrefab, transform.position + Vector3.up * 5, Quaternion.identity);
                }
                buyLandMenuInstance.UpdateUI(cost); 
            }
            else
            {
                if(buyLandMenuInstance != null)
                    Destroy(buyLandMenuInstance.gameObject);
            }
        }
        else
        {
                if(buyLandMenuInstance != null)
                    Destroy(buyLandMenuInstance.gameObject);            
        }

        if(cascade)
        {
            for (int i = 0; i < neighbors.Length; i++)
            {
                if(neighbors[i] != null)
                    neighbors[i].SetBuyLandMenu(false);
            }
        }

    }

    public InventoryUI buyLandMenuPrefab;
    [HideInInspector] public InventoryUI buyLandMenuInstance;

    public void GetDistanceToNextWalkable()
    {

    }

    public void SetSelected()
    {
        selected = !selected;
        mR.material.SetInt("_Selected", selected ? 1 : 0);
    }
    public HexCell GetNeighbor (int direction) 
    {
		return neighbors[direction];
	}
    public HexCell GetNeighbor (HexDirection direction) 
    {
		return neighbors[(int)direction];
	}

    public void SetNeighbor (HexDirection direction, HexCell cell) 
    {
		neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
	}

    void UpdateDistanceLabel()
    {
        label.text = distance == int.MaxValue ? "" : distance.ToString();
    }

    public bool SetContent(HexCellContent content)
    {
        if(!content.SpawnConditions)
            return false;
        
        if(Global.BuyContent(content.GetCost()))
        {
            this.content = Instantiate(content, transform.position, Quaternion.identity);
            this.content.Initialize(this);
            isOccupied = true;
            return true;
        }

        return false;
    }

    public bool DestroyContent()
    {
        if(content.IsDestructible)
        {
            content.HideDetails();  
            Destroy(content.gameObject);
            isOccupied = false;
            return true;
        }
        if(!Application.isPlaying)
        {
            DestroyImmediate(content.gameObject);
            isOccupied = false;
            return true;            
        }
        return false;
    }
}
