using NUnit.Framework;
using UnityEngine;

/*
 * ShopKeeper should only hold what is needed. Their inventory, dialogue, and send events. 
 * Using the inventory from the Scritpable Object, send the 'inventoryData' to ShopManager to load/display inventory
 */
public class ShopKeeper : BaseNPC
{
    [Header("Shop Inventory")]
    [SerializeField] private ShopData shopData;

    /*
     * Makes sure to only initialize the base shop when we first interact with the shop. This is to prevent resetting the inventory every
     * time we talk to the NPC
     */
    private bool _isShopInitialized = false; 

    public void SendShopInventory()
    {
        ShopEvents.OpenShop(shopData);
    }

    public void CloseShop()
    {
        //FIX: Rather than hardcoding GameState.Playing make sure to change this into whatever panel we are currently on(if we are in one)
        //Maybe no change but for clarirt maybe change
        GameManager.Instance.UpdateGameState(GameState.Playing);
    }
    public override void Interact()
    {
        if (!canInteract) return;
        if(!_isShopInitialized)
        {
            shopData.InitializeRuntime();
            _isShopInitialized = true;
        }
        SendShopInventory();
    }
}
