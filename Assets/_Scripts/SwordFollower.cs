using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordFollower : Follower
{
    public GameObject slash;
    // Override the HandleAttack method for sword attack behavior
    protected override void HandleAttack()
    {
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
            // Increase the distance during the attack to simulate a thrust
            distanceFromPlayer += 0.03f;
            this.GetComponent<Collider2D>().enabled = true;
        }
        else
        {
            // Return the sword to its original position after the attack
            if (distanceFromPlayer > 1.0f)
            {
                distanceFromPlayer -= 0.01f;
            }
        }
    }
}
