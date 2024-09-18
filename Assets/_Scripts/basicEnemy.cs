using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class basicEnemy : Enemy
{
    protected override void setSpeedAndHealth()
    {
        speed = 0.02f;
        health = 3;
    }

    protected override void move()
    {
        if(distanceFromPlayer > 1)
        {
            transform.position -= (Vector3)(directionToPlayer * speed);
        }
        else
        {
            attack();
        }
    }

    protected override void attack()
    {
        if(timer > 1)
        {
            Debug.Log("attack!");
            timer = 0;
        }
    }
}
