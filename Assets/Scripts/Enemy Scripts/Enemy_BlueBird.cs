using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BlueBird : Enemy
{
    [Space]
    [Header("Flying Controls:")]
    [SerializeField] private float groundAboveCheckDistance;
    private bool groundAboveDetected;
    [SerializeField] private float flyUpForce;
    [SerializeField] private float flyDownForce;
    private float flyForce;    
    private bool canFly;

    protected override void Awake()
    {
        base.Awake();
        canFly = true;
    }

    private void Update()
    {
        CollisionChecks();

        if (groundAboveDetected && !isGrounded) flyForce = flyDownForce;
        else if (isGrounded && !groundAboveDetected) flyForce = flyUpForce;

        if(isWallDetected) Flip();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + groundAboveCheckDistance));
    }

    protected override void CollisionChecks()
    {
        base.CollisionChecks();

        groundAboveDetected = Physics2D.Raycast(transform.position, Vector2.up, groundAboveCheckDistance, whatIsGroundLayer);
    }

    public void FlyUp()
    {
        if(canFly) enemyRB.velocity = new Vector2(moveSpeed * facingDirection, flyForce);
    }

    public override void TakeDamage()
    {
        canFly = false;
        enemyRB.gravityScale = 5f;
        base.TakeDamage();
    }
}
