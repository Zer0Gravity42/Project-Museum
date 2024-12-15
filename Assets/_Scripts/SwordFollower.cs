using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordFollower : Follower
{
    //public GameObject slash; // if needed
    public bool artifactActive = false;  // We no longer use artifactActive logic here
    public float swingDuration = 0.15f;  // Adjusted for faster swing
    public float thrustDuration = 0.15f; // Duration of the thrust attack
    public float comboResetTime = 1.0f;  // Time after which the combo resets
    public float flyingDistance = 1.0f;
    public float rotateSpeed = 2.0f;

    // The trail renderer (for normal combo attacks)
    TrailRenderer trail;

    // Particle System for thrust speed lines
    [SerializeField] private ParticleSystem speedLines;

    // Audio
    [SerializeField] AudioSource audioSourceOnce;
    [SerializeField] AudioClip swordSwing;

    // Combo tracking
    private int comboStep = 0;
    private float lastSwingTime = 0f;

    // GHOST SWORD SETTINGS
    [Header("Ghost Sword Settings")]
    [SerializeField] private GameObject ghostSwordPrefab; // Drag the GhostSword prefab here
    [SerializeField] private float ghostSwordCooldown = 5f;
    [SerializeField] private UnityEngine.UI.Image ghostSwordCooldownUI; // optional UI element
    private float ghostSwordCooldownTimer = 0f;

    void OnEnable()
    {
        attack = false;
        attackTimer = 0.0f;
        comboStep = 0;
        lastSwingTime = Time.time;
        // Reset any other necessary variables here
    }

    new void Start()
    {
        trail = GetComponent<TrailRenderer>();
        base.Start(); // Call the base class Start method
        playerTransform = GameObject.FindWithTag("Player").transform;
        trail.emitting = false;
    }

    protected override void SetRotation()
    {
        rotation = -90f; // Adjust this value as needed
    }

    protected override void Update()
    {
        base.Update(); // Runs Follower's Update logic (position/direction updates)

        // Handle ghost sword cooldown UI
        if (ghostSwordCooldownTimer > 0f)
        {
            ghostSwordCooldownTimer -= Time.deltaTime;
            if (ghostSwordCooldownUI != null)
            {
                float fill = Mathf.Clamp01(ghostSwordCooldownTimer / ghostSwordCooldown);
                ghostSwordCooldownUI.fillAmount = fill;
            }
        }

        // Press F to spawn ghost sword (orbit + artifact logic on its own script)
        if (Input.GetKeyDown(KeyCode.F) && ghostSwordCooldownTimer <= 0f)
        {
            ghostSwordCooldownTimer = ghostSwordCooldown;
            if (ghostSwordPrefab != null)
            {
                GameObject ghostSwordObj = Instantiate(ghostSwordPrefab, playerTransform.position, Quaternion.identity);
                // If using a script like GhostSwordAutoOrbit, pass the player reference:
                var orbitScript = ghostSwordObj.GetComponent<GhostSwordAutoOrbit>();
                if (orbitScript != null)
                {
                    orbitScript.playerTransform = playerTransform;
                    // orbitScript.orbitRadius = 2f; // tweak in inspector or code
                    // orbitScript.orbitSpeed = 180f;
                    // orbitScript.duration = 3f;
                    // orbitScript.selfSpinSpeed = 360f;
                }
            }
        }
    }

    protected override void HandleAttack()
    {
        // Update attack timer
        attackTimer += Time.deltaTime;

        // Default attack code with combo system (the normal combos)

        // Handle combo reset
        if (Time.time - lastSwingTime > comboResetTime)
        {
            comboStep = 0;
        }

        if (Input.GetMouseButton(0) && attackTimer > 0.2f && !attack)
        {
            attack = true;
            attackTimer = 0.0f;
            //slash.SetActive(false);

            // Start the attack coroutine with combo direction
            StartCoroutine(PerformAttack());

            // Play sword swing sound
            PlaySound(swordSwing);

            // Update combo tracking
            comboStep = (comboStep + 1) % 3; // For three steps: swing up, swing down, thrust
            lastSwingTime = Time.time;
        }
    }

    private IEnumerator PerformAttack()
    {
        if (comboStep == 2)
        {
            // Thrust attack
            yield return StartCoroutine(ThrustAttack());
        }
        else
        {
            // Swing attack
            yield return StartCoroutine(SwingSword());
        }
        attack = false;
    }

    private IEnumerator SwingSword()
    {
        float timeElapsed = 0f;

        // Calculate the base angle towards the mouse
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        Vector3 directionToMouse = (mousePosition - playerTransform.position).normalized;
        float baseAngle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;

        // Define the swing arc in degrees based on combo step
        float arcAngle = 180f; // Increased total arc angle (modify as needed)

        float startAngle, endAngle;

        if (trail != null)
        {
            trail.Clear();
            trail.emitting = true;
        }
        if (comboStep == 0)
        {
            // First swing: upward arc
            startAngle = baseAngle - arcAngle / 2;
            endAngle = baseAngle + arcAngle / 2;
        }
        else
        {
            // Second swing: downward arc
            startAngle = baseAngle + arcAngle / 2;
            endAngle = baseAngle - arcAngle / 2;
        }

        // Enable collider at the start of the swing
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = true;
        }

        while (timeElapsed < swingDuration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / swingDuration;

            // Use an animation curve for smoothness
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            // Interpolate the angle
            float currentAngle = Mathf.Lerp(startAngle, endAngle, smoothT);

            // Apply rotation around the player
            Vector3 offset = new Vector3(
                Mathf.Cos(currentAngle * Mathf.Deg2Rad),
                Mathf.Sin(currentAngle * Mathf.Deg2Rad),
                0f
            ) * distanceFromPlayer;

            transform.position = playerTransform.position + offset;
            transform.rotation = Quaternion.Euler(0, 0, currentAngle + rotation);

            yield return null;
        }
        // Disable trail renderer
        if (trail != null)
        {
            trail.emitting = false;
        }

        // Disable collider after the swing
        if (collider != null)
        {
            collider.enabled = false;
        }

        // Reset sword position and rotation after swing
        transform.position = playerTransform.position + (Vector3)(direction * distanceFromPlayer);
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    private IEnumerator ThrustAttack()
    {
        float timeElapsed = 0f;
        trail.emitting = false;

        // Calculate the direction towards the mouse
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        Vector3 directionToMouse = (mousePosition - playerTransform.position).normalized;
        float baseAngle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;

        // Enable collider at the start of the thrust
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = true;
        }

        if (speedLines != null)
        {
            speedLines.Play();
        }

        float initialDistance = distanceFromPlayer;
        float thrustDistance = initialDistance + 1.0f; // Adjust thrust distance

        // Thrust forward
        while (timeElapsed < thrustDuration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / thrustDuration;

            // Use an animation curve for smoothness
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            // Interpolate the distance
            float currentDistance = Mathf.Lerp(initialDistance, thrustDistance, smoothT);

            // Update position
            Vector3 offset = directionToMouse * currentDistance;
            transform.position = playerTransform.position + offset;
            transform.rotation = Quaternion.Euler(0, 0, baseAngle + rotation);

            yield return null;
        }

        // Return to initial position
        timeElapsed = 0f;
        while (timeElapsed < thrustDuration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / thrustDuration;

            // Use an animation curve for smoothness
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            // Interpolate the distance back to initial
            float currentDistance = Mathf.Lerp(thrustDistance, initialDistance, smoothT);

            // Update position
            Vector3 offset = directionToMouse * currentDistance;
            transform.position = playerTransform.position + offset;
            transform.rotation = Quaternion.Euler(0, 0, baseAngle + rotation);

            yield return null;
        }

        // Disable collider after the thrust
        if (collider != null)
        {
            collider.enabled = false;
        }

        if (speedLines != null)
        {
            speedLines.Stop();
        }

        // Reset sword position and rotation after thrust
        transform.position = playerTransform.position + (Vector3)(direction * distanceFromPlayer);
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSourceOnce.loop = false;
        audioSourceOnce.PlayOneShot(clip);
    }

    protected override void Flip()
    {
        // not needed here
    }
}
