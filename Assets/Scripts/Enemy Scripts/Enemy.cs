using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected const string WAS_HIT = "wasHit";

    [Header("Components")]
    protected Rigidbody2D enemyRB;
    protected Animator enemyAnimator;

    [Space]
    [Header("Movement:")]
    [SerializeField] protected float moveSpeed;
    protected bool canMove;

    [Space]
    [Header("Cooldown Timers:")]
    [SerializeField] protected float idleTime;
    protected float idleTimer;

    protected int facingDirection = -1;
    public bool isInvincible;

    [Space]
    [Header("Ground Check:")]
    [SerializeField] protected Transform groundCheckTransform;
    [SerializeField] protected float groundCheckDistance = 0.01f;
    [SerializeField] protected LayerMask whatIsGroundLayer;
    [SerializeField] protected bool isGrounded;
    [Space]
    [Header("Wall Check:")]
    [SerializeField] protected float wallCheckDistance = 0.01f;
    [SerializeField] protected Transform wallCheckTransform;
    [SerializeField] protected bool isWallDetected;

    protected virtual void Awake()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();

        canMove = true;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            PlayerController playerController = other.GetComponent<PlayerController>();

            playerController.KnockBack(transform);
        }
    }

    protected virtual void OnDrawGizmos()
    {
       Gizmos.DrawLine(groundCheckTransform.position, new Vector2(groundCheckTransform.position.x, groundCheckTransform.position.y - groundCheckDistance));
       Gizmos.DrawLine(wallCheckTransform.position, new Vector2(wallCheckTransform.position.x + wallCheckDistance * facingDirection, wallCheckTransform.position.y));
    }

    protected virtual void WalkAround()
    {
        if (idleTimer <= 0 && canMove) enemyRB.velocity = new Vector2(moveSpeed * facingDirection, enemyRB.velocity.y);
        else enemyRB.velocity = new Vector2(0f, enemyRB.velocity.y);

        idleTimer -= Time.deltaTime;

        if (isWallDetected || !isGrounded)
        {
            idleTimer = idleTime;
            Flip();
        }
    }

    protected virtual void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(groundCheckTransform.position, Vector2.down, groundCheckDistance, whatIsGroundLayer);
        isWallDetected = Physics2D.Raycast(wallCheckTransform.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGroundLayer);
    }

    protected virtual void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0, 180, 0);
    }

    public virtual void TakeDamage()
    {   
        //TODO: Play animation => Play particle FX
        if(!isInvincible) 
        {
            canMove = false;
            enemyAnimator.SetTrigger(WAS_HIT);
            AudioManager.AudioManagerInstance.PlayClip("EnemyDamaged");
        }
    }
    protected void DestroyAfterAnimation()
    {
        Destroy(gameObject);
    }
}   
