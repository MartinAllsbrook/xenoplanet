using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapMetalPickup : ItemPickup
{
    protected override void UseItem()
    {
        Player.Instance.ChangeHealth(10);
        Debug.Log("SCRAP METAL BABYYYYY");
        base.UseItem();
    }
}
