using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSkeleton : Enemy
{
    #region Public Variables
    public float attackDistance; // Minimum distance to trigger attack
    public Transform leftLimit;
    public Transform rightLimit;
    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange; // Is the player in attack range?
    public GameObject hotZone;
    public GameObject triggerArea;
    #endregion

    #region Private Variables
    private Animator anim;
    private bool attackMode;
    private bool cooling; // Is the enemy cooling down after an attack?
    private float intTimer; // Initial timer value
    private EnemyWeaponHitbox weaponHitbox;
    #endregion

    // Initialize variables and find the hitbox on the skeleton
    protected override void setSpeedAndHealth()
    {
        SelectTarget();// Store initial timer value
        speed = 2;
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

    // Main update loop, controls movement and attack logic
    protected override void move()
    {
        if (!attackMode)
        {
            anim.SetBool("canWalk", true);

            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy_attack"))
            {
                Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            }
        }

        // If not attacking and the player is not in range, select a new target
        if (!InsideOfLimits() && !inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy_attack"))
        {
            SelectTarget();
        }

        // If the player is in range, execute enemy logic
        if (inRange)
        {
            EnemyLogic();
        }
    }

    // Handles the attack logic based on distance
    void EnemyLogic()
    {
        distanceFromPlayer = Vector2.Distance(transform.position, target.position);

        if (distanceFromPlayer > attackDistance)
        {
            StopAttack();
        }
        else if (attackDistance >= distanceFromPlayer && !cooling)
        {
            attackMode = true;
        }

        if (cooling)
        {
            Cooldown();
            anim.SetBool("Attack", false);
        }
    }

    // Start attack sequence
    protected override void attack()
    {
        if(attackMode)
        {
            timer = 0; // Reset timer when entering attack range

            anim.SetBool("canWalk", false);
            anim.SetBool("Attack", true);

            // Activate the hitbox when the attack starts
            if (weaponHitbox != null)
            {
                weaponHitbox.ActivateHitbox();
            }
        }
    }

    // Handle attack cooldown logic
    void Cooldown()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }

    // Stop attacking and reset hitbox
    void StopAttack()
    {
        cooling = false;
        attackMode = false;
        anim.SetBool("Attack", false);

        // Deactivate hitbox after attack
        if (weaponHitbox != null)
        {
            weaponHitbox.DeactivateHitbox();
        }
    }

    // Trigger cooldown after an attack
    public void TriggerCooling()
    {
        cooling = true;

        // Deactivate hitbox when cooling starts
        if (weaponHitbox != null)
        {
            weaponHitbox.DeactivateHitbox();
        }
    }

    // Check if the enemy is within set movement limits
    private bool InsideOfLimits()
    {
        if (leftLimit == null || rightLimit == null)
        {
            Debug.LogError("One or both limits are missing.");
            return false; // If limits are missing, assume outside of bounds
        }

        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

    // Select the closer target between the left and right limits
    public void SelectTarget()
    {
        if (leftLimit == null || rightLimit == null)
        {
            Debug.LogError("One or both limits are missing.");
            return;
        }

        float distanceToLeft = Vector3.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector3.Distance(transform.position, rightLimit.position);

        // Select the closer limit as the target
        if (distanceToLeft > distanceToRight)
        {
            target = leftLimit;
        }
        else
        {
            target = rightLimit;
        }

        Flip();
    }

    // Flip the enemy to face the target
    public void Flip()
    {
        Vector3 rotation = transform.eulerAngles;
        if (transform.position.x > target.position.x)
        {
            rotation.y = 180;
        }
        else
        {
            rotation.y = 0;
        }

        transform.eulerAngles = rotation;
    }
}
