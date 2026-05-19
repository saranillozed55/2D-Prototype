using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public interface IShop
{
    public void SendShopInventory();
    public void BuyItem();
    public void SellItem();
    public bool CanAfford(int price);
}
