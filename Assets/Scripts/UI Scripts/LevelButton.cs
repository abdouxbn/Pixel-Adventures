using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelName;
    [SerializeField] private TextMeshProUGUI bestTime;
    [SerializeField] private TextMeshProUGUI fruitsCollected;
    [SerializeField] private TextMeshProUGUI totalLevelFruits;

    public void UpdateLevelIconInfo(int levelNumber)
    {
        levelName.text = $"LEVEL_{levelNumber}";
        bestTime.text = $"Best Time: {PlayerPrefs.GetFloat($"Level_{levelNumber}BestTime")}";
        fruitsCollected.text = $"Fruits: {PlayerPrefs.GetInt($"Level_{levelNumber}FruitsCollected")}/";
        totalLevelFruits.text = PlayerPrefs.GetInt($"Level_{levelNumber}TotalFruits").ToString();
    }
}
