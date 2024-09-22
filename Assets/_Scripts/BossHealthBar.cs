using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Enemy bossEnemy;       // Reference to the boss enemy
    private Slider healthBar;     // Reference to the health bar slider

    void Start()
    {
        // Get the Slider component
        healthBar = GetComponent<Slider>();

        // Initialize the health bar if boss is assigned
        if (bossEnemy != null)
        {
            healthBar.maxValue = bossEnemy.health;
            healthBar.value = bossEnemy.health;
        }
        else
        {
            // Hide the health bar if no boss is assigned
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (bossEnemy != null)
        {
            // Update the health bar value
            healthBar.value = bossEnemy.health;
            Debug.Log($"Updating Health Bar: Boss Health = {bossEnemy.Health}");
            // Hide the health bar when the boss dies
            if (bossEnemy.health <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    // Method to set the boss enemy reference and initialize the health bar
    public void Initialize(Enemy boss)
    {
        bossEnemy = boss;
        healthBar.maxValue = bossEnemy.health;
        healthBar.value = bossEnemy.health;
        gameObject.SetActive(true); // Show the health bar
    }
}
