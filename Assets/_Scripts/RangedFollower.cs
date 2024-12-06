using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI; // For UI elements if you're using them

public class RangedFollower : Follower
{
    [Header("Projectile Settings")]
    public GameObject projectilePrefab;  // Prefab for the projectile
    public Transform firePoint;          // The point from where projectiles are fired
    public float projectileSpeed = 10f;  // Speed of the projectile
    public float attackCooldown = 3.0f;  // Cooldown between normal shots
    public int numProjectiles = 5;

    private bool flipped = false;

    [Header("Audio")]
    [SerializeField] AudioSource audioSourceOnce;
    [SerializeField] AudioClip shotgun;

    [Header("Artifact Settings")]
    public bool fireArtifactActive;
    public GameObject fireBallPrefab;

    [Header("Charging Mechanic")]
    public float maxChargeTime = 2.0f;    // How long to hold right-click to fully charge
    private float currentChargeTime = 0f;
    private bool isCharging = false;

    // This should be a reference to some UI element (e.g., a slider) that represents the charge bar.
    // Place it above the player or on a canvas anchored near the player sprite.
    public Slider chargeBarUI;

    // Particle system that should be active while charging
    public ParticleSystem chargingParticles;

    public float minEmissionRate = 10f;
    public float maxEmissionRate = 20f;

    // Optionally, for size or color:
    public float minParticleSize = 0.1f;
    public float maxParticleSize = 0.2f;

    [Header("Camera Shake Settings")]
    public float normalShotShakeMagnitude = 0.3f;
    public float fireballShotShakeMagnitude = 0.5f;

    [Header("Knockback Settings")]
    public float fireballKnockbackForce = 5f;

    private PlayerController playerController;
    private CameraFollow cameraFollow;

    protected override void Start()
    {
        base.Start();
        playerController = GetComponentInParent<PlayerController>();
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
    }

    protected override void Update()
    {
        base.Update();

        // Only perform charging logic if the artifact is active
        if (fireArtifactActive)
        {
            // Check if right-click is pressed down: start charging
            if (Input.GetMouseButtonDown(1))
            {
                StartCharging();
            }

            // While holding right-click: increase charge time
            if (Input.GetMouseButton(1) && isCharging)
            {
                UpdateCharging();
            }

            // If right-click is released: fire if charging
            if (Input.GetMouseButtonUp(1) && isCharging)
            {
                ReleaseCharge();
            }

            // If fully charged and still holding, you might auto-release
            if (isCharging && currentChargeTime >= maxChargeTime)
            {
                ReleaseCharge();
            }
        }

        // Normal attacks (left-click) always available
        HandleAttack();
    }

    protected override void HandleAttack()
    {
        attackTimer += Time.deltaTime; // Increment the attack timer

        // Normal attack on left click (no change here)
        if (Input.GetMouseButton(0) && attackTimer >= attackCooldown)
        {
            PlaySound(shotgun);
            FireProjectile();

            // Trigger camera shake for normal shot
            cameraFollow.shakeMagnitude = normalShotShakeMagnitude;
            cameraFollow.TriggerShake();

            attackTimer = 0.0f; // Reset the attack timer after shooting
        }
    }

    protected override void SetRotation()
    {
        rotation = 0f;
    }

    protected override void Flip()
    {
        //this code flips the gun so it is always oriented correctly
        if (Mathf.Abs(transform.rotation.z) > 0.7f && flipped == false)
        {
            Vector3 localScale = transform.localScale;
            localScale.y = -localScale.y;
            transform.localScale = localScale;
            flipped = true;
        }
        else if (Mathf.Abs(transform.rotation.z) < 0.7f && flipped == true)
        {
            Vector3 localScale = transform.localScale;
            localScale.y = -localScale.y;
            transform.localScale = localScale;
            flipped = false;
        }
    }

    private void FireProjectile()
    {
        // Calculate direction towards the mouse
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = ((Vector2)(mousePosition - firePoint.position)).normalized;

        float spreadAngle = 30f; // Total spread angle in degrees
        int pellets = numProjectiles; // Number of projectiles (pellets)

        for (int i = 0; i < pellets; i++)
        {
            // Calculate random spread within the spread angle
            float randomAngle = UnityEngine.Random.Range(-spreadAngle / 2, spreadAngle / 2);

            // Instantiate the projectile
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

            // Rotate the direction vector by the random angle
            Vector2 spreadDirection = RotateVector(direction, randomAngle);

            // Set projectile rotation to match the direction
            projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, spreadDirection);

            // Apply velocity to the projectile
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.velocity = spreadDirection * projectileSpeed;
        }


    }

    // Utility function to rotate a 2D vector by an angle in degrees
    private Vector2 RotateVector(Vector2 vector, float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(
            vector.x * cos - vector.y * sin,
            vector.x * sin + vector.y * cos
        );
    }

    private void FireFireball()
    {
        GameObject fireball = Instantiate(fireBallPrefab, firePoint.position + new Vector3(-1.25f,0,0), firePoint.rotation);
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        rb.velocity = firePoint.up * projectileSpeed;
        fireball.transform.rotation *= Quaternion.Euler(0, 0, -90);

        // Trigger camera shake for fireball shot
        cameraFollow.shakeMagnitude = fireballShotShakeMagnitude;
        cameraFollow.TriggerShake();

        // Apply knockback to the player
        // Direction opposite of the firing direction
        Vector2 knockbackDirection = -firePoint.up;
        playerController.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * fireballKnockbackForce, ForceMode2D.Impulse);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSourceOnce.loop = false;
        audioSourceOnce.PlayOneShot(clip);
    }

    //=== Charging Methods ===
    private void StartCharging()
    {
        isCharging = true;
        currentChargeTime = 0f;

        // Show and reset the UI bar
        if (chargeBarUI != null)
        {
            chargeBarUI.gameObject.SetActive(true);
            chargeBarUI.value = 0f;
        }

        // Start particle effects
        if (chargingParticles != null)
        {
            chargingParticles.Play();
        }
    }

    private void UpdateCharging()
    {
        currentChargeTime += Time.deltaTime;
        float fillAmount = Mathf.Clamp01(currentChargeTime / maxChargeTime);

        // Update UI
        if (chargeBarUI != null)
        {
            chargeBarUI.value = fillAmount;
        }

        // You could also manipulate particle intensity based on fillAmount if desired
        // Update emission
        var emission = chargingParticles.emission;
        emission.rateOverTime = Mathf.Lerp(minEmissionRate, maxEmissionRate, fillAmount);

        // Update size
        var main = chargingParticles.main;
        main.startSize = Mathf.Lerp(minParticleSize, maxParticleSize, fillAmount);
    }

    private void ReleaseCharge()
    {
        isCharging = false;

        // Hide UI
        if (chargeBarUI != null)
        {
            chargeBarUI.gameObject.SetActive(false);
        }

        // Stop particle effects
        if (chargingParticles != null)
        {
            chargingParticles.Stop();
            chargingParticles.Clear();
        }

        // Fully charged if we reached max charge time
        bool fullyCharged = currentChargeTime >= maxChargeTime;

        // If the player had at least some charge, fire the artifact projectile
        if (fullyCharged && fireArtifactActive)
        {
            FireFireball();
        }

        chargingParticles.Stop();

        var emission = chargingParticles.emission;
        emission.rateOverTime = minEmissionRate; // Reset to default
        var main = chargingParticles.main;
        main.startSize = minParticleSize; // Reset size

        // Reset the charge
        currentChargeTime = 0f;
    }
}
