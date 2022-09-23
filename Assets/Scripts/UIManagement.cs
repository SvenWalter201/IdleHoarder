using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum ECurrency
{
    Acorn, Wood, Water, Stone
}
public class UIManagement : Singleton<UIManagement>
{
    [SerializeField] Button toggleAvailableButton;
    [SerializeField] Button toggleVillageBuildingButton;
    [SerializeField] Button toggleDestroyBuildingButton;
    [SerializeField] CurrencyUI acornC, woodC, waterC, stoneC;

    List<Button> btns = new List<Button>();

    ColorBlock pressed = new ColorBlock
    {
        normalColor = new Color(1.0f, 0.0f, 0.0f, 1.0f),
        highlightedColor = new Color(1.0f, 0.0f, 0.0f, 1.0f),
        pressedColor = new Color(1.0f, 0.0f, 0.0f, 1.0f),
        selectedColor = new Color(1.0f, 0.0f, 0.0f, 1.0f),
        disabledColor = new Color(1.0f, 0.0f, 0.0f, 1.0f),
        colorMultiplier = 1
    };

    ColorBlock normal = new ColorBlock
    {
        normalColor = new Color(0.0f, 1.0f, 0.0f, 1.0f),
        highlightedColor = new Color(0.0f, 1.0f, 0.0f, 1.0f),
        pressedColor = new Color(0.0f, 1.0f, 0.0f, 1.0f),
        selectedColor = new Color(0.0f, 1.0f, 0.0f, 1.0f),
        disabledColor = new Color(0.0f, 1.0f, 0.0f, 1.0f),
        colorMultiplier = 1
    };
    public SelectionMode selectionMode;

    private void Awake() {
        toggleAvailableButton.colors = normal;
        toggleVillageBuildingButton.colors = normal;
        toggleDestroyBuildingButton.colors = normal;

        btns.Add(toggleAvailableButton);
        btns.Add(toggleVillageBuildingButton);
        btns.Add(toggleDestroyBuildingButton);

        selectionMode = SelectionMode.None;
    }
    public void SetCurrency(ECurrency currency, int amount)
    {
        switch(currency)
        {
            case ECurrency.Acorn: acornC.SetAmount(amount); break;
            case ECurrency.Wood: woodC.SetAmount(amount); break;
            case ECurrency.Water: waterC.SetAmount(amount); break;
            case ECurrency.Stone: stoneC.SetAmount(amount); break;
        }
    }

    public void UpdateCurrencyPanels()
    {
            acornC.SetAmount(Global.acornAmount);
            woodC.SetAmount(Global.woodAmount);
            waterC.SetAmount(Global.waterAmount);
            stoneC.SetAmount(Global.stoneAmount);       
    }

    public void ToggleAvailable()
    {
        Toggle(toggleAvailableButton, SelectionMode.ToggleAvailable);

    }
    public void ToggleVillageBuilding()
    {
        Toggle(toggleVillageBuildingButton, SelectionMode.Build);
    }

    public void Toggle(Button btn, SelectionMode sM)
    {
        if(selectionMode == sM)
        {
            selectionMode = SelectionMode.None;
            btn.colors = normal;
        }
        else 
        {
            btn.colors = pressed;
            selectionMode = sM;
        } 

        foreach (var b in btns)
        {
            if(b != btn)
            {
                b.colors = normal;
            }
        }       
    }

    public void ToggleDestroy()
    {
        Toggle(toggleDestroyBuildingButton, SelectionMode.Destroy);
    }    
}

public enum SelectionMode
{
    Build,
    ToggleAvailable,
    Destroy,
    None
}
