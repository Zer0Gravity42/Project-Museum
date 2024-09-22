using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public abstract class Follower : MonoBehaviour
{
    protected Transform playerTransform;
    protected Vector2 direction;
    protected float distanceFromPlayer = 1.0f;
    protected bool attack = false;
    protected float attackTimer = 0.1f;

    protected virtual void Start()
    {
        // Initialize player transform reference
        playerTransform = transform.parent.transform;
        direction = playerTransform.position;
    }

    void Update()
    {
        // Handle the follower's position and direction in relation to the player
        UpdatePositionAndDirection();

        // Handle weapon-specific attack logic (to be defined in child classes)
        HandleAttack();
    }

    // Method to update position and direction of the follower based on player input
    protected void UpdatePositionAndDirection()
    {
        attackTimer += Time.deltaTime;

        if (!attack)
        {
            // Get mouse position in world space
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            mousePosition.z = playerTransform.position.z; // Ensure z is the same as the player

            // Calculate the direction vector from player to mouse
            direction = (mousePosition - playerTransform.position).normalized;

            // Only disable collider if it exists
            Collider2D collider = GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = false;
            }

            // Calculate the angle in degrees between the player and the mouse position
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Apply the rotation to the follower 
            transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        }

        // Update the position of the follower relative to the player
        transform.position = playerTransform.position + (Vector3)(direction * distanceFromPlayer);
    }

    // Abstract method for handling attack logic, to be defined in child classes
    protected abstract void HandleAttack();
}

