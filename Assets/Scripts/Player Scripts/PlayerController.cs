using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region COMPONENTS
    [Header("Components")]
    private Rigidbody2D playerRB;
    //private CapsuleCollider2D playerCollider;
    private GameInput gameInput; 
    #endregion

    #region INPUT
    [Header("Input")]
    private float moveInput;
    private float verticalInput;
    #endregion

    #region MOVEMENT
    [Header("Movement")]
    [SerializeField] private float moveMaxSpeed = 7.0f;
    [SerializeField] private float velocityPower;
    [SerializeField] private float moveAcceleration;
    [SerializeField] private float moveDecceleration;
    [Range(0.1f, 1.0f)] [SerializeField] private float wallSlideFallVelocityPercent = 0.35f;
    //[Range(0.40f, 0.65f)] [SerializeField] private float wallSlideFallVelocityAcelerator = 0.45f;
    private bool isPlayerControllable = false;
    private bool canMove;
    private float acceleration;
    private float deceleration;
    private float finalMovementForce;
    #endregion

    [Space]
    [Header("Particle Effects: ")]
    [SerializeField] private ParticleSystem dustParticleFx;

    #region KNOCKBACK
    [Space]
    [Header("Knockback")]
    [SerializeField] private Vector2 knockbackDirection;
    [SerializeField] private float knockbackForce;
    [SerializeField] private float knockbackDuration;
    [SerializeField] private float invincibilityDuration;
    private bool isKnocked;
    private bool canBeKnocked = true; 
    #endregion

    #region JUMPING
    [Space]
    [Header("Jumping")]
    [SerializeField] private float firstJumpForce = 5.0f;
    [SerializeField] private float doubleJumpForce = 5.0f;
    [SerializeField] private float wallJumpForce = 5.0f;
    [SerializeField] private Vector2 wallJumpDirection;
    //[SerializeField] private bool isAirborne = false;
    [SerializeField] private float bufferJumpTime;
    [SerializeField] private float coyoteJumpTime;
    private float bufferJumpTimer;
    private float coyoteJumpTimer;
    private bool canCoyoteJump;
    private int jumpsRemaining = 2;
    private bool canDoubleJump; 

    //* Unlock Abilities 
    //? SerializeField to use in inspector? To test? 
    //! Serialized for debugging and testing!
    [SerializeField] private bool doubleJumpAbilityUnlocked = false;

    //TODO: Add jumping modifiers to get a better jump, such as going up gravity and falling gravity
    #endregion

    #region COLLISIONS_CHECKS

    [Space(2)]
    [Header("Collisions Check")]

    [Space]
    [Header("Ground:")]
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private Vector2 boxCastSize = new Vector2(0.3f, 0.05f);
    [SerializeField] private float groundCheckDistance = 0.01f;
    [SerializeField] private LayerMask whatIsGroundLayer;
    [SerializeField] private bool isGrounded;

    [Space]
    [Header("Walls:")]
    [SerializeField] private LayerMask whatIsWallLayer;
    [SerializeField] private Transform rightWallCheckTransform;
    [SerializeField] private Transform leftWallCheckTransform;
    [SerializeField] private float circleRadius = 0.5f;
    [SerializeField] private float circleDistance = 0.0f;
    [SerializeField] private bool isTouchingWall;
    [SerializeField] private bool isTouchingRightWall;
    [SerializeField] private bool isTouchingLeftWall;
    [SerializeField] private bool canWallSlide;
    [SerializeField] private bool isWallSliding;

    [Space]
    [Header("Enemies:")]
    [SerializeField] private Transform enemiesHitCheckTransform;
    [SerializeField] private Vector2 enemiesBoxCastSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private float enemiesCheckDistance = 0.01f;
    [SerializeField] private float afterKillHopForce = 12.0f;
    [SerializeField] private LayerMask enemiesLayerMask;
    #endregion

    #region STATIC_PROPERTIES
    public static float MoveInput {get; private set;}
    public static Vector2 PlayerVelocity {get; private set;}
    public static bool IsPlayerGrounded {get; private set;}
    public static bool IsPlayerTouchingWall {get; private set;}
    public static bool IsPlayerWallSliding {get; private set;}
    public static bool IsKnocked{get; private set;}
    public static bool IsPlayerControllable {get; private set;}
    #endregion


    //! Debug Section // To be deleted when done!
    
    //! \\#####################################//
    
    //*  ################################################################################################################################################
    //*  ############################################################### Unity Functions ################################################################
    //*  ################################################################################################################################################

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        //playerCollider = GetComponent<CapsuleCollider2D>();

        gameInput = new GameInput();
        // gameInput.Player.Move.performed += ctx => moveInput = ctx.ReadValue<float>();
        // gameInput.Player.Vertical.performed += ctx => verticalInput = ctx.ReadValue<float>();
        gameInput.Player.Jump.performed += _ => OnJumpPerformed();

        //* Calculate run acceleration & deceleration forces using formula: amount = ((1 / Time.fixedDeltaTime) * acceleration) / runMaxSpeed //
        acceleration = (50 * moveAcceleration) / moveMaxSpeed;
        deceleration = (50 * moveDecceleration) / moveMaxSpeed;
        
        acceleration = Mathf.Clamp(acceleration, 0f, moveMaxSpeed);
        deceleration = Mathf.Clamp(deceleration, 0f, moveMaxSpeed);
    }

    //* There was an OnValidate() function here.
    

    private void OnEnable()
    {
        gameInput.Player.Enable();
    }

    private void Update()
    {
        // Input 
        MovementInput();
        
        // Collision checks
        IsTouchingWall();
        CheckIfCanWallSlide();
        CheckIfIsWallSliding();

        EnemiesHitBoxCastCheck();
        //
        CancelWallSlide();

        //? Put in JUMP_TIMERS REGION? RENAME REGION? 
        CoyoteJump();

        #region JUMP_TIMERS

        bufferJumpTimer -= Time.deltaTime;
        coyoteJumpTimer -= Time.deltaTime;

        #endregion
        
        #region AIRBORNE_CHECK
        /* Check if isAirborne
        if (!isGrounded && !isTouchingWall)
        {
            isAirborne = true;
        }
        if (isGrounded || isTouchingWall)
        {
            isAirborne = false;
        }*/
        #endregion
        
        // Setting properties for PlayerAnimator script
        AnimationPropertieSetter();
    }


    private void FixedUpdate()
    {
        DoMovement();
        AddFriction();
        DoWallSlide();
        BufferJump();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            dustParticleFx.Play();
            jumpsRemaining = 2;
        }
    }

    private void OnDisable()
    {
        gameInput.Player.Move.performed -= ctx => moveInput = ctx.ReadValue<float>();
        gameInput.Player.Vertical.performed -= ctx => verticalInput = ctx.ReadValue<float>();
        gameInput.Player.Jump.performed -= _ => OnJumpPerformed();
        gameInput.Player.Disable();
    }

    private void OnDrawGizmos()
    {
        // Define the parameters for the CircleCast
        float radius = circleRadius; // Set the radius of the circle

        // Calculate the positions for the CircleCasts on both sides of the player
        Vector2 leftPosition = rightWallCheckTransform.position + (Vector3)Vector2.left * (radius);
        Vector2 rightPosition = leftWallCheckTransform.position + (Vector3)Vector2.right * (radius);

        Color circleColor;
        if (isTouchingWall || isGrounded){
            circleColor = Color.green;
        } 
        else{
            circleColor = Color.red;
        }
        Gizmos.color = circleColor;

        // Draw the CircleCasts using Debug.DrawWireSphere
        Gizmos.DrawWireSphere(rightWallCheckTransform.position, radius);
        Gizmos.DrawWireSphere(leftWallCheckTransform.position, radius);
        Gizmos.DrawWireCube(groundCheckTransform.position + new Vector3(0, -groundCheckDistance, 0), boxCastSize);
        Gizmos.DrawWireCube(enemiesHitCheckTransform.position + new Vector3(0f, -enemiesCheckDistance, 0f), enemiesBoxCastSize);
    }

    //*  ################################################################################################################################################
    //*  ############################################################### Custom Functions ###############################################################
    //*  ################################################################################################################################################

    private void MovementInput()
    {
        if(!isPlayerControllable) return;
        moveInput = gameInput.Player.Move.ReadValue<float>();
        verticalInput = gameInput.Player.Vertical.ReadValue<float>();
    }

    public void PlayerIsControllable()
    {
        isPlayerControllable = true;
    }

    private void DoMovement()
    {   
        //TODO: Movement in the air modifiers
        if (canMove && moveInput != 0f && !isKnocked)
        {
            float targetVelocityX = moveInput * moveMaxSpeed;
            float speedDifference = targetVelocityX - playerRB.velocity.x;
            float accelerationRate = (Math.Abs(targetVelocityX) > 0.1f) ? acceleration : 0f;
            finalMovementForce = Mathf.Pow(Mathf.Abs(speedDifference) * accelerationRate, velocityPower) * Mathf.Sign(speedDifference);
            playerRB.AddForce((finalMovementForce * Vector2.right), ForceMode2D.Force);
        }
    }

    private void AddFriction()
    {
        // Adding Friction/Decelration
        if (isGrounded && Mathf.Abs(moveInput) < 0.1f)
        {
            float frictionAmountToApply = playerRB.velocity.x * deceleration;
            playerRB.AddForce(Vector2.right * -frictionAmountToApply, ForceMode2D.Force);
        }
    }

    private void EnemiesHitBoxCastCheck()
    {
        RaycastHit2D enemiesCastHit = Physics2D.BoxCast(enemiesHitCheckTransform.position, enemiesBoxCastSize, 0f, Vector2.down, enemiesCheckDistance, enemiesLayerMask);
        if (enemiesCastHit.collider != null)
        {   
            Enemy enemyHit = enemiesCastHit.collider.GetComponentInParent<Enemy>();
            if (enemyHit.isInvincible)
            {
                return;
            }
            if (playerRB.velocity.y < 0f)
            {
                DamageEnemy(enemyHit);
            }
        }
    }
    private void DamageEnemy(Enemy enemyHit)
    {
        enemyHit.TakeDamage();

        playerRB.velocity = new Vector2(playerRB.velocity.x, 0f);
        playerRB.AddForce(Vector2.up * afterKillHopForce, ForceMode2D.Impulse);
    }

    public void KnockBack(Transform damageSourceTransform)
    {
        if (!canBeKnocked) return;

   
        isKnocked = true;
        canBeKnocked = false;
        
        PlayerManager.PlayerManagerInstance.OnPlayerDamaged();

        #region DEFINE_KNOCKBACK_DIRECTION
        // Direction in which player will be knocked back 
        // Depending on damage souce position relevant to player
        int directionOverride = 0;

        if (transform.position.x > damageSourceTransform.position.x)
        {
            directionOverride = 1;
        }
        if (transform.position.x < damageSourceTransform.position.x)
        {
            directionOverride = -1;
        }
        if (Mathf.Approximately(transform.position.x, damageSourceTransform.position.x))
        {
            directionOverride = 0;
        }
        #endregion

        playerRB.velocity = new Vector2(0f, 0f);
        Vector2 calculatedKnockback = new Vector2(knockbackForce * knockbackDirection.x * directionOverride, knockbackForce * knockbackDirection.y); 
        playerRB.AddForce(calculatedKnockback, ForceMode2D.Impulse);
        PlayerManager.PlayerManagerInstance.ShakeScreen(-PlayerAnimator.FacingDirection);

        Invoke(nameof(CancelKnockback), knockbackDuration);
        Invoke(nameof(AllowKnockback), invincibilityDuration);
    }
    private void CancelKnockback()
    {
        isKnocked = false;
    }
    private void AllowKnockback()
    {
        canBeKnocked = true;
    }

    public void PushedByTrampoline(float force)
    {
        playerRB.velocity = new Vector2(0f, 0f);
        playerRB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    private void OnJumpPerformed()
    {
        //TODO Variable jump height? 
        if (!isGrounded)
        {
            bufferJumpTimer = bufferJumpTime;
        }
        if (isWallSliding)
        {
            DoWallJump();
        }
        //? Add if(!isKnocked) check?
        if (isGrounded || coyoteJumpTimer > 0f)
        {
            jumpsRemaining--;
            Jump(firstJumpForce);
        }
        if (!isGrounded && canDoubleJump && !isTouchingWall && jumpsRemaining < 2  && doubleJumpAbilityUnlocked ) 
        {
            canDoubleJump = false;
            jumpsRemaining--;
            Jump(doubleJumpForce);
        }
        canWallSlide = false;
    }

    private void Jump(float jumpForce)
    {   
        if (!isKnocked)
        {   dustParticleFx.Play();
            playerRB.velocity = new Vector2(playerRB.velocity.x, 0f);
            playerRB.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);        
        }
    }

    private void BufferJump()
    {
        if (isGrounded)
        {
            if (bufferJumpTimer > 0f)
            {
                bufferJumpTimer = -1f;
                Jump(firstJumpForce);
            }
        }
    }

    private void CoyoteJump()
    {
        if (isGrounded)
        {
            canCoyoteJump = true;
        }
        else
        {
            if (canCoyoteJump)
            {
                canCoyoteJump = false;
                coyoteJumpTimer = coyoteJumpTime;
            }
        }
    }

    private void DoWallJump()
    {
        canMove = true;
        canDoubleJump = true;
        jumpsRemaining--;

        Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * -PlayerAnimator.FacingDirection, wallJumpForce * wallJumpDirection.y);
        playerRB.AddForce(forceToAdd, ForceMode2D.Impulse);
        //!Not used! 
        //? Delete?
        //Invoke("EnableMoveAfterWallJump", 0.1f);
    }

    private bool IsGrounded()
    {
        RaycastHit2D boxCastHit = Physics2D.BoxCast(groundCheckTransform.position, boxCastSize, 0f, Vector2.down, groundCheckDistance, whatIsGroundLayer);

        #region UNITYEDITOR_BOXCAST_CHANGE_COLOUR
        #if UNITY_EDITOR
            Color rayColor;
        if (boxCastHit.collider != null) {
            rayColor = Color.green;
        } else {
            rayColor = Color.red;
        }
        #endif
        #endregion

        if (boxCastHit.collider != null)
        {
            isGrounded = true;
            canMove = true;
            canDoubleJump = true;
            return true;
        }
        isGrounded = false;
        return false;   
    }
    
    private void IsTouchingWall()
    {
        RaycastHit2D rightCircleCast = Physics2D.CircleCast(rightWallCheckTransform.position, circleRadius, Vector2.right, circleDistance, whatIsWallLayer);
        RaycastHit2D leftCircleCast = Physics2D.CircleCast(leftWallCheckTransform.position, circleRadius, Vector2.left, circleDistance, whatIsWallLayer);

        isTouchingWall = rightCircleCast.collider != null || leftCircleCast.collider != null;
        isTouchingRightWall = rightCircleCast.collider != null;
        isTouchingLeftWall = leftCircleCast.collider != null;

        if (!isTouchingWall)
        {
            canMove = true;
        }
    }

    private void CheckIfCanWallSlide()
    {
        if (isTouchingWall && playerRB.velocity.y < 0f)
        {
            canWallSlide = true;
        }
        if(!isTouchingWall)
        {
            canWallSlide = false;
        }
    }

    private void DoWallSlide()
    {
        if (canWallSlide)
        {
            isWallSliding = true;
            playerRB.velocity = new Vector2(playerRB.velocity.x, playerRB.velocity.y * wallSlideFallVelocityPercent);
        }
    }

    private void CheckIfIsWallSliding()
    {   
        if (isTouchingWall)
        {
            canMove = false;
            //? double jump ability reset
            //*? Add doubleJump here ? 
            //! Apparently no, shoud be added after DoWallJump() in OnJumpPerformed()
            //! Already reseting count in OnCollisionEnter2D() since the "walls" are also tagged ground
            //jumpsRemaining = 2;
        }
        if(!canWallSlide || !isTouchingWall)
        {
            isWallSliding = false;
        }
    }

    private void CancelWallSlide()
    {
        if(isWallSliding && verticalInput < 0f)
        {
           canWallSlide = false;
        }
    }

    private void AnimationPropertieSetter()
    {
        PlayerVelocity = playerRB.velocity;
        IsPlayerGrounded = IsGrounded();
        IsPlayerTouchingWall = isTouchingWall;
        IsPlayerWallSliding = isWallSliding;
        IsKnocked = isKnocked;
        IsPlayerControllable = isPlayerControllable;
    }

}