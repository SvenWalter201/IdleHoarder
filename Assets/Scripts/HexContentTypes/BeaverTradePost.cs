using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaverTradePost : TradePost
{
    public override void Initialize(HexCell hexCell)
    {
        base.Initialize(hexCell);
    }

    public override void Destruct()
    {
        base.Destruct();
    }

    public override ECurrency RequiredResource => ECurrency.Acorn;
    public override ECurrency ProducingResource => ECurrency.Wood;
    public override string Name => "Beaver Trading Post";



}
