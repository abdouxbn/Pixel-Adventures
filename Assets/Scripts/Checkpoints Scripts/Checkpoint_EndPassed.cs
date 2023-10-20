using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint_EndPassed : MonoBehaviour
{
    private UI_InGameHUD inGameUI;

    private void Start()
    {
        inGameUI = GameObject.Find("Canvas").GetComponent<UI_InGameHUD>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            GetComponent<Animator>().SetTrigger("passed");
            GameManager.GameManagerInstance.startTime = false;
            GameManager.GameManagerInstance.SaveBestTime();
            GameManager.GameManagerInstance.SaveCollectedFruit();
            GameManager.GameManagerInstance.SaveLevelInfo();
            inGameUI.OnLevelCompleted();
        }
    }
}
