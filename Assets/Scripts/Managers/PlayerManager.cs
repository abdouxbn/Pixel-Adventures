using UnityEngine;
using Cinemachine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager PlayerManagerInstance;

    [Header("Public Variables: -To Be Hidden-")]
    public int fruitsCollected;
    public int equippedSkinID;
    public Transform spawnPoint;
    public GameObject currentPlayer;

    [Space]
    [Header("Player: ")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerDeathFX;
    [SerializeField] private GameObject droppedFruitPrefab;
    [SerializeField] private float destroyDeathFxTime = 0.4f;
    [SerializeField] private float playerSpawnWaitTime = 1.0f;

    [Space]
    [Header("Camera Shake Settings:")]
    [SerializeField] private CinemachineImpulseSource camImpulse;
    [SerializeField] private Vector3 impulseDirection;
    [SerializeField] private float impulseForce;


    //! For Testing and Debugging purposes only! 

    //! ##################################### !\\

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (PlayerManagerInstance == null) PlayerManagerInstance = this;
        else Destroy(this.gameObject);
    }

    public void PlayerSpawn()
    {
        if (currentPlayer == null)
        {
            currentPlayer = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        }   
    }

    public void PlayerDeath()
    {
        GameObject newDeathFX = Instantiate(playerDeathFX, currentPlayer.transform.position, currentPlayer.transform.rotation);
        Destroy(newDeathFX, destroyDeathFxTime);
        Destroy(currentPlayer);
    }

    public void ShakeScreen(int facingDirection)
    {
        camImpulse.m_DefaultVelocity = new Vector3(impulseDirection.x, facingDirection, impulseDirection.y) * impulseForce;
        camImpulse.GenerateImpulse();
    }

    private void DropFruit()
    {
        fruitsCollected--;
        GameObject droppedFruit = Instantiate(droppedFruitPrefab, currentPlayer.transform.position, transform.rotation);
        Destroy(droppedFruit, 8.0f);
    }

    public void OnPlayerDamaged()
    {
        AudioManager.AudioManagerInstance.PlayClip("PlayerHit");
        if(fruitsCollected > 0) 
        {
            DropFruit();
            return;
        }
        PlayerDeath();
        Invoke("PlayerSpawn", playerSpawnWaitTime);
    }

    
    public void OnPlayerFall()
    {
        if(fruitsCollected > 0) 
        {
            DropFruit();
        }
        PlayerDeath();
        Invoke("PlayerSpawn", playerSpawnWaitTime);
    }
}
