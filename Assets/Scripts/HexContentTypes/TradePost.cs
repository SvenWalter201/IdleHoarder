using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradePost : HexCellContent
{
    public float productionRate = 3f;
    float productionProgress = 0.0f;

    public virtual ECurrency ProducingResource => ECurrency.Acorn;

    public override bool IsDestructible => false;

        public override void ContentUpdate()
    {
        base.ContentUpdate();
        productionProgress += Time.deltaTime * productionRate;
        if(productionProgress >= 1.0f)
        {
            if(currentlyStoredResources.storedResources[(int)ProducingResource] < currentlyStoredResources.capacities[(int)ProducingResource] 
            && currentlyStoredResources.storedResources[(int)RequiredResource] > 0)
            {
                currentlyStoredResources.storedResources[(int)ProducingResource] += 1;
                currentlyStoredResources.storedResources[(int)RequiredResource] -= 1;
                UpdateInventoryUI();
            }
            productionProgress = 0;
        }
    }
}
