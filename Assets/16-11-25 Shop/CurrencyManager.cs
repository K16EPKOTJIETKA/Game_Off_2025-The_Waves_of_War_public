using UnityEngine;
using Injection;

public class CurrencyManager : MonoBehaviour
{
    [Inject] ShopManager shopManager;
    [Inject] Injector injector;
    public int currentBalance = 333;

    public TMPro.TextMeshProUGUI balanceText;


    public void Initialize()
    {
        injector.Inject(this);
        UpdateBalanceUI();
    }

    public void AddCurrency(int amount)
    {
        if (amount < 0) amount = 0;
        currentBalance += amount;
        UpdateBalanceUI();
        shopManager.UpdateAllItemVisuals();
    }

    public bool TryPurchase(int cost)
    {
        if (currentBalance >= cost)
        {
            currentBalance -= cost;
            UpdateBalanceUI();
            shopManager.UpdateAllItemVisuals();
            return true; 
        }
        return false; 
    }

    private void UpdateBalanceUI()
    {
        if (balanceText != null)
        {
            balanceText.text = "Balance: " + currentBalance.ToString();
        }
    }
}