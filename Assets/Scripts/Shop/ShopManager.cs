using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/*
 * Shop Manager should manage the items.
 * Buy, Sell, Checks. Use this to handle transactions.
 */
public class ShopManager : GenericSingleton<ShopManager>
{
    private ShopData currentShopData;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        ShopEvents.OnConfirmBuy += HandleConfirmBuyItem;
        ShopEvents.OnShopOpened += StoreCurrentShopData;
    }
        
    private void OnDisable()
    {
        ShopEvents.OnConfirmBuy -= HandleConfirmBuyItem;
        ShopEvents.OnShopOpened -= StoreCurrentShopData;    
    }

    public void HandleConfirmBuyItem(Item item)
    {
        if (CurrencyManager.Instance.GetCoins() >= item.price)
        {
            Debug.Log("Item was bought!");
            CurrencyManager.Instance.TakeCoins(item.price);
            currentShopData.RemoveItem(item);
        }
    }

    public void HandleConfirmSellItem(Item item)
    {
        //Implement selling later here.
    }

    private void StoreCurrentShopData(ShopData data)
    {
        currentShopData = data;
    }
}
