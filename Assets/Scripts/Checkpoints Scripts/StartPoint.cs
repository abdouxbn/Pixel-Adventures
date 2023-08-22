using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    [SerializeField] private Transform actualSpawnPoint;

    private void Awake()
    {
        PlayerManager.PlayerManagerInstance.spawnPoint = actualSpawnPoint;
        PlayerManager.PlayerManagerInstance.SpawnPlayer();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {   
            if (other.transform.position.x > transform.position.x)
            {
                GetComponent<Animator>().SetTrigger("touched");            
            }
        }
    }
}
