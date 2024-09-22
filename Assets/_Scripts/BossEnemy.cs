using UnityEngine;

public class BossEnemy : basicEnemy
{
    protected override void setSpeedAndHealth()
    {
        speed = 0.02f; // Same speed as basicEnemy, adjust if needed
        maxHealth = 15;
        health = maxHealth;
    }

    
}
