using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public float distanceFromPlayer = 1.5f; // Distance from the center of the player
    private Transform playerTransform;

    void Start()
    {
        // Assuming Follower is a child of the player 
        playerTransform = transform.parent.transform;
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePosition.z = playerTransform.position.z; // Ensure mouse position is at the same z-depth as the player

        Vector2 direction = (mousePosition - playerTransform.position).normalized; // Get normalized direction vector from player to mouse
        transform.position = playerTransform.position + (Vector3)(direction * distanceFromPlayer); // Set position at a fixed distance from the player
        transform.up = direction; // Rotate to face the mouse
    }


}
