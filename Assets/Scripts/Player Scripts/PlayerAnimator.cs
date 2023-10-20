using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    #region CONSTANTS
    private const string IS_MOVING = "isMoving";
    private const string IS_GROUNDED = "isGrounded";
    private const string Y_VELOCITY = "yVelocity";
    private const string IS_TOUCHING_WALL = "isTouchingWall";
    private const string IS_WALL_SLIDING = "isWallSliding";
    private const string IS_KNOCKED = "isKnocked";
    private const string IS_PLAYER_CONTROLLABLE = "isPlayerControllable";
    #endregion

    #region COMPONENTS
    private Animator playerAnimator;
    private SpriteRenderer playerSpriteRenderer;
    #endregion

    [SerializeField] private ParticleSystem dustParticleFx;

    private bool isFacingRight = true;
    private int facingDirection = 1; 
    public static int FacingDirection {get; private set;}


    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();

        SetAnimationLayer();
    }

    private void Update()
    {
        playerAnimator.SetBool(IS_PLAYER_CONTROLLABLE, PlayerController.IsPlayerControllable);
        playerAnimator.SetBool(IS_KNOCKED, PlayerController.IsKnocked);
        playerAnimator.SetBool(IS_MOVING, IsPlayerMoving());
        playerAnimator.SetBool(IS_GROUNDED, IsPlayerGrounded());
        playerAnimator.SetFloat(Y_VELOCITY, PlayerController.PlayerVelocity.y);
        playerAnimator.SetBool(IS_TOUCHING_WALL, PlayerController.IsPlayerTouchingWall);
        playerAnimator.SetBool(IS_WALL_SLIDING, PlayerController.IsPlayerWallSliding);
        
        if(PlayerController.IsKnocked)
        {
            return;
        }
        FlipPlayerSprite();
        FacingDirection = facingDirection;
    }

    private bool IsPlayerMoving()
    {
        if (Mathf.Abs(PlayerController.PlayerVelocity.x) > 0.1f)
        {
            return true;
        }
        return false;
    }
    private bool IsPlayerGrounded()
    {
        if (PlayerController.IsPlayerGrounded)
        {
            return true;
        }
        return false;
    }
    private void FlipPlayerSprite()
    {
        if (isFacingRight && PlayerController.PlayerVelocity.x < 0f)
        {
            Flip(true);  
        }
        if (!isFacingRight && PlayerController.PlayerVelocity.x > 0f)
        {
            Flip(false);
        }
    }
    private void Flip(bool flip)
    {
        if (IsPlayerGrounded()) dustParticleFx.Play();    
        dustParticleFx.Play();
        facingDirection *= -1;
        isFacingRight = !isFacingRight;
        playerSpriteRenderer.flipX = flip;
    }

    private void SetAnimationLayer()
    {
        int skinID = PlayerManager.PlayerManagerInstance.equippedSkinID;

        for (int i = 0; i < playerAnimator.layerCount; i++)
        {
            playerAnimator.SetLayerWeight(i, 0);
        }

        playerAnimator.SetLayerWeight(skinID, 1);
    }

}
