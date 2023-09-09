using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject levelButton;
    [SerializeField] private Transform levelButtonParent;

    [SerializeField] private bool[] unlockedLevel;

    private void Start()
    {   
        PlayerPrefs.SetInt($"Level_{1}Unlocked", 1);

        UpdateUnlockedLevels();

        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            if (!unlockedLevel[i]) return;

            string levelName = $"LEVEL_0{i}";
            GameObject newButton = Instantiate(levelButton, levelButtonParent);
            newButton.GetComponent<Button>().onClick.AddListener(() => LoadScene(levelName));
            newButton.GetComponentInChildren<TMP_Text>().text = levelName;
        }
    }

    public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);

    private void UpdateUnlockedLevels()
    {
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            bool unlocked = PlayerPrefs.GetInt($"Level_{i}Unlocked") == 1;

            if (unlocked) unlockedLevel[i] = true;
            else return;
        }
    }

}
