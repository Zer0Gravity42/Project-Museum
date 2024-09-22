using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DragonEnemy : Enemy
{
    public GameObject projectilePrefab;  // Prefab for the projectile
    public float projectileSpeed = 14f;  // Speed of the projectile
    private Vector2 MoveDirection;
    private bool justAttacked = false;

    protected override void setSpeedAndHealth()
    {
        speed = 0.005f;
        health = 10;
        MoveDirection = UnityEngine.Random.insideUnitCircle.normalized;
    }

    protected override void move()
    {
        if (distanceFromPlayer < 6)
        {
            transform.position += (Vector3)(directionToPlayer * speed);
        }
        else if (distanceFromPlayer > 12) 
        {
            transform.position -= (Vector3)(directionToPlayer * speed);
        }
        else
        {
            transform.position += (Vector3)(MoveDirection * speed);
        }
        if(justAttacked)
        {
            MoveDirection = UnityEngine.Random.insideUnitCircle.normalized;
            justAttacked = false;
        }
    }

    protected override void attack()
    {
        if (timer > 2.0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, -directionToPlayer);
            GameObject projectile = Instantiate(projectilePrefab, transform.position, targetRotation);

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.velocity = (Vector3)(directionToPlayer * projectileSpeed); // Adjust projectile speed as needed

            timer= 0;
            justAttacked= true;
        }
    }
}
