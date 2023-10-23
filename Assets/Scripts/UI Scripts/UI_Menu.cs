using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Menu : MonoBehaviour
{
    [SerializeField] private UI_VolumeControl[] volumeControl; 

    private void Start()
    {
        for (int i = 0; i < volumeControl.Length; i++)
        {
            volumeControl[i].GetComponent<UI_VolumeControl>().SetGameVolume();
        }
    }

    public void ChangeMenu(GameObject uiMenu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name != "PIXEL_ADVENTURES")
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        uiMenu.SetActive(true);
    }

    public void ExitGame()
    {
        //* If in Unity Editor
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif

        //* If not in Unity Editor run this 
        Application.Quit();
    }
}
