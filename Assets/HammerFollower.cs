using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerFollower : Follower
{
    [SerializeField] private float hammerDamage = 50f;
    [SerializeField] private float hammerKnockback = 5f;
    [SerializeField] private float hammerAttackCooldown = 1.0f;
    [SerializeField] private float swingDuration = 0.2f;
    [SerializeField] private float swingArcAngle = 120f; // Total arc of the swing
    private Quaternion originalRotation;
    private TrailRenderer trail;
    private bool isHammerAttacking = false;

    protected override void Start()
    {
        base.Start();
        originalRotation = transform.localRotation; // Store the original rotation
        trail = GetComponent<TrailRenderer>();
        if (trail != null)
        {
            trail.emitting = false;
        }
    }

    protected override void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) && attackTimer >= hammerAttackCooldown && !isHammerAttacking)
        {
            StartCoroutine(HammerSwing());
        }
    }

    private IEnumerator HammerSwing()
    {
        isHammerAttacking = true;
        attackTimer = 0f;

        // Calculate the start and end angles for the swing arc
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        Vector3 directionToMouse = (mousePosition - playerTransform.position).normalized;
        float baseAngle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
        float startAngle = baseAngle - swingArcAngle / 2;
        float endAngle = baseAngle + swingArcAngle / 2;

        // Enable hammer collider for the duration of the swing
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = true;
        }

        // Activate the trail renderer if available
        if (trail != null)
        {
            trail.Clear();
            trail.emitting = true;
        }

        float elapsedTime = 0f;
        while (elapsedTime < swingDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / swingDuration;

            // Smooth interpolation of the swing arc
            float currentAngle = Mathf.Lerp(startAngle, endAngle, Mathf.SmoothStep(0f, 1f, t));

            // Update hammer position and rotation around the player
            Vector3 offset = new Vector3(
                Mathf.Cos(currentAngle * Mathf.Deg2Rad),
                Mathf.Sin(currentAngle * Mathf.Deg2Rad),
                0f
            ) * distanceFromPlayer;

            transform.position = playerTransform.position + offset;
            transform.rotation = Quaternion.Euler(0, 0, currentAngle + rotation);

            yield return null;
        }

        // Disable trail renderer after the swing
        if (trail != null)
        {
            trail.emitting = false;
        }

        // Disable the collider after the swing
        if (collider != null)
        {
            collider.enabled = false;
        }

        // Reset hammer position and rotation
        transform.position = playerTransform.position + (Vector3)(direction * distanceFromPlayer);
        transform.rotation = originalRotation;

        isHammerAttacking = false;
    }

    protected override void SetRotation()
    {
        rotation = 45f; // Adjust for alignment if needed
    }

    protected override void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.y = playerTransform.localScale.x > 0 ? 1 : -1;
        transform.localScale = localScale;
    }

  
}
