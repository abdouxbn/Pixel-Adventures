using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrapSwitch : MonoBehaviour
{
    private const string IS_PRESSED_STRING = "isPressed";

    [SerializeField] private FireTrap fireTrap;
    private Animator fireTrapSwitchAnimator;

    private void Awake()
    {
        fireTrapSwitchAnimator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && fireTrap.FireSwitch())
        {
            fireTrap.FireSwitchResetAfter();
            fireTrapSwitchAnimator.SetTrigger(IS_PRESSED_STRING);
        }
    }
}
