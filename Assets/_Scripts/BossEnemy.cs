using UnityEngine;
using UnityEngine.UI;

public class BossEnemy : basicEnemy
{
    public GameObject healthBar; // Assign the health bar UI element in the Inspector
    public Text bossNameLabel;

    protected override void setSpeedAndHealth()
    {
        base.setSpeedAndHealth(); // Call the base method to initialize common variables
                                  // Override or adjust properties as needed
        speed = 0.02f; // Adjust as needed
        maxHealth = 15;
        health = maxHealth;
    }

    protected override void Awake()
    {
        base.Awake();
        if (healthBar != null)
        {
            healthBar.SetActive(false); // Ensure the health bar is inactive initially
        }
        if (bossNameLabel != null)
        {
            bossNameLabel.gameObject.SetActive(false); // Ensure the boss name label is inactive initially
        }
    }

    public override void takeDamage(int damage)
    {
        base.takeDamage(damage);
        UpdateHealthBar();
    }

    protected override void Update()
    {
        base.Update();

        // If the boss is dead, deactivate the health bar and text label
        if (health <= 0)
        {
            if (healthBar != null)
            {
                healthBar.SetActive(false);
            }

            if (bossNameLabel != null)
            {
                bossNameLabel.gameObject.SetActive(false);
            }
        }
    }

    public void ActivateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.SetActive(true);

            // Initialize the health bar value
            Slider slider = healthBar.GetComponent<Slider>();
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
        if (healthBar != null)
        {
            Slider slider = healthBar.GetComponent<Slider>();
            if (slider != null)
            {
                slider.value = health;
            }
        }
    }
}

