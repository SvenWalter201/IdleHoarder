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
           
       
    }

    public void SetSelected()
    {
        selected = !selected;
        mR.material.SetInt("_Selected", selected ? 1 : 0);
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
        
        if(Global.BuyContent(content))
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
