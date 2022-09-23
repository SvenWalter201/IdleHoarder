using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ResourceContainer
{
    public int acorn, wood, water, stone;
}

public class HexCellContent : MonoBehaviour
{
    public virtual bool IsDestructible => true;
    public virtual ResourceContainer GetCost() {return default; }

    public HexCell hexCellReference;

    public virtual void Initialize(HexCell hexCell)
    {
        hexCellReference = hexCell;
    }

    public virtual bool SpawnConditions => true;


    public virtual void Destruct()
    {

    }
}
