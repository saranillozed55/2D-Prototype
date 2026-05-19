using System;
using UnityEngine;

public static class ShopEvents
{
    public static event Action<ShopData> OnShopOpened;
    public static event Action OnShopClosed;
    public static event Action<Item> OnRequestToBuy;

    public static event Action<Item> OnConfirmBuy;

    //events can only be invoked from within the class that declares them so use these helper methods
    public static void OpenShop(ShopData data) => OnShopOpened?.Invoke(data);
    public static void CloseShop() => OnShopClosed?.Invoke();
    public static void RequestToBuy(Item item) => OnRequestToBuy?.Invoke(item); // might not be needed
    public static void ConfirmBuy(Item item) => OnConfirmBuy?.Invoke(item);
}
