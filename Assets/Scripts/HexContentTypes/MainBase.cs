using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBase : HexCellContent
{
    public override bool IsDestructible => false;
    public override bool IsInteractable => false;
    public override string Name => "Main Base";

    public override bool SpawnConditions => HexGrid.Instance.mainBaseInstance == null;

    public override void Initialize(HexCell hexCell)
    {
        base.Initialize(hexCell);
        HexGrid.Instance.mainBaseInstance = this;
    }

    public override void Destruct()
    {
        base.Destruct();
        HexGrid.Instance.mainBaseInstance = null;
    }

    float progressRate1 = 1.0f, progressRate2 = 0.5f;
    float progress1 = 0.0f, progress2 = 0.0f;

    public override void ContentUpdate()
    {
        base.ContentUpdate();
        progress1 += Time.deltaTime * progressRate1;
        if(progress1 >= 1.0f)
        {
            if(currentlyStoredResources.storedResources[0] < currentlyStoredResources.capacities[0])
            {
                currentlyStoredResources.storedResources[0] += 1;
                UpdateInventoryUI();
                UIManagement.Instance.UpdateCurrencyPanels();
            }
            progress1 = 0;
        }

        progress2 += Time.deltaTime * progressRate2;
        if(progress2 >= 1.0f)
        {
            if(currentlyStoredResources.storedResources[2] < currentlyStoredResources.capacities[2])
            {
                currentlyStoredResources.storedResources[2] += 1;
                UpdateInventoryUI();
                UIManagement.Instance.UpdateCurrencyPanels();
            }
            progress2 = 0;
        }
    }
}
