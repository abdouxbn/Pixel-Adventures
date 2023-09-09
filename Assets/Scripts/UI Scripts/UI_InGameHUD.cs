using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_InGameHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI currentFruitCount;

    private void Start()
    {
        GameManager.GameManagerInstance.levelNumber = SceneManager.GetActiveScene().buildIndex;
    }

    private void Update()
    {
        timerText.text = $"Time: {GameManager.GameManagerInstance.timer.ToString("0.0")}s";
        currentFruitCount.text = PlayerManager.PlayerManagerInstance.fruitsCollected.ToString();
    }
}
