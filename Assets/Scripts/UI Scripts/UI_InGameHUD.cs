using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_InGameHUD : MonoBehaviour
{
    [Header("UI Menu GameObjects:")]
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject levelCompletedUI;


    [Space]
    [Header("HUD Elements:")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI currentFruitCount;

    [Space]
    [Header("Level Completed:")]
    [SerializeField] private TextMeshProUGUI yourTimeText;
    [SerializeField] private TextMeshProUGUI bestTimeText;
    [SerializeField] private TextMeshProUGUI fruitsCollectedText;

    private GameInput gameInput;
    private bool isGamePaused = false;

    private void Awake()
    {
        Time.timeScale = 1f;
        gameInput = new GameInput();
        gameInput.UI.Pause.performed += _ => OnPause();
    }

    private void OnEnable()
    {
        gameInput.UI.Enable();
    }

    private void OnDisable()
    {
        gameInput.UI.Disable();
    }

    private void Start()
    {
        GameManager.GameManagerInstance.levelNumber = SceneManager.GetActiveScene().buildIndex;
    }

    private void Update()
    {
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        timerText.text = $"Time: {GameManager.GameManagerInstance.timer}s";
        currentFruitCount.text = PlayerManager.PlayerManagerInstance.fruitsCollected.ToString();
    }

    public void ShowUIMenu(GameObject uiMenu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name != "IN_GAME_UI")
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        uiMenu.SetActive(true);
    }

    public void HideUIMenu(GameObject uiMenu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name != "IN_GAME_UI")
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        uiMenu.SetActive(false);

        if (Time.timeScale != 1f) Time.timeScale = 1f;
        if (isGamePaused) isGamePaused = false;
    }

    public void OnPause()
    {
        if (!isGamePaused)
        {
            isGamePaused = true;
            Time.timeScale = 0f;
            ShowUIMenu(pauseUI);
        }
        else
        {
            isGamePaused = false;
            Time.timeScale = 1f;
            HideUIMenu(pauseUI);
        }
    }

    public void GoToMainMenu() => SceneManager.LoadScene("MainMenu");
    public void RestartCurrentLevel() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.GameManagerInstance.startTime = false;
        GameManager.GameManagerInstance.timer = 0f;
        PlayerManager.PlayerManagerInstance.fruitsCollected = 0;
    }
    public void ToNextLevel()
    {
        if (Time.timeScale == 0f) Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    } 

    public void OnLevelCompleted()
    {
        fruitsCollectedText.text = PlayerManager.PlayerManagerInstance.fruitsCollected.ToString();
        yourTimeText.text = $"Time: {GameManager.GameManagerInstance.timer}s";
        bestTimeText.text = $"Best Time: {PlayerPrefs.GetFloat($"Level_{GameManager.GameManagerInstance.levelNumber}BestTime")}s";
        ShowUIMenu(levelCompletedUI);
    }
}
