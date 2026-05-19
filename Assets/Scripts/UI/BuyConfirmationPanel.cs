using UnityEngine;
using UnityEngine.UIElements;

public class BuyConfirmationPanel : UIToolkitPanel
{
    private Button _yesButton;
    private Button _noButton;

    //Hold the item that is being bought so that when the yes button is clicked we can send the correct item to the ShopManager to be bought
    private Item item;

    protected override void Awake()
    {
        base.Awake();
        Root.AddToClassList("hidden");
        Debug.Log($"Buy Confirmation Panel was added to hidden: {Root.ClassListContains("hidden")}");
    }

    private void OnEnable()
    {
        ShopEvents.OnRequestToBuy += HandleConfirmationBuy;
    }

    private void OnDisable()
    {
        ShopEvents.OnRequestToBuy -= HandleConfirmationBuy;
        if (_yesButton != null) _yesButton.UnregisterCallback<ClickEvent>(OnYesButtonClicked);
        if(_noButton != null) _noButton.UnregisterCallback<ClickEvent>(OnNoButtonClicked);
    }

    public override void OnOpen()
    {
        base.OnOpen();
        if(Root != null)
        {
            _yesButton = Root.Q<Button>("YesButton");
            _noButton = Root.Q<Button>("NoButton");
            if(_yesButton != null) _yesButton.RegisterCallback<ClickEvent>(OnYesButtonClicked);
            if(_noButton != null) _noButton.RegisterCallback<ClickEvent>(OnNoButtonClicked);
        }
    }

    private void OnYesButtonClicked(ClickEvent evt)
    {
        Debug.Log("Yes Button Clicked");
        ShopEvents.ConfirmBuy(item);
        Root.AddToClassList("hidden");
    }
    private void OnNoButtonClicked(ClickEvent evt)
    {
        Debug.Log("No Button Clicked");

        Root.AddToClassList("hidden");
    }

    private void HandleConfirmationBuy(Item item)
    {
        Debug.Log("Confirmation Raised");
        OnOpen();
        this.item = item;
    }
}
