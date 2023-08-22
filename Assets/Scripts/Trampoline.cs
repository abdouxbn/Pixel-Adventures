using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private float launchForce = 20f;
    [SerializeField] private bool canBeUsed = true;

    private void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.GetComponent<PlayerController>() != null && canBeUsed)
        {
            canBeUsed = false;
            GetComponent<Animator>().SetTrigger("launch");
            other.GetComponent<PlayerController>().PushedByTrampoline(launchForce);
        }
    }

    private void CanUseTrampolineAgain() => canBeUsed = true;
}
