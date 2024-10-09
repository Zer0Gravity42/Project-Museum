using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemy : basicEnemy
{
    [SerializeField] DungeonController dungeonController; // Reference to the DungeonController (for audio)
    public GameObject bossHealthBar; // Assign the health bar UI element in the Inspector
    public Text bossNameLabel;
    public TextMeshProUGUI bossQuoteLabel;
    public GameObject enemySpawn;
    private float spawnTimer;
    private bool isDead = false; // Flag to track if the boss is dead

    protected override void setSpeedAndHealth()
    {
        base.setSpeedAndHealth(); // Call the base method to initialize common variables
                                  // Override or adjust properties as needed
        speed = 0.04f; // Adjust as needed
        maxHealth = 30;
        health = maxHealth;
    }

    protected override void Awake()
    {
        isDead = false; //Reset the dead flag. Why? Who knows. It's a mystery.
        base.Awake();
        if (bossHealthBar != null)
        {
            bossHealthBar.SetActive(false); // Ensure the health bar is inactive initially
        }
        if (bossNameLabel != null)
        {
            bossNameLabel.gameObject.SetActive(false); // Ensure the boss name label is inactive initially
        }

        StartCoroutine(ShowBossCaption());
    }

    public override void takeDamage(int damage)
    {
        base.takeDamage(damage);
        UpdateHealthBar();
    }

    protected override void Update()
    {
        base.Update();
        UpdateBossQuotePosition();

        spawnTimer += Time.deltaTime;

        //spawn enemies
        if(spawnTimer > 3) 
        {
            spawnTimer = 0;
            GameObject enemy = Instantiate(enemySpawn, transform.position, Quaternion.identity);
            enemy.GetComponent<Enemy>().player = player;
        }

        // If the boss is dead, deactivate the health bar and text label
        if (health <= 0 && !isDead)
        {
            isDead = true; //Mark the boss as dead so the audio doesn't 
            
            StartCoroutine(PlayBossDeathAudio());
            
            //Update Health Bar
            if (bossHealthBar != null)
            {
                bossHealthBar.SetActive(false);
            }

            if (bossNameLabel != null)
            {
                bossNameLabel.gameObject.SetActive(false);
            }
        }
    }

    public void ActivateHealthBar()
    {
        if (bossHealthBar != null)
        {
            bossHealthBar.SetActive(true);

            // Initialize the health bar value
            Slider slider = bossHealthBar.GetComponent<Slider>();
            if (slider != null)
            {
                slider.maxValue = maxHealth;
                slider.value = health;
            }
        }
        if (bossNameLabel != null)
        {
            bossNameLabel.gameObject.SetActive(true);
            
        }
    }

    private void UpdateHealthBar()
    {
        if (bossHealthBar != null)
        {
            Slider slider = bossHealthBar.GetComponent<Slider>();
            if (slider != null)
            {
                slider.value = health;
            }
        }
    }

    private IEnumerator PlayBossDeathAudio()
    {
        //Update Audio
        dungeonController.StopAllSound();
        dungeonController.PlaySound(dungeonController.dungeonBossDeath);
        
        yield return new WaitForSeconds(dungeonController.dungeonBossDeath.length);
    }

    private IEnumerator ShowBossCaption()
    {
        //Show the boss caption
        bossQuoteLabel.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(dungeonController.dungeonBossVoiceLine.length);
        
        //Hide the boss caption
        bossQuoteLabel.gameObject.SetActive(false);
    }
    
    private void UpdateBossQuotePosition()
    {
        if (bossQuoteLabel != null && bossQuoteLabel.gameObject.activeSelf)
        {
            // Get the boss's world position
            Vector3 bossPosition = transform.position;

            // Calculate the offset in world space
            Vector3 offset = new Vector3(-2, 1, 0); // Adjust this value as needed for height above the boss

            // Set the position of the quote label to be above the boss
            bossQuoteLabel.transform.position = bossPosition + offset;
        }
    }
}

