using UnityEngine;
using Injection;
using System.Collections.Generic;

public class ShopBootstrap : MonoBehaviour
{

    [SerializeField] ShopManager shopManager;
    ShopItemUI[] shopItems;
    [Inject] Injector injector;

    public void Initialize()
    {
        //shopManager.Init();
          
    }

    public void InjectShopItems()
    {
        var shopItems = FindObjectsByType<ShopItemUI>(FindObjectsSortMode.None);

        foreach (var shopItem in shopItems)
        {
            injector.Inject(shopItem);
        }
    }

}
