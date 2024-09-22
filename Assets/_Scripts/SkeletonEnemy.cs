using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEnemy : Enemy
{
    private Vector2 attackDirection;
    protected override void setSpeedAndHealth()
    {
        speed = 0.005f;
        health = 10;
    }

    protected override void move()
    {
        if (distanceFromPlayer > 3 && attacking == false)
        {
            transform.position -= (Vector3)(directionToPlayer * speed);
        }
        else
        {
            attacking = true;
        }
    }

    protected override void attack()
    {
        bool attackOnCooldown = false;
        if (timer < 1.0)
        {
            attackOnCooldown = true;
        }
        if (attacking == true)
        {
            if (attackOnCooldown == false)
            {
                timer = 0;
            }
            if (timer > 0.7)
            {
                attacking = false;
            }
            if (timer == 0 && attacking)
            {
                attackDirection = directionToPlayer;
            }
            if (timer > 0.2 && timer < 0.3 && attacking)
            {
                transform.position -= (Vector3)(attackDirection * (speed * 8));
            }
            else
            {
                transform.position += (Vector3)(attackDirection * (speed / 4));
            }
        }
    }
}
