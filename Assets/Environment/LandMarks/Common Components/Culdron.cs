using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Culdron : IntuitionSource
{
    protected override void Consume()
    {
        base.Consume();
        Player.Instance.AddShield(15);
    } 
}
