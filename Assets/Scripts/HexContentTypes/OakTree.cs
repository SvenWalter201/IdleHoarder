using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OakTree : HexCellContent
{
    public float productionRate = 0.5f;
    public override bool IsDestructible => false;

    public override ECurrency RequiredResource => ECurrency.None;
    public override string Name => "Oak Tree";

    public override void Initialize(HexCell hexCell)
    {
        base.Initialize(hexCell);
        HexGrid.Instance.AddProductionSite(this);
    }

    public override void Destruct()
    {
        base.Destruct();
        HexGrid.Instance.RemoveProductionSite(this);
    }

    float productionProgress = 0.0f;

    public override void ContentUpdate()
    {
        base.ContentUpdate();
        productionProgress += Time.deltaTime * productionRate;
        if(productionProgress >= 1.0f)
        {
            if(currentlyStoredResources.storedResources[0] < currentlyStoredResources.capacities[0])
            {
                currentlyStoredResources.storedResources[0] += 1;
                UpdateInventoryUI();
            }
            productionProgress = 0;
        }

    }
}
