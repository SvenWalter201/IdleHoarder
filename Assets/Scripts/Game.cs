using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] int startAcorn = 20, startWood = 0, startWater = 0, startStone = 0;

    private void Start() 
    {
        Global.acornAmount = startAcorn;
        Global.woodAmount = startWood;
        Global.waterAmount = startWater;
        Global.stoneAmount = startStone;

        UIManagement.Instance.UpdateCurrencyPanels();
    }
}
