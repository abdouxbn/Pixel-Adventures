using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Mushroom : Enemy
{
    private const string X_VELOCITY = "xVelocity";

    private void Update()
    {
        CollisionChecks();
        WalkAround();
        enemyAnimator.SetFloat(X_VELOCITY, enemyRB.velocity.x);
    }

}
