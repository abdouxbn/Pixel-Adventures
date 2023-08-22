using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Plant : Enemy
{
    [Space]
    [Header("Player Detection:")]
    [SerializeField] private Transform playerCheckTransform;
    [SerializeField] private float playerCheckDistance;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private bool isFacingRight;
    private RaycastHit2D isPlayerDetected;

    [Space]
    [Header("Plant Bullet:")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletShootPosition;
    [Space]
    [Header("Bullet Speed:")]
    [SerializeField] private float xBulletSpeed;

    protected override void Awake()
    {
        base.Awake();
        
        if (isFacingRight)
        {
            Flip();
        }
    }

    private void Update()
    {
        isPlayerDetected = Physics2D.Raycast(playerCheckTransform.position, Vector2.right * facingDirection, playerCheckDistance, playerLayerMask);

        idleTimer -= Time.deltaTime;

        if (idleTimer < 0f && isPlayerDetected)
        {
            idleTimer = idleTime;
            enemyAnimator.SetTrigger("attack");
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(playerCheckTransform.position, new Vector2(playerCheckTransform.position.x + playerCheckDistance * facingDirection, playerCheckTransform.position.y));
    }

    public void Attack()
    {   GameObject newBullet = Instantiate(bulletPrefab, bulletShootPosition.position, bulletShootPosition.rotation);

        newBullet.GetComponent<Enemy_Plant_Bullet>().SetupBulletSpeed(xBulletSpeed * facingDirection, 0f);
    }
}
