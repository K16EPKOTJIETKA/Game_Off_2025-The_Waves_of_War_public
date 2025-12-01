using System;
using UnityEngine;
using Injection;

public class SatelliteTunerController : MonoBehaviour
{
    [Inject] Injector injector;
    [Inject] ShopManager shopManager;

    public bool canStartTuning = false;
    public bool isTuning = false;
    public bool isFinishing = false;
    public bool isFailing = false;
    public int correctTuning = 0;
    public int attemptCount = 0;
    int attempt;

    public event Action tuningStarted;
    public event Action tuningFinished;
    public event Action tuningFailed;

    public float tunerPointSizeKoef = 1;
  
    public void ChangeCanStartTuning(bool flag)
    {
        canStartTuning = flag;
    }
    public void OnTuningStarted()
    {
        attempt = attemptCount;
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
        if (correctTuning == 2)
        {
            OnTuningFinished();
        }
    }

    void ImproveAttemptCount(ShopItemData shopItem)
    {
        if (shopItem.itemID == 2 || shopItem.itemID == 3)
            attemptCount++;
    }

    void ImproveTunerPointSizeKoef(ShopItemData shopItem)
    {
        if (shopItem.itemID == 4)
            tunerPointSizeKoef += 0.2f;
        if (shopItem.itemID == 5)
            tunerPointSizeKoef += 0.4f;
    }

    public void Initialize()
    {
        injector.Inject(this);
    }

    private void OnEnable()
    {
        shopManager.productPurchased += ImproveAttemptCount;
        shopManager.productPurchased += ImproveTunerPointSizeKoef;
    }

    private void OnDisable()
    {
        shopManager.productPurchased -= ImproveAttemptCount;
        shopManager.productPurchased -= ImproveTunerPointSizeKoef;
    }
}
