using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class GolemBoss : basicEnemy
{
    [SerializeField] DungeonController dungeonController; // Reference to the DungeonController (for audio)
    public GameObject bossHealthBar; // Assign the health bar UI element in the Inspector
    public Text bossNameLabel;
    public TextMeshProUGUI bossQuoteLabel;
    private bool isDead = false; // Flag to track if the boss is dead

    public List<GameObject> golemSpawns;
    private int previousHealth;
    private float3 teleportPositon;
    private bool teleportStart = false;

    protected override void setSpeedAndHealth()
    {
        base.setSpeedAndHealth(); // Call the base method to initialize common variables
                                  // Override or adjust properties as needed
        speed = 5f; // Adjust as needed
        maxHealth = 50;
        speedMultiplyer= 2f;
        health = maxHealth;
        afterImageColor= UnityEngine.Color.red;
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
        GameObject.Find("BossDoor").transform.Find("RightDoor").GetComponent<DoorController>().CloseDoor();

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

        if(timer > 2)
        {
            teleport();
            awake = false;
        }

        // If the boss is dead, deactivate the health bar and text label
        if (health <= 0 && !isDead)
        {
            isDead = true; //Mark the boss as dead so the audio doesn't 

            //kill any alive spawns
            for(int i = 0;i < golemSpawns.Count;i++)
            {
                if (golemSpawns[i]!= null)
                {
                    golemSpawns[i].GetComponent<GolemEnemy>().health = 0;
                }
            }

            StartCoroutine(PlayBossDeathAudio());
            GameObject.Find("BossDoor").transform.Find("RightDoor").GetComponent<DoorController>().OpenDoor();

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

        //this sprite is facing a different direction from the normal enemy :sob:
        if(spriteRenderer.flipX)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }

        if(health < 50 && previousHealth == 50)
        {
            golemSpawns[0].GetComponent<GolemEnemy>().activateGolem();
        }
        if (health < 40 && previousHealth >= 40)
        {
            golemSpawns[1].GetComponent<GolemEnemy>().activateGolem();
        }
        if (health < 30 && previousHealth >= 30)
        {
            golemSpawns[2].GetComponent<GolemEnemy>().activateGolem();
        }
        if (health < 20 && previousHealth >= 20)
        {
            golemSpawns[3].GetComponent<GolemEnemy>().activateGolem();
        }
        if (health < 10 && previousHealth >= 10)
        {
            golemSpawns[4].GetComponent<GolemEnemy>().activateGolem();
        }
        previousHealth = health;
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

    private void teleport()
    {
        if(teleportStart == false)
        {
            teleportPositon.x = player.transform.position.x - (directionToPlayer.x*3);
            teleportPositon.y = player.transform.position.y - (directionToPlayer.y * 3);
        }
        teleportStart = true;
        anim.SetBool("awake", false);

        anim.SetBool("teleportout", true);

        if(timer > 2.5)
        {
            anim.SetBool("teleportin", true);
            gameObject.transform.position = teleportPositon;
        }
        if(timer>3)
        {
            timer = 1;
            awake= true;
            teleportStart = false;
            anim.SetBool("awake", true);
            anim.SetBool("teleportin", false);
            anim.SetBool("teleportout", false);
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
