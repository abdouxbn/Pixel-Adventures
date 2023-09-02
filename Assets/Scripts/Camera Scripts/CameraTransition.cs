using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.PlayerLoop;

public class CameraTransition : MonoBehaviour
{
    [Header("Settings:")]
    [SerializeField] private GameObject vCam;
    [SerializeField] private PolygonCollider2D vcamCollider;
    [SerializeField] private Color camGizmosColour; 

    private void Start()
    {
        vCam.GetComponentInChildren<CinemachineVirtualCamera>().Follow = PlayerManager.PlayerManagerInstance.currentPlayer.transform;
    }

    private void Update()
    {
        if (vCam.GetComponent<CinemachineVirtualCamera>().Follow == null)
        {
            vCam.GetComponentInChildren<CinemachineVirtualCamera>().Follow = PlayerManager.PlayerManagerInstance.currentPlayer.transform;
        }
    }

   private void OnTriggerEnter2D(Collider2D other)
   {
        if (other.GetComponent<PlayerController>() != null)
        {
            vCam.SetActive(true);
        }
   }

   private void OnTriggerExit2D(Collider2D other)
   {
        if (other.GetComponent<PlayerController>() != null)
        {
            vCam.SetActive(false);
        }
   }

   private void OnDrawGizmos()
   {
        Gizmos.color = camGizmosColour;
        Gizmos.DrawWireCube(vcamCollider.bounds.center, vcamCollider.bounds.size);
   }
}