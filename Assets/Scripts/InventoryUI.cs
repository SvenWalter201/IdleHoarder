using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public CurrencyUI currencyUIPrefab;

    public List<Sprite> icons = new List<Sprite>();
    public List<CurrencyUI> currencyUIs = new List<CurrencyUI>();

    public ResourceContainer currentlyDisplayingInventory;
    public void UpdateUI(ResourceContainer containerToDisplay, Transform follow = null)
    {
        currentlyDisplayingInventory = containerToDisplay;
        int thingsToDisplay = 0;
        for (int i = 0; i < 4; i++)
        {
            if(containerToDisplay.capacities[i] > 0)
                thingsToDisplay++;
        }

        if(thingsToDisplay != currencyUIs.Count)
        {
            int startingPos = (thingsToDisplay - 1) * 4;
            currencyUIs.Clear();
            for (int i = 0; i < 4; i++)
            {
                if(containerToDisplay.capacities[i] > 0)
                {
                    var current = Instantiate(currencyUIPrefab);
                    current.SetAmount(containerToDisplay.storedResources[i]);
                    current.SetIcon(icons[i]); 
                    currencyUIs.Add(current);
                    current.transform.SetParent(transform, false);
                    current.transform.localPosition = Vector3.up * startingPos;
                    startingPos-=4;
                }
            }
        }
        else
        {
            int idx = 0;
            for (int i = 0; i < 4; i++)
            {
                if(containerToDisplay.capacities[i] > 0)
                {
                    currencyUIs[idx].SetAmount(containerToDisplay.storedResources[i]);
                    currencyUIs[idx].SetIcon(icons[i]);
                    idx++;
                }
            }
        }

        f = follow;
    }

    Transform f;

    void Update() 
    {
        if(f != null)
        {
            transform.position = f.position;
        }
    }
}
