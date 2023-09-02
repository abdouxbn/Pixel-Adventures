using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI currentFruitCount;

    private void Update()
    {
        timerText.text = $"Time: {GameManager.GameManagerInstance.timer.ToString("0.0")}s";
        currentFruitCount.text = PlayerManager.PlayerManagerInstance.fruitsCollected.ToString();
    }
}
