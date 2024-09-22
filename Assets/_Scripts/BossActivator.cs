using UnityEngine;

public class BossActivator : MonoBehaviour
{
    public GameObject boss;              // Assign the BossEnemy GameObject in the Inspector
    public Vector3 bossSpawnPosition;    // Set the position where the boss should appear
    public BossHealthBar bossHealthBar;  // Reference to the BossHealthBar script

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Set the boss's position
            boss.transform.position = bossSpawnPosition;

            // Activate the boss
            boss.SetActive(true);

            // Get the Enemy component from the boss GameObject
            Enemy bossEnemy = boss.GetComponent<Enemy>();
            if (bossEnemy != null)
            {
                // Initialize the health bar with the boss enemy reference
                bossHealthBar.Initialize(bossEnemy);
            }
            else
            {
                Debug.LogError("Boss GameObject does not have an Enemy component.");
            }

            // Optionally, destroy the trigger so it doesn't activate again
            Destroy(gameObject);
        }
    }
}
