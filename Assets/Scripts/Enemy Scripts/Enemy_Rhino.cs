using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Rhino : Enemy
{
    private const string X_VELOCITY = "xVelocity";
    protected const string IS_INVINCIBLE = "isInvincible";
    
    [Space]
    [Header("Movement _ Chase Player")]
    [SerializeField] private float pursuitSpeed;

    [Space]
    [Header("Shocked Time:")]
    [SerializeField] private float shockTime;
    private float shockTimer;

    [Space]
    [Header("Player Detection:")]
    [SerializeField] private Transform playerCheckTransform;
    [SerializeField] private float playerCheckDistance;
    [SerializeField] private LayerMask playerLayerMask;
    private bool isPlayerDetected;
    private bool isAgressive;

    protected override void Awake()
    {
        base.Awake();
        isInvincible = true;
    }

    // Update is called once per frame
    private void Update()
    {   
        CollisionChecks();

        isPlayerDetected = Physics2D.Raycast(playerCheckTransform.position, Vector2.right * facingDirection, playerCheckDistance, playerLayerMask);

        if(isPlayerDetected) isAgressive = true;

        if (!isAgressive)
        {   
            // i.e. Normal behaviour, Just patrolling
            WalkAround();
        }
        else
        {
            // i.e. Agressive behaviour, Charges toward the player until he hits a wall
            enemyRB.velocity = new Vector2(pursuitSpeed * facingDirection, enemyRB.velocity.y);

            if (isWallDetected && isInvincible)
            {
                isInvincible = false;
                shockTimer = shockTime;
            }

            if (shockTimer <= 0f && !isInvincible)
            {
                isInvincible = true;
                Flip();
                isAgressive = false;
            }
            
            shockTimer -= Time.deltaTime;
        }

        enemyAnimator.SetFloat(X_VELOCITY, enemyRB.velocity.x);
        enemyAnimator.SetBool(IS_INVINCIBLE, isInvincible);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(playerCheckTransform.position, new Vector2(playerCheckTransform.position.x + playerCheckDistance * facingDirection, playerCheckTransform.position.y));
    }
}
