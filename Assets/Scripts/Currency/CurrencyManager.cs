using Unity.VisualScripting;
using UnityEngine;

public class CurrencyManager : GenericSingleton<CurrencyManager>
{

    [SerializeField] private Currency _currencyData;

    /*
     * ADD LATER: Two types of currency, use enum for 'CurrencyType'
     * Lerp: One number to another over a fixed duration
     */

    protected override void Awake()
    {
        base.Awake();
    }

    public void AddCoins(int amount)
    {
        _currencyData.currentCurrency += amount;
    }

    /*
     * USE: Use for shops, currency will be currency for the player
     */
    public void TakeCoins(int amount)
    {
        _currencyData.currentCurrency -= amount;
    }

    public void ClearCoins()
    {
        _currencyData.currentCurrency = 0;
    }

    public int GetCoins() => _currencyData.currentCurrency;
}
