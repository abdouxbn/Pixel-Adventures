using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private Color deathZoneColor;
    [SerializeField] private BoxCollider2D deathZoneCollider;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            Destroy(other.gameObject);
        }
    }

    private void OnDrawGizmos()
   {
        Gizmos.color = deathZoneColor;
        Gizmos.DrawWireCube(deathZoneCollider.bounds.center, deathZoneCollider.bounds.size);
   }
}
