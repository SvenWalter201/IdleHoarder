using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum ECurrency
{
    Acorn, Wood, Water, Stone, None
}
public class UIManagement : Singleton<UIManagement>
{

    [SerializeField] GameObject buildMenu;
    [SerializeField] Button toggleAvailableButton;
    [SerializeField] Button toggleVillageBuildingButton;
    [SerializeField] Button toggleDestroyBuildingButton;
    //[SerializeField] Button toggleCommandUnitsButton;
    [SerializeField] Button toggleBuildingModeButton;
    [SerializeField] CurrencyUI acornC, woodC, waterC, stoneC;

    List<Button> btns = new List<Button>();

    ColorBlock buildModePressed = new ColorBlock
    {
        normalColor = new Color(0.63f, 0.16f, 0.05f, 1.0f),
        highlightedColor = new Color(0.63f, 0.16f, 0.05f, 1.0f),
        pressedColor = new Color(0.63f, 0.16f, 0.05f, 1.0f),
        selectedColor = new Color(0.63f, 0.16f, 0.05f, 1.0f),
        disabledColor = new Color(0.63f, 0.16f, 0.05f, 1.0f),
        colorMultiplier = 1
    };

    ColorBlock BuildModeNormal = new ColorBlock
    {
        normalColor = new Color(0.53f, 0.24f, 0.03f, 1.0f),
        highlightedColor = new Color(0.53f, 0.24f, 0.03f, 1.0f),
        pressedColor = new Color(0.53f, 0.24f, 0.03f, 1.0f),
        selectedColor = new Color(0.53f, 0.24f, 0.03f, 1.0f),
        disabledColor = new Color(0.53f, 0.24f, 0.03f, 1.0f),
        colorMultiplier = 1
    };

    ColorBlock pressed = new ColorBlock
    {
        normalColor = new Color(1.0f, 0f, 0f, 1.0f),
        highlightedColor = new Color(1.0f, 0f, 0f, 1.0f),
        pressedColor = new Color(0.5f, 0f, 0f, 1.0f),
        selectedColor = new Color(1.0f, 0.1f, 0f, 1.0f),
        disabledColor = new Color(0.7f, 0.7f, 0.7f, 1.0f),
        colorMultiplier = 1
    };

    ColorBlock normal = new ColorBlock
    {
        normalColor = new Color(0.7f, 0.7f, 0.7f, 1.0f),
        highlightedColor = new Color(1.0f, 0f, 0f, 1.0f),
        pressedColor = new Color(0.5f, 0f, 0f, 1.0f),
        selectedColor = new Color(0.7f, 0.7f, 0.7f, 1.0f),
        disabledColor = new Color(0.7f, 0.7f, 0.7f, 1.0f),
        colorMultiplier = 1
    };
    public SelectionMode selectionMode;

    void Awake() 
    {
        toggleAvailableButton.colors = normal;
        toggleVillageBuildingButton.colors = normal;
        toggleDestroyBuildingButton.colors = normal;
        toggleBuildingModeButton.colors = BuildModeNormal;
        //toggleCommandUnitsButton.colors = normal;

        btns.Add(toggleAvailableButton);
        btns.Add(toggleVillageBuildingButton);
        btns.Add(toggleDestroyBuildingButton);
        //btns.Add(toggleCommandUnitsButton);

        //toggleVillageBuildingButton.gameObject.SetActive(buildingMode);
        //toggleDestroyBuildingButton.gameObject.SetActive(buildingMode);
        buildMenu.SetActive(buildingMode);
        selectionMode = SelectionMode.CommandUnits;
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
            var mainBaseStorage = HexGrid.Instance.mainBaseInstance.currentlyStoredResources.storedResources;

            acornC.SetAmount(mainBaseStorage[0]);
            woodC.SetAmount(mainBaseStorage[1]);
            waterC.SetAmount(mainBaseStorage[2]);
            stoneC.SetAmount(mainBaseStorage[3]);  
    }

    public void ToggleAvailable() => 
        Toggle(toggleAvailableButton, SelectionMode.ToggleAvailable);

    public void ToggleVillageBuilding() => 
        Toggle(toggleVillageBuildingButton, SelectionMode.Village);

    public void ToggleDestroy() => 
        Toggle(toggleDestroyBuildingButton, SelectionMode.Destroy);
      
    //public void ToggleCommandUnits() => 
    //    Toggle(toggleCommandUnitsButton, SelectionMode.CommandUnits);


    public bool buildingMode = false;
    public void ToggleBuildingMode()
    {
        if(toggleBuildingModeButton.colors == buildModePressed)
        {
            selectionMode = SelectionMode.CommandUnits;
            toggleBuildingModeButton.colors = BuildModeNormal;
        }
        else 
        {
            toggleBuildingModeButton.colors = buildModePressed;
            selectionMode = SelectionMode.Build;
        } 

        foreach (var b in btns)
        {
            if(b != toggleBuildingModeButton)
            {
                b.colors = normal;
            }
        } 

        HexGrid.Instance.OnUpdateSelectionMode(SelectionMode.Build);

        buildingMode = !buildingMode;
        buildMenu.SetActive(buildingMode);
        //toggleVillageBuildingButton.gameObject.SetActive(buildingMode);
        //toggleDestroyBuildingButton.gameObject.SetActive(buildingMode);

        if(!buildingMode) selectionMode = SelectionMode.CommandUnits;
    }
    
    public void Toggle(Button btn, SelectionMode sM)
    {
        if(btn.colors == pressed)
        {
            if(toggleBuildingModeButton.colors == buildModePressed)
                selectionMode = SelectionMode.Build;
            else 
                selectionMode = SelectionMode.CommandUnits;
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

        HexGrid.Instance.OnUpdateSelectionMode(sM);      
    }

  
}

public enum SelectionMode
{
    Build,
    Village,
    ToggleAvailable,
    Destroy,
    CommandUnits,
    None
}
