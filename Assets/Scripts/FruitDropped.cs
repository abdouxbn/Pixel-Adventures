using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FruitDropped : Fruit
{
    [SerializeField] private SpriteRenderer fruitSpriteRenderer;
    [SerializeField] private Vector2 moveSpeed;
    [SerializeField] private Color colourTransparency;
    private bool canPickupFruit = false;


    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine("SpriteBlink");
        Invoke("CanPickupFruit", 2.0f);

    }

    private void Update()
    {
        transform.position += new Vector3(moveSpeed.x, moveSpeed.y) * Time.deltaTime;
    } 

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (canPickupFruit)
        {
            base.OnTriggerEnter2D(other);            
        }
    }


    private IEnumerator SpriteBlink()
    {
        int i = 0;
        while (i < 8)
        {
            fruitSpriteRenderer.color = colourTransparency;
            yield return new WaitForSeconds(0.3f);
            fruitSpriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.3f);
            i++;
        }
    }

    private void CanPickupFruit()
    {
        canPickupFruit = true;
    }
}
