using Injection;
using System;
using UnityEngine;

public class LaserDeviceController : MonoBehaviour
{
    public bool deviceIsOn = true;

    [Inject] Injector injector;
    [Inject] SignalManager signalManager;
    [Inject] ShopManager shopManager;

    public bool canStartTuning = false;
    public bool isTuning = false;
    public bool isFinishing = false;
    public bool isFailing = false;
    public int correctTuning = 0;
    public float tunerPointSizeKoef = 1;

    int attemtCount = 0;
    int attempt;

    public event Action tuningStarted;
    public event Action tuningFinished;
    public event Action tuningFailed;
    public event Action deviceWasOn;



    public void ChangeCanStartTuning(bool flag)
    {
        if (!deviceIsOn)
        {
            deviceIsOn = false;
            return;
        } 
        canStartTuning = flag;
    }

    public void DeviceOn(ShopItemData shopItem)
    {
        if (shopItem.itemID != 0) return;
        deviceIsOn = true;
        deviceWasOn?.Invoke();
    }

    public void OnTuningStarted()
    {
        attempt = attemtCount;
        tuningStarted?.Invoke();
        isTuning = true;
    }
    private void OnTuningFinished()
    {
        tuningFinished?.Invoke();
        isTuning = false;
        isFinishing = true;
        correctTuning = 0;
    }
    public void OnTuningFailed()
    {
        if (attempt > 0)
        {
            attempt--;
            return;
        }
        tuningFailed?.Invoke();
        isTuning = false;
        isFailing = true;
        correctTuning = 0;
    }

    public void CorrectTuning()
    {
        correctTuning++;
        if (correctTuning == 4)
        {
            OnTuningFinished();
        }
    }

    void AddAttempt(ShopItemData shopItem)
    {
        if (shopItem.itemID > 13 && shopItem.itemID < 18)
        {
            attemtCount++;
        }
    }

    void ImproveTunerPointSizeKoef(ShopItemData shopItem)
    {
        if (shopItem.itemID == 18)
        {
            tunerPointSizeKoef += 0.2f;
        }
        if (shopItem.itemID == 19)
        {
            tunerPointSizeKoef += 0.4f;
        }
    }


    public void Initialize()
    {
        injector.Inject(this);
    }

    private void OnEnable()
    {
        shopManager.productPurchased += DeviceOn;
        shopManager.productPurchased += AddAttempt;
        shopManager.productPurchased += ImproveTunerPointSizeKoef;
    }

    private void OnDisable()
    {
        shopManager.productPurchased -= DeviceOn;
        shopManager.productPurchased -= AddAttempt;
        shopManager.productPurchased -= ImproveTunerPointSizeKoef;
    }

}
