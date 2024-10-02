using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSkeleton : Enemy
{ 
    #region Private Variables
    private bool attackMode;
    private bool cooling; // Is the enemy cooling down after an attack?
    private EnemyWeaponHitbox weaponHitbox;
    #endregion

    // Initialize variables and find the hitbox on the skeleton
    protected override void setSpeedAndHealth()
    {
        speed = 0.005f;
        anim = GetComponent<Animator>();
        Transform hitboxTransform = transform.Find("Hitbox");
        if (hitboxTransform != null)
        {
            weaponHitbox = hitboxTransform.GetComponent<EnemyWeaponHitbox>();
        }
        else
        {
            Debug.LogError("Hitbox not found on the enemy. Please ensure there is a child game object named 'Hitbox' with the EnemyWeaponHitbox script attached.");
        }
    }

    // Main update loop, controls movement and attack logic
    protected override void move()
    {
        if (distanceFromPlayer <= 2 && !cooling)
        {
            attackMode = true;
        }
        if(distanceFromPlayer >2) 
        {
            anim.SetBool("canWalk", true);
            transform.position -= (Vector3)(directionToPlayer * speed);
            Flip();
        }
        if (cooling)
        {
            anim.SetBool("Attack", false);
        }
        if(timer > 1.5)
        {
            cooling= false;
        }
    }

    // Start attack sequence
    protected override void attack()
    {
        if (attackMode)
        {
            anim.SetBool("canWalk", false);
            if (timer > 1.5) 
            {
                timer = 0;
                anim.SetBool("Attack", true);

                // Activate the hitbox when the attack starts
                if (weaponHitbox != null)
                {
                    weaponHitbox.ActivateHitbox();
                }
            }     
            if(timer > 0.6)
            {
                TriggerCooling();
            }
        }
    }

    // Trigger cooldown after an attack
    public void TriggerCooling()
    {
        cooling = true;
        attackMode = false;
        anim.SetBool("Attack", false);
        // Deactivate hitbox when cooling starts
        if (weaponHitbox != null)
        {
            weaponHitbox.DeactivateHitbox();
        }
    }

    // Select the closer target between the left and right limits
    

    // Flip the enemy to face the target
    public void Flip()
    {
        Vector3 rotation = transform.eulerAngles;
        if (transform.position.x > player.transform.position.x)
        {
            rotation.y = 180;
        }
        else
        {
            rotation.y = 0;
        }

        transform.eulerAngles = rotation;
    }
}
