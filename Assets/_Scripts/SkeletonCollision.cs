using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCollision : MonoBehaviour
{
    private MeleeSkeleton skeleton; // Reference to the MeleeSkeleton script

    private void Start()
    {
        // Find the MeleeSkeleton component on the parent or attached object
        skeleton = GetComponent<MeleeSkeleton>();

        if (skeleton == null)
        {
            Debug.LogError("MeleeSkeleton component not found on the object.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the collider is the player's weapon
        if (collider.gameObject.CompareTag("Weapon"))
        {
            // Get the DamageManager component from the player's weapon to apply the correct amount of damage
            DamageManager damageManager = collider.GetComponent<DamageManager>();

            if (damageManager != null)
            {
                // Apply damage to the skeleton using the takeDamage method in MeleeSkeleton
                skeleton.takeDamage(damageManager.damage);

                // Optionally, add some visual feedback here (e.g., flash skeleton red when hit)
                StartCoroutine(FlashRedOnHit());
            }
            else
            {
                Debug.LogError("DamageManager component not found on the player's weapon.");
            }
        }
    }

    // Optional: Flash red when hit to indicate damage taken
    private IEnumerator FlashRedOnHit()
    {
        SpriteRenderer skeletonSprite = GetComponentInChildren<SpriteRenderer>();

        if (skeletonSprite != null)
        {
            // Change color to red
            skeletonSprite.color = Color.red;

            // Wait for a short period
            yield return new WaitForSeconds(0.1f);

            // Reset color to white (or original color)
            skeletonSprite.color = Color.white;
        }
    }
}

