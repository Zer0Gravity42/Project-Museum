using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class basicEnemy : Enemy
{
    private Vector2 attackDirection;
    protected Animator anim;
    protected override void setSpeedAndHealth()
    {
        anim = GetComponent<Animator>();
        speed = 0.02f;
        health = 5;
    }

    protected override void move()
    {
        if(distanceFromPlayer > 3 && attacking == false)
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
        bool attackOnCooldown= false;
        //start attack only if timer is high enough
        if (timer < 1.0)
        {
            attackOnCooldown= true;
        }
        if(attacking== true) 
        {

            //this sets the timer to 0 at the first frame of attacking
            if (attackOnCooldown == false)
            {
                timer = 0;
                anim.SetBool("attack", true);
            }
            //attack is done
            if (timer >0.7)
            {
                anim.SetBool("attack", false);
                attacking= false;
            }
            //set attack direction at start and keep that direction
            if (timer == 0 && attacking)
            {
                attackDirection = directionToPlayer;
            }
            //pull back
            if (timer < 0.3 && attacking)
            {
                transform.position += (Vector3)(attackDirection * (speed/4));
            }
            //shoot forward
            if (timer > 0.3 && attacking)
            {
                transform.position -= (Vector3)(attackDirection * (speed*2));
            }
        }
    }
}
