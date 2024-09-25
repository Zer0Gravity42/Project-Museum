using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEnemy : Enemy
{
    private Vector2 attackDirection;
    protected override void setSpeedAndHealth()
    {
        speed = 0.005f;
        //he needs twice the health because he has 2 colliders lol
        health = 20;
    }

    protected override void move()
    {
        if (distanceFromPlayer > 3 && attacking == false)
        {
            //move closer
            transform.position -= (Vector3)(directionToPlayer * speed);
        }
        else
        {
            //start attack
            attacking = true;
        }
    }

    protected override void attack()
    {
        bool attackOnCooldown = false;
        //start attack only if timer is high enough
        if (timer < 1.0)
        {
            attackOnCooldown = true;
        }
        if (attacking == true)
        {
            //this sets the timer to 0 at the first frame of attacking
            if (attackOnCooldown == false)
            {
                timer = 0;
            }
            //attack is done
            if (timer > 0.7)
            {
                attacking = false;
            }
            //set attack direction at start and keep that direction
            if (timer == 0 && attacking)
            {
                attackDirection = directionToPlayer;
            }
            //shoot forward
            if (timer > 0.2 && timer < 0.3 && attacking)
            {
                transform.position -= (Vector3)(attackDirection * (speed * 8));
            }
            //pull back
            else
            {
                transform.position += (Vector3)(attackDirection * (speed / 4));
            }
        }
    }
}
