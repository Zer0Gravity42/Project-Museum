using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    private float distanceFromPlayer = 1.0f; // Distance from the center of the player
    private Transform playerTransform;
    private bool attack = false;
    private float attackTimer = 0.0f;

    void Start()
    {
        // Assuming Follower is a child of the player 
        playerTransform = transform.parent.transform;
    }

    void Update()
    {
        //time how long the attack lasts and how long the attack lasts and how long the cooldown is
        attackTimer += Time.deltaTime;

        //alekseys code
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePosition.z = playerTransform.position.z; // Ensure mouse position is at the same z-depth as the player

        Vector2 direction = (mousePosition - playerTransform.position).normalized; // Get normalized direction vector from player to mouse
        transform.position = playerTransform.position + (Vector3)(direction * distanceFromPlayer); // Set position at a fixed distance from the player
        transform.up = direction; // Rotate to face the mouse

        //when mouse clicked (if attack not on cooldown) set attack to true and reset the timer
        if (Input.GetMouseButtonDown(0) && attackTimer>0.5f)
        {
            attack = true;
            attackTimer = 0.0f;
        }
        //when attack duration is up set attack to false
        if(attackTimer>0.1f)
        {
            attack = false;
        }
        if(attack)
        {
            distanceFromPlayer += 0.03f;
        }
        else
        {
            if(distanceFromPlayer > 1.0f)
            {
                distanceFromPlayer -= 0.01f;
            }
        }
    }


}
