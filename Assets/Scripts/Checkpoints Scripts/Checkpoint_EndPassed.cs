using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint_EndPassed : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            GetComponent<Animator>().SetTrigger("passed");
            Debug.Log("LEVEL COMPLETED!");
            GameManager.GameManagerInstance.startTime = false;

            GameManager.GameManagerInstance.SaveBestTime();
            GameManager.GameManagerInstance.SaveCollectedFruit();
            GameManager.GameManagerInstance.SaveLevelInfo();
        }
    }
}
