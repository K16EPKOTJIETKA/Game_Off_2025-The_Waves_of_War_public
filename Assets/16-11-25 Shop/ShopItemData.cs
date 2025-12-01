using UnityEngine;

[CreateAssetMenu(fileName = "NewShopItem", menuName = "Shop/Shop Item")]
public class ShopItemData : ScriptableObject
{
    public int itemID;
    public string itemName = "Default Name";
    [TextArea(3, 5)]
    public string description = "Default description of the item or upgrade.";
    public Sprite icon; // Для відображення "icon"
    public int price = 100;
}