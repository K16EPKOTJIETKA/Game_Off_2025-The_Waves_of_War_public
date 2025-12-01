using Injection;
using UnityEngine;
using UnityEngine.UI;

public class BalanceBarUI : MonoBehaviour
{
    [Inject] BalanceSystem system;
    public Image allyFill;
    public Image enemyFill;

    public void Init()
    {
        system.OnBalanceChanged += UpdateBar;
        UpdateBar(system.allyPoints / (float)system.totalPoints);
    }

    void UpdateBar(float percent)
    {
        allyFill.fillAmount = percent;
        enemyFill.fillAmount = 1f - percent;
    }
}
