using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager PlayerManagerInstance;

    public int fruitsCollected;

    //! For Testing and Debugging purposes only! 
    private GameInput gameInputForTest;

    [SerializeField] private GameObject playerPrefab;
    public Transform spawnPoint;
    public GameObject currentPlayer;

    private void Awake()
    {
        if (PlayerManagerInstance == null)
        {
            PlayerManagerInstance = this;
        }

        gameInputForTest = new GameInput();
        gameInputForTest.Testing.Respawn.performed += _ => SpawnPlayer();
    }

    private void OnEnable()
    {
        gameInputForTest.Testing.Enable();
    }

    private void OnDisable()
    {
        gameInputForTest.Testing.Disable();
    }

    public void SpawnPlayer()
    {
        if (currentPlayer == null)
        {
            currentPlayer = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        }   
    }
}
