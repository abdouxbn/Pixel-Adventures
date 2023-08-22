using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Ghost : Enemy
{
    [Space]
    [Header("Ghost:")]
    [SerializeField] private float activeTime = 4.0f;
    private float activeTimer;
    private bool isAggressive;
    [SerializeField] private int[] xOffsetArray;

    private Transform player;
    private SpriteRenderer ghostSpriteRenderer;

    protected override void Awake()
    {
        base.Awake();

        player = PlayerManager.PlayerManagerInstance.currentPlayer.transform;
        ghostSpriteRenderer = GetComponent<SpriteRenderer>();

        isAggressive = true;
        isInvincible = true;
    }

    private void Update()
    {
        if (PlayerManager.PlayerManagerInstance.currentPlayer == null)
        {
            enemyAnimator.SetTrigger("disappear");
            return;
        }

        activeTimer -= Time.deltaTime;
        idleTimer -= Time.deltaTime;

        if (activeTimer > 0F)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }

        if (activeTimer < 0f && idleTimer < 0f && isAggressive)
        {
            enemyAnimator.SetTrigger("disappear");
            isAggressive = false;
            idleTimer = idleTime;
        }

        if (activeTimer < 0f && idleTimer < 0f && !isAggressive)
        {
            RandomAppearPosition();
            enemyAnimator.SetTrigger("appear");
            isAggressive = true;
            activeTimer = activeTime;
        }

        GhostFlip();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (isAggressive)
        {
            base.OnTriggerEnter2D(other);            
        }
    }

    protected override void OnDrawGizmos()
    {
        if (groundCheckTransform != null && wallCheckTransform != null)
        {            
            base.OnDrawGizmos();
        }
    }

    private void RandomAppearPosition()
    {
        float xOffset = UnityEngine.Random.Range(0,xOffsetArray.Length);
        float yOffset = UnityEngine.Random.Range(-6, 6);
        transform.position = new Vector2(player.position.x + xOffsetArray[(int)xOffset], player.position.y + yOffset);
    }

    public void Appear()
    {
        ghostSpriteRenderer.enabled = true;
    }

    public void Disappear()
    {
        ghostSpriteRenderer.enabled = false;
    }

    private void GhostFlip()
    {
        if (PlayerManager.PlayerManagerInstance.currentPlayer == null)
        {
            return;
        }

        if (facingDirection == -1 && transform.position.x < player.position.x)
        {
            Flip();
        }
        if (facingDirection == 1 && transform.position.x > player.position.x)
        {
            Flip();
        }
    }

}
