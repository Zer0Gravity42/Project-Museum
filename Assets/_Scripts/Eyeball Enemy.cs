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

    private float afterImageTimer = 0f;
    private float afterImageInterval = 0.15f; // Interval between afterimages
    private float enemyAfterImageFadeSpeed = 3f; // Adjust this value as needed



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
        speed = 9f;
        health = 5;
    }

    protected override void move()
    {
        if (distanceFromPlayer > 3 && !attacking)
        {
            transform.position -= (Vector3)(directionToPlayer * speed * Time.deltaTime);
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
                    afterImageTimer = 0f; // Reset afterImageTimer
                }
                // Adjust position based on attack phase
                if (timer < 0.3f)
                {
                    transform.position += (Vector3)(attackDirection * (speed / 4) * Time.deltaTime);
                }
                else
                {
                    transform.position -= (Vector3)(attackDirection * (speed * 1.25f) * Time.deltaTime);
                }

                // Create afterimages at intervals
                afterImageTimer += Time.deltaTime;
                if (afterImageTimer >= afterImageInterval)
                {
                    CreateAfterImage();
                    afterImageTimer = 0f;
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

    void CreateAfterImage()
    {
        // Create a new GameObject for the afterimage
        GameObject afterImage = new GameObject("AfterImage");
        afterImage.transform.position = transform.position;
        afterImage.transform.rotation = transform.rotation;
        afterImage.transform.localScale = transform.localScale;

        // Add a SpriteRenderer component
        SpriteRenderer sr = afterImage.AddComponent<SpriteRenderer>();

        // Copy the sprite and settings from the enemy's SpriteRenderer
        sr.sprite = spriteRenderer.sprite;
        sr.flipX = spriteRenderer.flipX;
        sr.flipY = spriteRenderer.flipY;
        sr.color = spriteRenderer.color;

        // Match the sorting layer and order
        sr.sortingLayerID = spriteRenderer.sortingLayerID;
        sr.sortingOrder = spriteRenderer.sortingOrder - 1; // Ensure afterimage appears behind the enemy

        // Disable any colliders (if any are added by default, which they aren't in this case)
        Collider2D collider = afterImage.GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // Add the AfterImageFade script
        AfterImageFade afterImageFade = afterImage.AddComponent<AfterImageFade>();

        // Set the fade speed for this afterimage
        afterImageFade.SetAlphaDecay(enemyAfterImageFadeSpeed);
    }

}
