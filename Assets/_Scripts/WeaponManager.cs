using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Follower swordFollower;  // Reference to SwordFollower instance
    public Follower rangedFollower; // Reference to RangedFollower instance

    private Follower currentWeapon; // The currently equipped weapon
    private int currentWeaponIndex = 0; // 0 for Sword, 1 for Ranged

    // Start is called before the first frame update
    void Start()
    {
        // Start with the sword by default
        currentWeapon = swordFollower;
        SwitchWeapon(0); // Equip sword initially
    }

    // Update is called once per frame
    void Update()
    {
        // Handle weapon switching via keys (1 for sword, 2 for ranged)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(0); // Switch to sword
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(1); // Switch to ranged
        }

        // Handle weapon switching via mouse scroll wheel
        HandleMouseScroll();
    }

    // Method to handle mouse scroll input for weapon switching
    private void HandleMouseScroll()
    {
        // Get mouse scroll input (positive for up, negative for down)
        float scroll = Input.mouseScrollDelta.y;

        if (scroll != 0)
        {
            // If scrolling up, increment weapon index; if scrolling down, decrement weapon index
            if (scroll > 0)
            {
                currentWeaponIndex = (currentWeaponIndex + 1) % 2; // Cycles between 0 and 1
            }
            else if (scroll < 0)
            {
                currentWeaponIndex = (currentWeaponIndex - 1 + 2) % 2; // Handles negative scroll, cycles back to 1
            }

            // Switch to the weapon based on the updated index
            SwitchWeapon(currentWeaponIndex);
        }
    }

    // Method to switch between weapons based on index
    void SwitchWeapon(int weaponIndex)
    {
        if (weaponIndex == 0)
        {
            currentWeapon = swordFollower;
            swordFollower.gameObject.SetActive(true);
            rangedFollower.gameObject.SetActive(false);
            Debug.Log("Switched to Sword");
        }
        else if (weaponIndex == 1)
        {
            currentWeapon = rangedFollower;
            swordFollower.gameObject.SetActive(false);
            rangedFollower.gameObject.SetActive(true);
            Debug.Log("Switched to Ranged Weapon");
        }
    }
}


