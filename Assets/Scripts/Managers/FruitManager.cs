using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitManager : MonoBehaviour
{
    [SerializeField] private Transform[] fruitPosition;
    [SerializeField] private GameObject fruitPrefab;

    private void Awake()
    {
        fruitPosition = GetComponentsInChildren<Transform>();

        for (int i = 1; i < fruitPosition.Length; i++)
        {   
            GameObject newFruit = Instantiate(fruitPrefab, fruitPosition[i]);
        }

        int levelNumber = GameManager.GameManagerInstance.levelNumber;
        int totalAmountOfFruits = PlayerPrefs.GetInt($"Level_{levelNumber}TotalFruits");

        if (totalAmountOfFruits != fruitPosition.Length - 1)
        {
            PlayerPrefs.SetInt($"Level_{levelNumber}TotalFruits", fruitPosition.Length - 1);
        }
    } 
}
