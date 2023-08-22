using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            PlayerManager.PlayerManagerInstance.fruitsCollected++;
            Destroy(gameObject);
        }
    }
}
