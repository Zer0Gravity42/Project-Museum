using System.Collections;
using UnityEngine;

public class SwordFollower : Follower
{
    //public GameObject slash;
    public bool artifactActive = false; // Set this in the Inspector or elsewhere
    public float swingDuration = 0.15f;  // Adjusted for faster swing
    public float thrustDuration = 0.15f; // Duration of the thrust attack
    public float comboResetTime = 1.0f; // Time after which the combo resets
    public float flyingDistance = 1.0f;
    public float rotateSpeed = 2.0f;
    TrailRenderer trail;
    // Particle System for thrust speed lines
    [SerializeField]private ParticleSystem speedLines;
    // Scaling

    // Audio
    [SerializeField] AudioSource audioSourceOnce;
    [SerializeField] AudioClip swordSwing;

    // Combo tracking
    private int comboStep = 0;
    private float lastSwingTime = 0f;

   

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

    protected override void HandleAttack()
    {
        // Update attack timer
        attackTimer += Time.deltaTime;

        if (artifactActive)
        {
            // Original artifact code
            if (Input.GetMouseButtonDown(0) && attackTimer > 0.5f)
            {
                attack = true;
                attackTimer = 0.0f;
                // Make the sword fly forward to a certain distance
                distanceFromPlayer += 45f * Time.deltaTime; // Adjust as needed
                GetComponent<Collider2D>().enabled = true;
                flyingDistance = distanceFromPlayer;
            }

            if (Input.GetMouseButton(0) && attack)
            {
                // Rotate the sword while holding left click
                transform.Rotate(Vector3.forward * Time.deltaTime * 360 * rotateSpeed); // Rotate around Z axis
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0f;
                Vector3 playerPosition = playerTransform.position;

                Vector3 direction = (mousePosition - playerPosition).normalized;

                transform.position = playerPosition + direction * flyingDistance;
                //slash.SetActive(true);

            }
            else if (Input.GetMouseButtonUp(0) && attack)
            {
                // Return the sword when left click is released
                attack = false;
                GetComponent<Collider2D>().enabled = false;
                //slash.SetActive(false);
            }

            if (!attack)
            {
                // Return the sword to its original position
                if (distanceFromPlayer > 1.0f)
                {
                    distanceFromPlayer -= 1.5f * Time.deltaTime; // Adjust speed as needed
                }
                else
                {
                    distanceFromPlayer = 1.0f; // Ensure it doesn't go below default
                }
                // Reset rotation
                transform.rotation = Quaternion.identity;
            }
        }
        else
        {
            // Default attack code with combo system

            // Handle combo reset
            if (Time.time - lastSwingTime > comboResetTime)
            {
                comboStep = 0;
            }

            if (Input.GetMouseButton(0) && attackTimer > 0.2f &!attack)
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

            //// Smooth scaling (scale up)
            //float scale = Mathf.Lerp(1f, maxScale, t);
            //transform.localScale = new Vector3(scale, scale, 1f);

            //slash.SetActive(true);

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

            // Smooth scaling (scale down)
            //float scale = Mathf.Lerp(maxScale, 1f, t);
            //transform.localScale = new Vector3(scale, scale, 1f);

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
}
