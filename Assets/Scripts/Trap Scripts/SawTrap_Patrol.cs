using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//? Reverse Inheritance ? 
//! Think about this one
//TODO: make this inherit from SawTrap_ForwadBack in order to remove the huge text area
public class SawTrap_Patrol : Damage
{
    #region IN_EDITOR_INSTRUCTIONS
    [Header("Instructions")]
    /*[Tooltip(@"Set isWorking to true and hasCooldown to false, 
    if you want to use multiple points (more than 2).

    Set isWorking to false and hasCooldown to true, 
    if you want to have a two point patrol behaviour 
    with cooldown at each point and a sprite flip function.")]*/
    [TextArea(10, 11)]
    [SerializeField] string Instructions;
    #endregion  

    private const string IS_WORKING_STRING = "isWorking";

    private Animator sawTrapAnimator;
    private SpriteRenderer sawSpriteRenderer;

    [Space()]
    [Header("Saw Behaviour Settings")]
    [SerializeField] private bool isWorking;
    [SerializeField] private bool hasCooldown = false; 
    [SerializeField] protected float moveSpeed = 5.0f;
    [SerializeField] private float cooldownDuration = 1.0f;
    [SerializeField] protected Transform[] moveToPointArray;
    protected int moveToPointIndex;
    private float cooldownTimer;
    protected bool flip = false;


    private void Awake()
    {
        sawTrapAnimator = GetComponent<Animator>();
        sawSpriteRenderer = GetComponent<SpriteRenderer>();
        
        //* Always set the saw to the first point position
        transform.position = moveToPointArray[0].position;
    }

    private void Update()
    {
        if (hasCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            isWorking = cooldownTimer < 0f;
        }

        sawTrapAnimator.SetBool(IS_WORKING_STRING, isWorking);

        //* MoveSaw()
        MoveSaw();
    }

    protected virtual void MoveSaw()
    {
        if (isWorking)
        {
            transform.position = Vector2.MoveTowards(transform.position, moveToPointArray[moveToPointIndex].position, moveSpeed * Time.deltaTime);            
        }
        if (Vector2.Distance(transform.position, moveToPointArray[moveToPointIndex].position) < 0.01f)
        {   
            if (hasCooldown)
            {   
                FlipSaw(flip);
                flip = !flip;
                cooldownTimer = cooldownDuration;
            }
            moveToPointIndex++;
            if (moveToPointIndex >= moveToPointArray.Length)
            {
                moveToPointIndex = 0;
            }
        }
    }

    protected void FlipSaw(bool flip)
    {
        sawSpriteRenderer.flipX = flip;
    }
}