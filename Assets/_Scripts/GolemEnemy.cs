using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemEnemy : Enemy
{
    private float attackCooldown = 1.5f;  // Cooldown time after each attack
    private float attackDuration = 0.7f;  // Duration of the attack animation
    private bool readyToAttack = true; // New flag to control attack readiness
    protected SpriteRenderer spriteRenderer; // Added SpriteRenderer variable
    private BoxCollider2D activateTrigger;


    protected override void Update()
    {
        base.Update();

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

    protected override void setSpeedAndHealth()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Initialize the SpriteRenderer
        speed = 1f;
        health = 20;
        activateTrigger= GetComponentInChildren<BoxCollider2D>();
        isAGolem= true;
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
