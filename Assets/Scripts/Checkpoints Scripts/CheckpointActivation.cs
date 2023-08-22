using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointActivation : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            GetComponent<Animator>().SetTrigger("activated");
            PlayerManager.PlayerManagerInstance.spawnPoint = transform;
        }
    }
}
