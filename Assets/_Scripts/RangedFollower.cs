using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RangedFollower : Follower
{
    public GameObject projectilePrefab;  // Prefab for the projectile
    public Transform firePoint;          // The point from where projectiles are fired
    public float projectileSpeed = 10f;  // Speed of the projectile
    public float attackCooldown = 1.0f;  // Cooldown time between shots (lower this to increase fire rate)

    // Override the HandleAttack method for ranged attack behavior
    protected override void HandleAttack()
    {
        attackTimer += Time.deltaTime; // Increment the attack timer

        // If the left mouse button is clicked and enough time has passed since the last shot
        // You can hold down the button for continuous firing
        if (Input.GetMouseButton(0) && attackTimer >= attackCooldown)
        {
            FireProjectile();
            attackTimer = 0.0f; // Reset the attack timer after shooting
        }
    }

    protected override void SetRotation()
    {
        rotation = -135f;
    }

    // Method to instantiate and fire a projectile
    private void FireProjectile()
    {

        // Instantiate the projectile at the fire point
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Set the velocity of the projectile's Rigidbody2D to make it move
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = firePoint.up * projectileSpeed; // Adjust projectile speed as needed

        //correct the direction
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        projectile.transform.rotation *= Quaternion.Euler(0, 0, angle-90);
    }
}



