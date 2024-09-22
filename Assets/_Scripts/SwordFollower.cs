using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordFollower : Follower
{
    public GameObject slash;
    public bool artifactActive = false; // Set this in the Inspector
    public float flyingDistance = 1.0f;
    public float rotateSpeed = 2.0f;
    // Override the HandleAttack method for sword attack behavior

    void Start()
    {
        base.Start(); // Call the base class Start method 
        playerTransform = GameObject.FindWithTag("Player").transform;
    }
    protected override void HandleAttack()
    {
        if (artifactActive)
        {
            if (Input.GetMouseButtonDown(0) && attackTimer > 0.5f)
            {
                attack = true;
                attackTimer = 0.0f;
                // Make the sword fly forward to a certain distance
                distanceFromPlayer += 1.5f; // Adjust as needed
                GetComponent<Collider2D>().enabled = true;
                flyingDistance = distanceFromPlayer;
            }

            if (Input.GetMouseButton(0) && attack)
            {
                // Rotate the sword while holding left click
                transform.Rotate(Vector3.forward * Time.deltaTime * 360 * rotateSpeed); // Rotate around Z axis
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0f;
                Vector3 playerPosition = playerTransform.position;

                Vector3 direction = (mousePosition - playerPosition).normalized;

                transform.position = playerPosition + direction * flyingDistance;
                slash.SetActive(true);
            }

            else if (Input.GetMouseButtonUp(0) && attack)
            {
                // Return the sword when left click is released
                attack = false;
                GetComponent<Collider2D>().enabled = false;
                slash.SetActive(false);
            }

            if (!attack)
            {
                // Return the sword to its original position
                if (distanceFromPlayer > 1.0f)
                {
                    distanceFromPlayer -= 0.05f; // Adjust speed as needed
                }
                else
                {
                    distanceFromPlayer = 1.0f; // Ensure it doesn't go below default
                }
                // Reset rotation
                transform.rotation = Quaternion.identity;
            }
        }
        else
        {
            // Original attack code
            if (Input.GetMouseButtonDown(0) && attackTimer > 0.5f)
            {
                attack = true;
                attackTimer = 0.0f;
                slash.SetActive(true);
            }

            if (attackTimer > 0.1f)
            {
                attack = false;
                slash.SetActive(false);
            }

            if (attack)
            {
                // Increase distance to simulate a thrust
                distanceFromPlayer += 0.03f;
                GetComponent<Collider2D>().enabled = true;
            }
            else
            {
                // Return sword to original position
                if (distanceFromPlayer > 1.0f)
                {
                    distanceFromPlayer -= 0.01f;
                }
            }
        }

        // Update attack timer
        attackTimer += Time.deltaTime;
    }
}
