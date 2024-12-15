using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSwordAutoOrbit : MonoBehaviour
{
    [Header("Orbit Settings")]
    public Transform playerTransform;  // Assigned when we spawn the sword
    public float orbitRadius = 2f;     // Distance from player center
    public float orbitSpeed = 180f;    // Degrees/sec around player
    public float duration = 3f;        // Total orbit time before returning/destroying

    [Header("Artifact Spin Settings")]
    public float selfSpinSpeed = 360f; // Sword spins around its own axis (deg/sec)
    public float flyForwardSpeed = 0f; // (Optional) If you want it to fly outward initially

    private float orbitAngle = 0f;
    private float timer = 0f;
    private Collider2D col2D;
    private TrailRenderer trail;

    void Awake()
    {
        col2D = GetComponent<Collider2D>();
        trail = GetComponent<TrailRenderer>();
        if (trail) trail.emitting = false;
        if (col2D) col2D.enabled = false;
    }

    void OnEnable()
    {
        timer = duration;
        // Enable collider & trail at start
        if (col2D) col2D.enabled = true;
        if (trail) trail.emitting = true;
    }

    void Update()
    {
        if (playerTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        // Countdown the orbit duration
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            // Optional: Return sword or just destroy it
            Destroy(gameObject);
            return;
        }

        // PHASE 1: (Optional) Fly outward from player on spawn
        if (flyForwardSpeed > 0f && timer == duration) // only run on first frame
        {
            orbitRadius += flyForwardSpeed * Time.deltaTime;
        }

        // ORBIT around player
        orbitAngle += orbitSpeed * Time.deltaTime;
        float rad = orbitAngle * Mathf.Deg2Rad;
        Vector3 orbitOffset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * orbitRadius;
        transform.position = playerTransform.position + orbitOffset;

        // SPIN around own axis
        transform.Rotate(Vector3.forward, selfSpinSpeed * Time.deltaTime);
    }
}
