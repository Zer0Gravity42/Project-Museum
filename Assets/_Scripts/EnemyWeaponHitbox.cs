using UnityEngine;

public class EnemyWeaponHitbox : MonoBehaviour
{
    public int damageAmount = 10; // Default damage amount
    private Collider2D weaponCollider;

    void Awake()
    {
        weaponCollider = GetComponent<Collider2D>();
        weaponCollider.enabled = false; // Disable collider initially
    }

    // Activate the hitbox
    public void ActivateHitbox()
    {
        weaponCollider.enabled = true;
    }

    // Deactivate the hitbox
    public void DeactivateHitbox()
    {
        weaponCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!weaponCollider.enabled)
            return; // Do nothing if hitbox is not active

        if (collision.CompareTag("Player"))
        {
            // Deal damage to the player
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // Use damage from DamageManager if available
                DamageManager damageManager = GetComponent<DamageManager>();
                int damageToDeal = damageManager != null ? damageManager.damage : damageAmount;

                playerController.takeDamage(damageToDeal);

                // Set player color to red to indicate damage
                SpriteRenderer playerSprite = collision.GetComponent<SpriteRenderer>();
                if (playerSprite != null)
                {
                    playerSprite.color = Color.red;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Reset player color to white
        if (collision.CompareTag("Player"))
        {
            SpriteRenderer playerSprite = collision.GetComponent<SpriteRenderer>();
            if (playerSprite != null)
            {
                playerSprite.color = Color.white;
            }
        }
    }
}
