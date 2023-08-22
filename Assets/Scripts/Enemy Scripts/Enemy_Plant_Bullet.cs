using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Plant_Bullet : Damage
{
    private Rigidbody2D bulletRB;

    private float xSpeed;
    private float ySpeed;

    private void Awake()
    {
        bulletRB = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        bulletRB.velocity = new Vector2(xSpeed, ySpeed);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);        
        Destroy(gameObject);
    }

    public void SetupBulletSpeed(float x, float y)
    {
        xSpeed = x;
        ySpeed = y;
    }
}
