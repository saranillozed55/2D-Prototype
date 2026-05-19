using cherrydev;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.UIElements;

/*
 * Whenever someone talks to a Shop this will open and will be pushed into the stack.
 * This script loads the items of the NPC with their runTimeInventory
 */
public class ShopMenuPanel : UIToolkitPanel
{
    private ShopData _currentShopData;

    [SerializeField] private VisualTreeAsset itemTemplate;
    private VisualElement _holder; // This should be the reference to the UXML of Shop.UXML

    private List<ShopItemSlot> _activeSlot = new List<ShopItemSlot>();
    protected override void Awake()
    {
        base.Awake();
        Root.AddToClassList("hidden");
    }

    private void OnEnable()
    {
        ShopEvents.OnShopOpened += HandleShopOpened;
    }

    private void OnDisable()
    {
        ShopEvents.OnShopOpened -= HandleShopOpened;
    }

    public override void OnOpen()
    {
        base.OnOpen();
        GameManager.Instance.UpdateGameState(GameState.Shopping);
    }

    private void HandleShopOpened(ShopData shopData)
    {
        //Unsubscribe from previous shop if switching
        if (_currentShopData != null) _currentShopData.OnInventoryChanged -= LoadItems;

        _currentShopData = shopData;
        _holder = Root.Q<VisualElement>("holder"); //Due to base class UIToolKitPanel, can use Root for the Shop.UXML
        _currentShopData.OnInventoryChanged += LoadItems;

        
        LoadItems(shopData.RunTimeInventory);
        UIStackManager.Instance.Push(this);
    }

    private void LoadItems(IReadOnlyList<Item> items)
    {
        //clean up existing slots if there are any and unregister any events if necessary
        foreach (var slot in _activeSlot)
        {
            slot.CleanUp();
        }
        _activeSlot.Clear();
        _holder.Clear();

        foreach (var item in items)
        {
            var slot = new ShopItemSlot(itemTemplate, item);
            _activeSlot.Add(slot); //track it 
            _holder.Add(slot.GetElement());
        }
    }

}
