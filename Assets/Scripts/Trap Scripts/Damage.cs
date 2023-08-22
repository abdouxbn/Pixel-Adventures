using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();

            playerController.KnockBack(transform);
        }
    }
}
