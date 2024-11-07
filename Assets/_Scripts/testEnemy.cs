using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class testEnemy : Enemy
{
    private Vector2 attackDirection;
    
    protected override void setSpeedAndHealth()
    {
        //test enemy is immobile, high hp 
        speed = 0.0f;
        health = 20;
        
    }

    protected override void move()
    {
       //dont let the test enemy move
    }

    protected override void attack()
    {
       //the test enemy doesn't attack
    }

    
}
