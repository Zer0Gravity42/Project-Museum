using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotZoneCheck : MonoBehaviour
{
    private MeleeSkeleton skeletonParent; // Reference to the parent skeleton
    private bool inRange; // Is the player within the hot zone?
    private Animator anim; // Reference to the skeleton's animator

    // Initialize references to the parent skeleton and its animator
    private void Awake()
    {
        skeletonParent = GetComponentInParent<MeleeSkeleton>();
        anim = GetComponentInParent<Animator>();
    }

    // Check if the player is in range and the skeleton is not attacking, flip the skeleton's direction
    private void Update()
    {
        if (inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("Skeleton_Attack"))
        {
            skeletonParent.Flip();
        }
    }

    // Trigger when the player enters the hot zone
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            inRange = true; // Player is in range
        }
    }

    // Trigger when the player exits the hot zone
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            inRange = false; // Player is no longer in range
            gameObject.SetActive(false); // Deactivate the hot zone
            skeletonParent.triggerArea.SetActive(true); // Activate the trigger area
            skeletonParent.inRange = false; // Notify the skeleton the player is no longer in range
            skeletonParent.SelectTarget(); // Select a new target for the skeleton
        }
    }
}
