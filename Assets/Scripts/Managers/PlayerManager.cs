using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager PlayerManagerInstance;

    public int fruitsCollected;
    public int equippedSkinID;


    [SerializeField] private GameObject playerPrefab;
    public Transform spawnPoint;
    public GameObject currentPlayer;


    //! For Testing and Debugging purposes only! 

    //! ##################################### !\\

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (PlayerManagerInstance == null) PlayerManagerInstance = this;
        else Destroy(this.gameObject);
    }

    public void SpawnPlayer()
    {
        if (currentPlayer == null)
        {
            currentPlayer = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        }   
    }
}
