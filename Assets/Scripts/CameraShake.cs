using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
   [SerializeField] private CinemachineImpulseSource camImpulse;
   [SerializeField] private Vector3 impulseDirection;
   [SerializeField] private float impulseForce;

   public void ShakeScreen(int facingDirection)
   {
      camImpulse.m_DefaultVelocity = new Vector3(impulseDirection.x, facingDirection, impulseDirection.y) * impulseForce;
      camImpulse.GenerateImpulse();
   }
}
