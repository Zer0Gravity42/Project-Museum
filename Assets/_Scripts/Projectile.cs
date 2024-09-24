using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;   // Speed of the projectile
    public float lifetime = 5f; // How long the projectile lasts before being destroyed

    private Rigidbody2D rb;

    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();

        // Set the projectile to move in the forward direction
        rb.velocity = transform.up * speed;

        // Destroy the projectile after its lifetime
        Destroy(gameObject, lifetime);
    }
}

