using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShopTestScript : BaseScriptedEvent
{
    public void ItemShopTest()
    {
        ShowItemShop();
    }

    void ShowItemShop()
    {
        OpenShop("Item");
    }
}
