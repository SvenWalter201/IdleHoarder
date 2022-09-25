using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleTradePost : TradePost
{
    public override void Initialize(HexCell hexCell)
    {
        base.Initialize(hexCell);
        //HexGrid.Instance.moleTradePost = this;
    }

    public override void Destruct()
    {
        base.Destruct();
        //HexGrid.Instance.moleTradePost = null;
    }


    public override ECurrency RequiredResource => ECurrency.Water;
    public override ECurrency ProducingResource => ECurrency.Stone;
    public override string Name => "Mole Trading Post";
}
