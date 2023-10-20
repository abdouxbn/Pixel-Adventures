using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GameManagerInstance;

    [Header("Timer:")]
    public float timer;
    public bool startTime;

    [Space]
    [Header("Level:")]
    public int levelNumber;

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (GameManagerInstance == null) GameManagerInstance = this;
        else Destroy(this.gameObject);
    }

    private void Update()
    {
        if (startTime) timer += Time.deltaTime;
    }

    public void SaveBestTime()
    {
        startTime = false;
        float lastTime = PlayerPrefs.GetFloat($"Level_{levelNumber}BestTime", 0f);
        
        if (lastTime == 0f || timer < lastTime)
        {
            PlayerPrefs.SetFloat($"Level_{levelNumber}BestTime", timer);
        }
    }

    public void SaveCollectedFruit()
    {
        int totalFruits = PlayerPrefs.GetInt("TotalFruitsCollected");
        int newTotalFruits = totalFruits + PlayerManager.PlayerManagerInstance.fruitsCollected;

        PlayerPrefs.SetInt("TotalFruitsCollected", newTotalFruits);
        PlayerPrefs.SetInt($"Level_{levelNumber}FruitsCollected", PlayerManager.PlayerManagerInstance.fruitsCollected);
    }

    public void SaveLevelInfo()
    {
        int nextLevelNumber = levelNumber + 1;
        PlayerPrefs.SetInt($"Level_{nextLevelNumber}Unlocked", 1);
    }
}
