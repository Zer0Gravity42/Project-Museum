using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DragonEnemy : Enemy
{
    public GameObject projectilePrefab;  // Prefab for the projectile
    public float projectileSpeed = 14f;  // Speed of the projectile
    private Vector2 MoveDirection;
    private bool justAttacked = false;
    private SpriteRenderer spriteRenderer; // Added SpriteRenderer variable

    protected override void setSpeedAndHealth()
    {
        speed = 1f;
        health = 8;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Initialize the SpriteRenderer
        MoveDirection = UnityEngine.Random.insideUnitCircle.normalized;
    }

    protected override void move()
    {
        if (directionToPlayer.x > 0)
        {
            spriteRenderer.flipX = true;  // Face right
        }
        else if (directionToPlayer.x < 0)
        {
            spriteRenderer.flipX = false;   // Face left
        }

        //move further if too close
        if (distanceFromPlayer < 6)
        {
            transform.position += (Vector3)(directionToPlayer * speed * Time.deltaTime);
        }
        //move closer if too far
        else if (distanceFromPlayer > 12) 
        {
            transform.position -= (Vector3)(directionToPlayer * speed * Time.deltaTime);
        }
        //move randomly
        else
        {
            transform.position += (Vector3)(MoveDirection * speed * Time.deltaTime);
        }
        if(timer > 0.5f)
        {
            anim.SetBool("attack", false);
        }
        if(timer > 1.6f)
        {
            anim.SetBool("attack", true);
        }
        //change direction
        if(justAttacked)
        {
            MoveDirection = UnityEngine.Random.insideUnitCircle.normalized;
            justAttacked = false;
        }
    }

    protected override void attack()
    {
        //shoot a fireball at the player every 2 seconds
        if (timer > 2.0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, -directionToPlayer);
            GameObject projectile = Instantiate(projectilePrefab, transform.position, targetRotation);

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.velocity = (Vector3)(directionToPlayer * projectileSpeed); // Adjust projectile speed as needed

            timer= 0;
            justAttacked= true;
        }
    }
}
