using Injection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    [Inject] ManualSliderScroll manualSliderScroll;
    [Inject] ShopManager shopManager;
    [Inject] CurrencyManager currencyManager;
    [Inject] Injector injector;

    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public Button buyButton;
    public Image background; 

    private ShopItemData itemData;
    AudioSource audioSource;

    void PlaySound()
    {
        if (audioSource == null) return;
        audioSource.Play();
    }
    public void Setup(ShopItemData data)
    {
        itemData = data;

        iconImage.sprite = data.icon;
        nameText.text = data.itemName + " - " + data.price;
        descriptionText.text = data.description;

        audioSource = GetComponentInParent<AudioSource>();
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(OnBuyButtonClicked);

        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        bool canAfford = currencyManager.currentBalance >= itemData.price;

        if (canAfford)
        {
            SetTransparency(iconImage, 1f);
            SetTransparency(nameText, 1f);
            SetTransparency(descriptionText, 1f);
           // SetTransparency(background, 1f);
            SetTransparency(buyButton.GetComponent<Image>(), 1f);
            buyButton.interactable = true; 
        }
        else
        {
            // Напівпрозорі
            SetTransparency(iconImage, 0.5f);
            SetTransparency(nameText, 0.5f);
            SetTransparency(descriptionText, 0.5f);
           // SetTransparency(background, 0.5f);
            SetTransparency(buyButton.GetComponent<Image>(), 0.5f);
            buyButton.interactable = false; 
        }
    }

    private void SetTransparency(Graphic g, float alpha)
    {
        Color c = g.color;
        c.a = alpha;
        g.color = c;
    }

    private void OnBuyButtonClicked()
    {
        if (currencyManager.TryPurchase(itemData.price))
        {
            PlaySound();
            ApplyUpgrade(itemData);
            Destroy(gameObject);           
        }   
    }

    private void OnDestroy()
    {
        manualSliderScroll.RecalculateCounts();
    }

    private void ApplyUpgrade(ShopItemData data)
    {
        shopManager.OnProductPurchased(data);
        Debug.Log($"Apply upgrade: {data.itemName}");
        
    }
}