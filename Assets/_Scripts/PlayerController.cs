using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 2.0f;
    private Rigidbody2D rb;
    private Vector2 movementDirection;
    private bool isFacingRight = true; // Tracks the player's current facing direction


    [Header("Animation Settings")]
    public Animator animator;

    [Header("Health Settings")]
    public int health = 3;
    public int maxHealth = 3;
    public event System.Action<int, int> OnHealthChanged;

    #region Dash Variables
    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;  // Time before dash can be used again
    [SerializeField] private bool canDash = false;
    private bool isDashing = false;
    private float lastDashTime; // Timestamp for last dash

    private SpriteRenderer spriteRenderer;
    public float afterImageInterval = 0.05f;
    private float afterImageTimer = 0f;
    private bool isInvulnerable = false;  // Indicates if the player is invulnerable
    // Public Getter for isInvulnerable
    public bool IsInvulnerable
    {
        get { return isInvulnerable; }
    }
    #endregion

    #region Transformation Variables
    [Header("Transformation Settings")]
    public Sprite golemSprite;          // Golem sprite
    private Sprite originalSprite;      // Store the original player sprite
    private Vector3 originalScale;      // Store the player's original scale
    private bool isTransformed = false; // Track if the player is currently transformed
    private bool canMove = true;        // Player can move when not transformed
    [SerializeField] private bool canTransform = false;
    private GameObject activeWeapon = null; // Currently active weapon
    public AnimationClip golemAttackClip;
    [SerializeField] private GameObject golemHitbox; // Reference to the golem's hitbox collider

    #endregion

    #region Audio Variables
    [Header("Audio Settings")]
    //[SerializeField] private AudioClip transformSound; // Assign in Inspector
    [SerializeField] private AudioClip attackSound;   // Assign in Inspector
    [SerializeField] private AudioClip transformSound; 
    private AudioSource audioSource;                 // Reference to AudioSource component
    #endregion

    #region UI Variables
    [Header("UI Settings")]
    [SerializeField] private Image cooldownBar;          // Assign in Inspector
    //[SerializeField] private TextMeshProUGUI cooldownText; // Assign in Inspector (Optional)
    [SerializeField] private float attackCooldown = 5f;  // Cooldown duration in seconds
    private bool isAttackOnCooldown = false;             // Tracks if attack is on cooldown
    #endregion

    private GameObject currentInteraction = null; //This controls our interaction system, basically what the interactable object we are currently in contact with

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Initialize AudioSource
        audioSource = GetComponent<AudioSource>();

        // Save the original player sprite at the start
        originalSprite = spriteRenderer.sprite;
        originalScale = transform.localScale;

        //Initialize health
        health = maxHealth;
        OnHealthChanged?.Invoke(health, maxHealth);

        #region Artifact Powers Enables/Disables

        GameObject tempManager = GameObject.FindGameObjectWithTag("MainManager"); //get temporary access to which artifacts are equipped

        #region Dash: if an Artifact with an ID of 1 is equipped in either slots, allow the player to dash
        if (tempManager.GetComponent<MainManager>().equippedSlotOneId == 1 || tempManager.GetComponent<MainManager>().equippedSlotTwoId == 1)    
        {
            canDash = true;
        }
        #endregion
        #region GolemTransform: if an Artifact with an ID of 2 is equipped in either slots, allow the player to transform
        if (tempManager.GetComponent<MainManager>().equippedSlotOneId == 2 || tempManager.GetComponent<MainManager>().equippedSlotTwoId == 2)
        {
            canTransform = true;
        }
        #endregion
        //#region SuperSword: if an Artifact with an ID of 3 is equipped in either slots, allow the player to use the super sword
        //if (tempManager.GetComponent<MainManager>().equippedSlotOneId == 3 || tempManager.GetComponent<MainManager>().equippedSlotTwoId == 3)
        //{
        //    canDash = true;
        //}
        //#endregion

        //IF THE SCENE IS IN THE MUSEUM NO ARTIFACT POWERS WE DONT WANT OUR CUSTOMERS TO DIE
        if(SceneManager.GetActiveScene().name == "Museum")
        {
            DisableAllArtifacts();
        }
        #endregion

        // Initialize cooldown bar visibility based on canTransform
        if (cooldownBar != null)
        {
            cooldownBar.gameObject.SetActive(canTransform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
            ProcessInputs();

        if (canTransform)
            cooldownBar.gameObject.SetActive(true);

        UpdateAnimation();
        ProcessInteracts(); //interaction system

        // Handle Dash input
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(Dash());
        }

        // Temporary testing: Press 'K' to increase max health by 1
        if (Input.GetKeyDown(KeyCode.K))
        {
            IncreaseMaxHealth(1);
        }
        // Temporary testing: Press 'H' to heal 1 hp 
        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(1);
        }

        // Handle transformation when 'T' is pressed
        if (Input.GetKeyDown(KeyCode.T) && !isTransformed)
        {
            StartCoroutine(TransformIntoGolem());
        }


    }

    void FixedUpdate()
    {
        if (canMove)
               Move();
        if (health <= 0)
        {
            die();
        }
    }


    //im moving this to a public method so i can call it if timer hits 0
    public void die()
    {
        ClearTempManagerObjects();
        Application.LoadLevel("Main Menu");
    }

    private void DisableAllArtifacts()
    {
        canDash = false;
        canTransform = false;
    }


    public void takeDamage(int damage)
    {
        if (isInvulnerable)
            return;

        else
        {
            health -= damage;
            health = Mathf.Clamp(health, 0, maxHealth);
            OnHealthChanged?.Invoke(health, maxHealth);
        }
    }

    public void Heal(int amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        OnHealthChanged?.Invoke(health, maxHealth);
    }
    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        OnHealthChanged?.Invoke(health, maxHealth);
    }
    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); //Only 0 and 1. Can be changed to GetAxis for slight movement (controller)
        float moveY = Input.GetAxisRaw("Vertical");

        movementDirection = new Vector2(moveX, moveY).normalized; //Normalizing this keeps the speed consistent on diagonals
                                                                  // Update facing direction based on horizontal input
        if (moveX > 0)
        {
            isFacingRight = true;
        }
        else if (moveX < 0)
        {
            isFacingRight = false;
        }
    }


    void Move()
    {
        if (isDashing)
            return; // Skip movement during dash
        rb.velocity = new Vector2(movementDirection.x * movementSpeed, movementDirection.y * movementSpeed);
    }

    #region Dash functions
    IEnumerator Dash()
    {
        if (canDash)
        {
            isDashing = true;
            isInvulnerable = true;
            lastDashTime = Time.time;
            Vector2 dashDirection = movementDirection;

            rb.velocity = dashDirection * dashSpeed;

            float dashTime = 0f;
            afterImageTimer = 0f;
            while (dashTime < dashDuration)
            {
                dashTime += Time.deltaTime;
                afterImageTimer += Time.deltaTime;
                if (afterImageTimer >= afterImageInterval)
                {
                    CreateAfterImage();
                    afterImageTimer = 0f;
                }
                yield return null;
            }

            isDashing = false;
            isInvulnerable = false;
            rb.velocity = Vector2.zero;
        }

    }

    void CreateAfterImage()
    {
        GameObject afterImage = new GameObject("AfterImage");
        SpriteRenderer sr = afterImage.AddComponent<SpriteRenderer>();
        sr.sprite = spriteRenderer.sprite;
        sr.color = new Color(1f, 1f, 0f, 0.5f); // Semi-transparent
        sr.sortingLayerID = spriteRenderer.sortingLayerID;
        sr.sortingOrder = spriteRenderer.sortingOrder;

        afterImage.transform.position = transform.position;
        afterImage.transform.localScale = transform.localScale;
        afterImage.transform.rotation = transform.rotation;

        afterImage.AddComponent<AfterImageFade>();
    }
    #endregion


    void UpdateAnimation()
    {
        // Update the animator parameters
        animator.SetFloat("Horizontal", movementDirection.x);
        animator.SetFloat("Vertical", movementDirection.y);
        animator.SetFloat("Speed", movementDirection.sqrMagnitude);  // Use square magnitude to determine if the player is moving
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DungeonPortal")
        {
            Application.LoadLevel("Floor1"); //loads the dungeon scene
        }

        if (collision.tag == "MuseumPortal")
        {
            GameObject tempManager = GameObject.FindGameObjectWithTag("MainManager");
            tempManager.SendMessage("AddAllToPermanents");
            ClearTempManagerObjects();
            Application.LoadLevel("Museum"); //loads the dungeon system
        }

        if (collision.CompareTag("InteractObject"))
        {
            //Debug.Log(collision.name);
            currentInteraction = collision.gameObject; //Assing interaction object (usable for both dialogue and pickups)          
        }
    }

    private void ProcessInteracts()
    {
        if (Input.GetButtonDown("Interact") && currentInteraction && currentInteraction.GetComponent<Artifact>()) //sends a message to object interaction system only if we're interacting with something, and this object has 
        {                                                                                                        //the Artifact script attached (marking it as an Artifact)
            currentInteraction.SendMessage("DoPickUp"); //the method inside
            currentInteraction = null;
        }

        if (Input.GetButtonDown("Interact") && currentInteraction && currentInteraction.GetComponent<ArtifactDescription>())
        {
            //Debug.Log("Interacted with Museum object");
            currentInteraction.SendMessage("DoDescriptionPopUp"); //artifact description interact (museum)
            currentInteraction = null;
        }

        if (Input.GetButtonDown("Interact") && currentInteraction && currentInteraction.GetComponent<DoorController>())
        {
            currentInteraction.SendMessage("DoOpenDoor"); //ask to open the door
            currentInteraction = null;
        }

        if (Input.GetButtonDown("Interact") && currentInteraction && currentInteraction.GetComponent<KeyController>())
        {
            currentInteraction.SendMessage("DoPickUpKey"); //pick up the key
            currentInteraction = null;
        }

        if (Input.GetButtonDown("Interact"))
        {
            Debug.Log("Interacting");
        }

    }

    private void ClearTempManagerObjects()
    {
        GameObject tempManager = GameObject.FindGameObjectWithTag("MainManager");
        tempManager.GetComponent<MainManager>().doors.Clear(); //clear player progress rip
        tempManager.GetComponent<MainManager>().keys.Clear(); //clear player progress rip
        tempManager.GetComponent<MainManager>().tempArtifacts.Clear(); //clear player progress rip
        tempManager.GetComponent<MainManager>().artifactsInScene.Clear(); //changing scene so we need a different list of current artifacts
    }

    // Coroutine to handle the transformation and attack animation
    IEnumerator TransformIntoGolem()
    {
        if (canTransform == false)
            yield break;
        // Prevent transformation if already on cooldown
        if (isAttackOnCooldown)
        {
            Debug.Log("Golem attack is on cooldown.");
            yield break;
        }

        // Reset player's velocity to stop movement
        rb.velocity = Vector2.zero;
        isTransformed = true; // Prevent spamming the transformation
        canMove = false;

        //Store the currently active weapon
        activeWeapon = GetActiveWeapon();
        SetChildrenActive(false);

        // Determine the current facing direction and preserve it during scaling
        float newScaleX = isFacingRight ? originalScale.x * 2.5f : -originalScale.x * 2.5f;
        transform.localScale = new Vector3(newScaleX, originalScale.y * 2.5f, originalScale.z); // Scale up while preserving directions
        // Swap the player's sprite to the golem sprite
        spriteRenderer.sprite = golemSprite;
        isInvulnerable = true;
        // Trigger the golem's attack animation
        animator.SetTrigger("GolemAttack");

        // Start the cooldown
        StartCoroutine(GolemCooldown());

        // Wait for the exact length of the animation clip
        yield return new WaitForSeconds(golemAttackClip.length);
        Debug.Log(golemAttackClip.length);

        

        animator.SetBool("isGolemAttacking", false);

        spriteRenderer.sprite = originalSprite;
        transform.localScale = originalScale;
        // Optionally, revert back to the player's original sprite after the animation finishes
        // Re-enable only the previously active weapon
        if (activeWeapon != null)
        {
            activeWeapon.SetActive(true);
        }

        canMove = true;
        isTransformed = false; // Allow transformation again
        isInvulnerable = false;
    }

    // Coroutine to handle the attack cooldown
    IEnumerator GolemCooldown()
    {
       
        isAttackOnCooldown = true;

        // Reset and activate the cooldown bar
        if (cooldownBar != null)
        {
            cooldownBar.fillAmount = 1f; // Start full
        }

        // Update the cooldown bar over time
        float cooldownTimer = attackCooldown;
        while (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            float fillAmount = cooldownTimer / attackCooldown;
            if (cooldownBar != null)
            {
                cooldownBar.fillAmount = fillAmount;
            }

            yield return null;
        }

        // Ensure the cooldown bar is empty
        if (cooldownBar != null)
        {
            cooldownBar.fillAmount = 1f;
        }

        isAttackOnCooldown = false;
    }


    // Function to enable/disable all child objects of the player (like weapons)
    void SetChildrenActive(bool isActive)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(isActive);
        }
        
    }

    // Function to get the currently active weapon
    GameObject GetActiveWeapon()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
            {
                return child.gameObject;
            }
        }
        return null; // No active weapon found
    }

    // Method to be called by the Animation Event
    public void PlayAttackSound()
    {
        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }
        else
        {
            Debug.LogWarning("Attack sound or AudioSource is not assigned.");
        }
    }

    public void PlayTransformSound()
    {
        if (audioSource != null && transformSound != null)
        {
            audioSource.PlayOneShot(transformSound);
        }
        else
        {
            Debug.LogWarning("Attack sound or AudioSource is not assigned.");
        }
    }

}
