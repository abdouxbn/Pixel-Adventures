using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Radish : Enemy
{
    [Space]
    [Header("Flying Controls:")]
    [SerializeField] private Transform groundAboveTransform;
    [SerializeField] private float groundAboveCheckDistance;
    private bool groundAboveDetected;
    [SerializeField] private Transform groundBelowTransform;
    [SerializeField] private float groundBelowCheckDistance;
    private RaycastHit2D groundBelowDetected;

    [Space]
    [Header("Aggressiveness:")]
    [SerializeField] private float aggressiveTime;
    private float aggressiveTimer;
    private bool isAggressive;

    [Space]
    [Header("Gravity Scale:")]
    [SerializeField] private float fallingGravityScale = 4f;
    private float defaultGravityScale;


    protected override void Awake()
    {
        base.Awake();

        defaultGravityScale = enemyRB.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {

        aggressiveTimer -= Time.deltaTime;

        if (aggressiveTimer <= 0f)
        {
            enemyRB.gravityScale = defaultGravityScale;
            isAggressive = false;
        }
        
        if (!isAggressive)
        {
            if (groundBelowDetected && !groundAboveDetected)
            {
                enemyRB.velocity = new Vector2(0f, 1f);
            }
        }
        else
        {   
            if (groundBelowDetected.distance <= 0.54f)
            {
                WalkAround();                
            }
        }

        CollisionChecks();
        enemyAnimator.SetFloat("xVelocity", enemyRB.velocity.x);
        enemyAnimator.SetBool("isAggressive", isAggressive);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(groundAboveTransform.position, new Vector2(groundAboveTransform.position.x, groundAboveTransform.position.y + groundAboveCheckDistance));
        Gizmos.DrawLine(groundBelowTransform.position, new Vector2(groundBelowTransform.position.x, groundBelowTransform.position.y - groundBelowCheckDistance));
    }

    protected override void CollisionChecks()
    {
        base.CollisionChecks();

        groundAboveDetected = Physics2D.Raycast(groundAboveTransform.position, Vector2.up, groundAboveCheckDistance, whatIsGroundLayer);
        groundBelowDetected = Physics2D.Raycast(groundBelowTransform.position, Vector2.down, groundBelowCheckDistance, whatIsGroundLayer);
    }

    public override void TakeDamage()
    {
        if (!isAggressive)
        {
            aggressiveTimer = aggressiveTime;
            enemyRB.gravityScale = fallingGravityScale;
            isAggressive = true;
        }
        else
        {
            base.TakeDamage();            
        }
    } 
}
