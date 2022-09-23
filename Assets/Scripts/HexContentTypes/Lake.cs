using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lake : HexCellContent
{
    public float productionRate = 0.5f;
    public override bool IsDestructible => false;

    public override ECurrency RequiredResource => ECurrency.None;
    public override string Name => "Lake";

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
            if(currentlyStoredResources.storedResources[2] < currentlyStoredResources.capacities[2])
            {
                currentlyStoredResources.storedResources[2] += 1;
                UpdateInventoryUI();
            }
            productionProgress = 0;
        }

    }
}
