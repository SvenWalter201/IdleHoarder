using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaverTradePost : TradePost
{
    public override void Initialize(HexCell hexCell)
    {
        base.Initialize(hexCell);
        HexGrid.Instance.beaverTradePost = this;
    }

    public override void Destruct()
    {
        base.Destruct();
        HexGrid.Instance.beaverTradePost = null;
    }
}
