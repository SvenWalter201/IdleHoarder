using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global
{
    public static int acornAmount, woodAmount, waterAmount, stoneAmount; //total currency

    public static bool BuyContent(HexCellContent cellContent)
    {
        var cost = cellContent.GetCost();
        if(acornAmount - cost.acorn >= 0 && woodAmount - cost.wood >= 0 && waterAmount - cost.water >= 0 && stoneAmount - cost.stone >= 0)
        {
            acornAmount -= cost.acorn;
            woodAmount -= cost.wood;
            waterAmount -= cost.water;
            stoneAmount -= cost.stone;

            UIManagement.Instance.UpdateCurrencyPanels();

            return true;
        }
        return false;
    }
}
