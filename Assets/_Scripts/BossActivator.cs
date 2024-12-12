using System;
using UnityEngine;

public class BossActivator : MonoBehaviour
{
    public GameObject boss; // Assign the BossEnemy GameObject in the Inspector
    public Vector3 bossSpawnPosition; // Set the position where the boss should appear
    [SerializeField] DungeonController dungeonController; // Reference to the DungeonController (for audio)

    private void Start()
    {
        // Find the DungeonController in the parent object
        dungeonController = GetComponentInParent<DungeonController>();
    }

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
            else
            {
                GolemBoss golemBoss = boss.GetComponent<GolemBoss>();
                if(golemBoss != null)
                {
                    golemBoss.ActivateHealthBar();
                }
                else
                {
                    SkeletonBoss skeletonBoss= boss.GetComponent<SkeletonBoss>();
                    skeletonBoss.ActivateHealthBar();
                }
            }
            
            //Play the boss music
            dungeonController.ActivateBossAudio();
            
            // Optionally, destroy the trigger so it doesn't activate again
            Destroy(gameObject);
        }
    }
}
