using UnityEngine;
using System.Collections.Generic;
using Injection;
using System;

public class ShopManager : MonoBehaviour
{
    [Header("UI Елементи")]
    public GameObject itemPrefab;
    public Transform contentParent; 

    [Header("Логіка Скролінгу")]

    [SerializeField] ManualSliderScroll manualSliderScroll;

    [Header("Дані")]
    public ShopItemData[] allAvailableItems;

    public List<GameObject> activeItemCards = new List<GameObject>();

    [Inject] Injector injector;

    public event Action<ShopItemData> productPurchased;

    public void OnProductPurchased(ShopItemData shopItem)
    {
        productPurchased?.Invoke(shopItem);
    }

    public void Initialize()
    {
        LoadShopItems();

    }

    private void LoadShopItems()
    {

        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
        activeItemCards.Clear();

        foreach (var data in allAvailableItems)
        {
            GameObject cardObject = Instantiate(itemPrefab, contentParent);
            ShopItemUI cardUI = cardObject.GetComponent<ShopItemUI>();
            injector.Inject(cardUI);

            if (cardUI != null)
            {
              
                cardUI.Setup(data);
                activeItemCards.Add(cardObject);
            }
        }


        manualSliderScroll.RecalculateCounts();
    }

    public void RemoveItemFromList(GameObject itemToRemove)
    {
        activeItemCards.Remove(itemToRemove);
        manualSliderScroll.RecalculateCounts();
        if (activeItemCards.Count <= 3) manualSliderScroll.gameObject.SetActive(false);
    }

    public void UpdateAllItemVisuals()
    {
        foreach (var cardObject in activeItemCards)
        {
            if (cardObject != null)
            {
                cardObject.GetComponent<ShopItemUI>()?.UpdateVisuals();
            }
        }
        manualSliderScroll.RecalculateCounts();
    }

}