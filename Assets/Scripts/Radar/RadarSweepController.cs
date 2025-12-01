using UnityEngine;

public class RadarSweepController : MonoBehaviour
{
    [SerializeField] ShopManager shopManager;
    [Header("Speed settings")]

    [SerializeField]
    private float rotationSpeedCoefficient = 1f;

    [SerializeField]
    private float fixedRotationRate = 180f;


    private float currentSweepSpeed => rotationSpeedCoefficient * fixedRotationRate;

    void Update()
    {

        float rotationAmount = currentSweepSpeed * Time.deltaTime;

        transform.Rotate(0f, rotationAmount, 0f);
    }

    void ImproveRadarSpeed(ShopItemData shopItem)
    {
        if (shopItem.itemID == 13)
        {
            rotationSpeedCoefficient = 2;
        }
    }

    private void OnEnable()
    {
        shopManager.productPurchased += ImproveRadarSpeed;
    }
    private void OnDisable()
    {
        shopManager.productPurchased -= ImproveRadarSpeed;
    }
}