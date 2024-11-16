using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemEnemy : Enemy
{
    private bool readyToAttack = true; // New flag to control attack readiness
    protected SpriteRenderer spriteRenderer; // Added SpriteRenderer variable
    private BoxCollider2D golemHitbox;


    protected override void Update()
    {
        base.Update();

        if(golemSpawn)
        {
            // Flip sprite to face the player
            if (directionToPlayer.x > 0)
            {
                spriteRenderer.flipX = true;  // Face right
            }
            else if (directionToPlayer.x < 0)
            {
                spriteRenderer.flipX = false;   // Face left
            }
        }
    }

    protected override void setSpeedAndHealth()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Initialize the SpriteRenderer
        isAGolem= true;
        golemHitbox= GetComponent<BoxCollider2D>();
        golemHitbox.enabled= false;
    }

    protected override void move()
    {
        if (distanceFromPlayer > 3 && !attacking)
        {
            anim.SetBool("moving", true);
            transform.position -= (Vector3)(directionToPlayer * speed * Time.deltaTime);
        }
        else if (!attacking && readyToAttack)
        {
            attacking = true;
        }
        else
        {
            anim.SetBool("moving", false);
        }
        if(timer > 2)
        {
            readyToAttack = true;
        }
        else
        {
            readyToAttack = false;
        }
    }

    protected override void attack()
    {
        if(attacking && timer> 2)
        {
            anim.SetBool("attack", true);
            timer = 0;
        }
        if(attacking && timer > 1) 
        {
            anim.SetBool("attack", false);
            attacking= false;
        }
    }

    public void activateGolem()
    {
        anim.SetBool("spawn", true);
        golemSpawn= true;

        golemHitbox.enabled= true;

        // Instantiate the health bar as a child of the enemy
        GameObject hb = Instantiate(healthBarPrefab, transform);
        hb.transform.localPosition = healthBarOffset; // Position it relative to the enemy

        // Apply the custom scale to the health bar
        hb.transform.localScale = healthBarScale;

        // Get the HealthBar component
        healthBar = hb.GetComponentInChildren<HealthBar>();

        // Initialize the health bar
        maxHealth = health; // Ensure maxHealth is set
        healthBar.SetMaxHealth(maxHealth);
    }
}
