using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;

public class basicEnemy : Enemy
{
    private Vector2 attackDirection;
    private float attackCooldown = 1.0f;  // Cooldown time after each attack
    private float attackDuration = 0.7f;  // Duration of the attack animation
    private bool readyToAttack = true; // New flag to control attack readiness
    protected SpriteRenderer spriteRenderer; // Added SpriteRenderer variable

    protected override void Update()
    {
        base.Update();
        //timer += Time.deltaTime;  // Ensure the timer increments each frame
        
        //if(awake && alive)
        //{
          //  move();
            //attack();
        //}

        // Flip sprite to face the player
        if (directionToPlayer.x > 0)
        {
            spriteRenderer.flipX = false;  // Face right
        }
        else if (directionToPlayer.x < 0)
        {
            spriteRenderer.flipX = true;   // Face left
        }
    }

    protected override void setSpeedAndHealth()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Initialize the SpriteRenderer
        speed = 0.035f;
        health = 5;
    }

    protected override void move()
    {
        if (distanceFromPlayer > 3 && !attacking)
        {
            transform.position -= (Vector3)(directionToPlayer * speed);
        }
        else if (!attacking && readyToAttack)
        {
            attacking = true;
            timer = 0;  // Reset timer when starting an attack
        }
    }

    protected override void attack()
    {
        if (attacking)
        {
            if (timer < attackDuration)
            {
                anim.SetBool("attack", true);
                if (timer == 0)
                {
                    attackDirection = directionToPlayer;  // Set attack direction at the start of the attack
                }
                // Adjust position based on attack phase
                if (timer < 0.3)
                {
                    transform.position += (Vector3)(attackDirection * (speed / 4));
                }
                else
                {
                    transform.position -= (Vector3)(attackDirection * (speed * 1.25f));
                }

            }
            else
            {
                anim.SetBool("attack", false);
            }

            // Wait for the full cooldown to finish before allowing another attack
            if (timer >= attackCooldown)
            {
                attacking = false;
                readyToAttack = false;  // Prevent immediate re-attack
                timer = 0;  // Reset timer to allow for another attack after cooldown
            }
        }
        else if (!attacking && !readyToAttack)
        {
            // Ensure there's a delay before setting readyToAttack true again
            if (timer >= attackCooldown)
            {
                readyToAttack = true;
            }
        }
    }
}
