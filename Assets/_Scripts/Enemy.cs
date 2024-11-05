using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    //player attributes
    public GameObject player;
    protected Vector2 directionToPlayer;
    protected float distanceFromPlayer;
    protected Animator anim;
    

    //enemy atributes
    protected float speed;
    public int health;
    protected int maxHealth;
    protected bool isAnEnemy = true; //dont question it

    //for use in ai behaviour
    protected float timer;
    protected bool moving = true;
    protected bool attacking = false;
    public bool alive = true;
    protected bool awake = false;

    // Health bar variables
    public bool showHealthBar = true;         // Controls whether to show the health bar
    public GameObject healthBarPrefab;        // Assign the HealthBar prefab via the Inspector
    public Vector3 healthBarOffset = new Vector3(0, -1, 0); // Offset position of the health bar
    protected HealthBar healthBar;            // Reference to the HealthBar script
    public Vector3 healthBarScale = Vector3.one;  //scale for the health bar
    //public int Health { get { return health; } }
    //public int MaxHealth { get { return maxHealth; } }

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        //sets the attributes for the specific enemy type
        setSpeedAndHealth();

        if (showHealthBar && healthBarPrefab != null)
        {
            // Instantiate the health bar as a child of the enemy
            GameObject hb = Instantiate(healthBarPrefab, transform);
            hb.transform.localPosition = healthBarOffset; // Position it relative to the enemy

            // Apply the custom scale to the health bar
            hb.transform.localScale = healthBarScale;

            // Get the HealthBar component
            healthBar = hb.GetComponentInChildren<HealthBar>();

            // Initialize the health bar
            maxHealth = health; // Ensure maxHealth is set
            healthBar.SetMaxHealth(maxHealth);
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //for use in enemy ai
        timer += Time.deltaTime;

        //every enemy type will probably need these
        directionToPlayer = (transform.position - player.transform.position).normalized;
        distanceFromPlayer = MathF.Abs(transform.position.x - player.transform.position.x) + MathF.Abs(transform.position.y - player.transform.position.y);

        if(alive && awake)
        {
            //call move and attack with specific ai for each enemy
            move();
            attack();
        }

        //if dead then die
        if(health <= 0 && alive)
        {
            if(isAnEnemy)
            {
                anim.SetBool("dead", true);
                alive = false;
                timer = 0;
            }
            else
            {
                Destroy(healthBar.gameObject);
                Destroy(gameObject);
            }
        }

        if (!alive)
        {
            if (showHealthBar && healthBar != null)
            {
                Destroy(healthBar.gameObject);
            }
        }

        if (!alive && timer >= 1)
        {
            if (showHealthBar && healthBar != null)
            {
                Destroy(healthBar.gameObject);
            }
            Destroy(gameObject);
        }
        if(!awake && timer >= 1 && isAnEnemy)
        {
            awake= true;
            anim.SetBool("awake", true);
        }
        if(!isAnEnemy)
        {
            showHealthBar = true;
        }
    }

    public virtual void takeDamage (int damage)
    {
        health -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Health is now {health}.");

        if (showHealthBar && healthBar != null)
        {
            healthBar.SetHealth(health);
        }
        anim.SetTrigger("hurt"); 

    }
  

    protected abstract void setSpeedAndHealth();

    protected abstract void move();

    protected abstract void attack();
}
