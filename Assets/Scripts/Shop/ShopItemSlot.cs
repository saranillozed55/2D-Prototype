using System;
using UnityEngine;
using UnityEngine.UIElements;

/*
 * [Serializable] allows object's state to be stored or transmitted and later reconstructed(deseralized)
 */

[Serializable]
public class ShopItemSlot
{
    [Header("UXML")]
    private VisualTreeAsset itemTemplate;

    private VisualElement slotElementRoot;
    private Item item;

    private Label itemNameLabel;
    private Label itemPriceLabel;
    private Image itemIcon;

    public ShopItemSlot(VisualTreeAsset itemTemplate, Item item)
    { 
        this.itemTemplate = itemTemplate;
        this.item = item;

        Initialize();
    }

    private void Initialize()
    {
        slotElementRoot = itemTemplate.Instantiate();

        itemNameLabel = slotElementRoot.Q<Label>("ItemName");
        itemPriceLabel = slotElementRoot.Q<Label>("ItemPrice");
        itemIcon = slotElementRoot.Q<Image>("ItemImage");

        slotElementRoot.RegisterCallback<ClickEvent>(OnSlotClicked);

        Populate();
    }

    private void Populate()
    {
        itemNameLabel.text = item.itemName;
        itemPriceLabel.text = item.price.ToString();
        itemIcon.sprite = item.icon;
    }

    public VisualElement GetElement()
    {
        return slotElementRoot;
    }

    private void OnSlotClicked(ClickEvent evt)
    {
        Debug.Log($"You clicked on {itemNameLabel.text}, and the price of this item is {itemPriceLabel.text}, with an image of{itemIcon.sprite.ToString()}");
        ShopEvents.RequestToBuy(item);
    }

    public void CleanUp()
    {
        slotElementRoot.UnregisterCallback<ClickEvent>(OnSlotClicked);
    }
}
