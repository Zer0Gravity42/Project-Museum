using UnityEngine;

public class BossActivator : MonoBehaviour
{
    public GameObject boss;              // Assign the BossEnemy GameObject in the Inspector
    public Vector3 bossSpawnPosition;    // Set the position where the boss should appear

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Set the boss's position
            boss.transform.position = bossSpawnPosition;

            // Activate the boss
            boss.SetActive(true);

            // Activate the boss's health bar
            BossEnemy bossEnemy = boss.GetComponent<BossEnemy>();
            if (bossEnemy != null)
            {
                bossEnemy.ActivateHealthBar();
            }

            // Optionally, destroy the trigger so it doesn't activate again
            Destroy(gameObject);
        }
    }
}
