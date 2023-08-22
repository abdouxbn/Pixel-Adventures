using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawTrap_ForwardBack : SawTrap_Patrol
{
    private bool goingForward = true;

    protected override void MoveSaw()
    {
        transform.position = Vector2.MoveTowards(transform.position, moveToPointArray[moveToPointIndex].position, moveSpeed * Time.deltaTime); 

        if (Vector2.Distance(transform.position, moveToPointArray[moveToPointIndex].position) < 0.01f)
        {   
            if (moveToPointIndex == 0)
            {
                goingForward = true;
                FlipSaw(flip);
                flip = !flip;
            }

            if (goingForward)
            {
                moveToPointIndex++;
            }
            else
            {
                moveToPointIndex--;
            }

            if (moveToPointIndex >= moveToPointArray.Length)
            {
                moveToPointIndex = moveToPointArray.Length - 1;
                goingForward = false;
                FlipSaw(flip);
                flip = !flip;
            }
        }
    }
}