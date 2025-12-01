using UnityEngine;
using TMPro;
using Injection;

public class ScoreUI : MonoBehaviour
{
    [Inject] BalanceSystem system;
    public TMP_Text allyText;
    public TMP_Text enemyText;

    public void Init()
    {
        system.OnPointsChanged += UpdateScores;
        UpdateScores(system.allyPoints, system.enemyPoints);
    }

    void UpdateScores(int ally, int enemy)
    {
        if (allyText != null)
            allyText.text = ally.ToString();

        if (enemyText != null)
            enemyText.text = enemy.ToString();
    }
}

