using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairPack : UseHandler
{
    protected override void UseItem()
    {
        base.UseItem();
        Player.Instance.AddHealth(10);
    }
}
