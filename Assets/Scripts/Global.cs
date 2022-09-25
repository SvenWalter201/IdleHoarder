using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global
{
    public static int acornAmount, woodAmount, waterAmount, stoneAmount; //total currency


    public static bool BuyContent(ResourceContainer cost)
    {

        if(cost.storedResources[0] == 0 && cost.storedResources[1] == 0 && cost.storedResources[2] == 0 && cost.storedResources[3] == 0)
        {
            return true;
        }
        
        var mainBase = HexGrid.Instance.mainBaseInstance;
        if(mainBase == null)
        {
            Debug.Log("Build Main Base first");
            return false;
        }

        var availableResources = mainBase.currentlyStoredResources.storedResources;

        bool canBuy = true;
        for (int i = 0; i < 4; i++)
        {
            if(availableResources[i] < cost.storedResources[i]) 
            {
                canBuy = false;
                break;
            }
        }
        if(canBuy)
        {
            for (int i = 0; i < 4; i++)
                availableResources[i] -= cost.storedResources[i];
            UIManagement.Instance.UpdateCurrencyPanels();
            return true;
        }
        return false;
    }
}
