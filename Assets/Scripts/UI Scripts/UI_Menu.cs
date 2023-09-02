using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Menu : MonoBehaviour
{
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
