using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonTriggerArea : MonoBehaviour
{
    private MeleeSkeleton skeletonParent; // Reference to the parent skeleton

    // Initialize reference to the parent skeleton
    private void Awake()
    {
        skeletonParent = GetComponentInParent<MeleeSkeleton>();
    }

    // Trigger when the player enters the trigger area
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false); // Deactivate the trigger area
            skeletonParent.target = collider.transform; // Set the player as the skeleton's target
            skeletonParent.inRange = true; // Indicate that the player is in range
            skeletonParent.hotZone.SetActive(true); // Activate the hot zone for further interactions
        }
    }
}
