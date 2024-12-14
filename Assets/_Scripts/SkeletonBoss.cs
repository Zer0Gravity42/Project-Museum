using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkeletonBoss : Enemy
{
    [SerializeField] DungeonController dungeonController; // Reference to the DungeonController (for audio)
    public GameObject bossHealthBar; // Assign the health bar UI element in the Inspector
    public Text bossNameLabel;
    public TextMeshProUGUI bossQuoteLabel;
    private bool isDead = false; // Flag to track if the boss is dead
    
    private bool attackMode = false;
    private EnemyWeaponHitbox weaponHitbox;
    private bool cooling = false;
    public GameObject skeletonSpawnA;
    public GameObject skeletonSpawnB;
    public GameObject skeletonSpawnC;
    public Transform skeletonSpawnLocation;
    private float spawnTimer;

    // Start is called before the first frame update
    protected override void setSpeedAndHealth()
    {
        maxHealth = 75;
        health = maxHealth;
        speed = 3;
        anim = GetComponent<Animator>();
        Transform hitboxTransform = transform.Find("Hitbox");
        if (hitboxTransform != null)
        {
            weaponHitbox = hitboxTransform.GetComponent<EnemyWeaponHitbox>();
        }
        else
        {
            Debug.LogError("Hitbox not found on the enemy. Please ensure there is a child game object named 'Hitbox' with the EnemyWeaponHitbox script attached.");
        }
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
        //GameObject.Find("BossDoor").transform.Find("RightDoor").GetComponent<DoorController>().CloseDoor();

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

        spawnTimer += Time.deltaTime;
        if (spawnTimer > 4) 
        {
            Spawn();
            spawnTimer = 0;
        }


        if (health <= 0 && !isDead)
        {
            isDead = true; //Mark the boss as dead so the audio doesn't 

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
    }

    protected override void move()
    {
        if (distanceFromPlayer <= 5 && !cooling)
        {
            Flip();
            attackMode = true;
        }
        if (!attackMode && !cooling)
        {
            anim.SetBool("canWalk", true);
            if(distanceFromPlayer > 3)
            {
                transform.position -= (Vector3)(directionToPlayer * speed * Time.deltaTime);
                Flip();
            }
            else
            {
                anim.SetBool("canWalk", false);
            }
        }
        if (cooling)
        {
            anim.SetBool("attack", false);
            anim.SetBool("canWalk", true);
            transform.position += (Vector3)(directionToPlayer * speed * 1.5f * Time.deltaTime);
            NegativeFlip();
        }
        if (timer > 2.5)
        {
            cooling = false;
        }
    }

    protected override void attack()
    {
        if (attackMode)
        {
            if (timer > 2)
            {
                timer = 0;
                anim.SetBool("attack", true);
                if (weaponHitbox != null)
                {
                    weaponHitbox.ActivateHitbox();
                }
            }
            if(timer>1.4)
            {
                anim.SetBool("attack", false);
                attackMode = false;
                cooling = true;
                if (weaponHitbox != null)
                {
                    weaponHitbox.DeactivateHitbox();
                }
            }
            if(timer >0.4 && timer < 0.6)
            {
                transform.position -= (Vector3)(directionToPlayer * speed * 3 * Time.deltaTime);
            }
            if (timer > 1 && timer < 1.2)
            {
                transform.position -= (Vector3)(directionToPlayer * speed * 3 * Time.deltaTime);
            }
        }
    }

    private void Spawn()
    {
        int spawn = UnityEngine.Random.Range(1, 3);
        if(spawn == 1)
        {
            GameObject spawnA = Instantiate(skeletonSpawnA, skeletonSpawnLocation);
            spawnA.GetComponent<Enemy>().player = player;
        }
        if (spawn == 2)
        {
            GameObject spawnB = Instantiate(skeletonSpawnB, skeletonSpawnLocation);
            spawnB.GetComponent<Enemy>().player = player;
        }
        if (spawn == 3)
        {
            GameObject spawnC = Instantiate(skeletonSpawnC, skeletonSpawnLocation);
            spawnC.GetComponent<Enemy>().player = player;
        }
    }

    public void Flip()
    {
        Vector3 rotation = transform.eulerAngles;
        if (transform.position.x > player.transform.position.x)
        {
            rotation.y = 180;
        }
        else
        {
            rotation.y = 0;
        }
        transform.eulerAngles = rotation;
    }
    public void NegativeFlip()
    {
        Vector3 rotation = transform.eulerAngles;
        if (transform.position.x > player.transform.position.x)
        {
            rotation.y = 0;
        }
        else
        {
            rotation.y = 180;
        }
        transform.eulerAngles = rotation;
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
