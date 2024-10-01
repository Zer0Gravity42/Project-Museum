using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private float movementSpeed = 2.0f;
    private Rigidbody2D rb;
    private Vector2 movementDirection;
    public Animator animator;
    private int health = 3;
    private GameObject currentInteraction = null; //This controls our interaction system, basically what the interactable object we are currently in contact with


    #region Dash Variables
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;  //time before dash can be used again
    private bool isDashing = false;
    private float lastDashTime; //timestamp for last dash

    private SpriteRenderer spriteRenderer;
    public float afterImageInterval = 0.05f;
    private float afterImageTimer = 0f;
    #endregion

    // Start is called before the first frame update
    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
        UpdateAnimation();
        ProcessInteracts(); //interaction system

        // Handle Dash input
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(Dash());
        }
    }
    
    void FixedUpdate()
    {
        Move();
        if(health == 0)
        {
            Application.LoadLevel("Main Menu");
        }
    }

    public void takeDamage(int damage)
    {
        health -= damage;
    }
    
    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); //Only 0 and 1. Can be changed to GetAxis for slight movement (controller)
        float moveY = Input.GetAxisRaw("Vertical");

        
        
        movementDirection = new Vector2(moveX, moveY).normalized; //Normalizing this keeps the speed consistent on diagonals
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
        isDashing = true;
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
        rb.velocity = Vector2.zero;
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
            Application.LoadLevel("Dungeon"); //loads the dungeon scene
        }

        if(collision.tag == "MuseumPortal")
        {
            Application.LoadLevel("Museum"); //loads the dungeon system
        }

        if(collision.CompareTag("InteractObject"))
        {
            //Debug.Log(collision.name);
            currentInteraction = collision.gameObject; //Assing interaction object (usable for both dialogue and pickups)          
        }
    }

    private void ProcessInteracts()
    {
        if(Input.GetButtonDown("Interact") && currentInteraction && currentInteraction.GetComponent<Artifact>()) //sends a message to object interaction system only if we're interacting with something
        {
            currentInteraction.SendMessage("DoPickUp"); //the method inside
            currentInteraction = null;
        }

        if(Input.GetButtonDown("Interact") && currentInteraction && currentInteraction.GetComponent<ArtifactDescription>())
        {
            //Debug.Log("Interacted with Museum object");
            currentInteraction.SendMessage("DoDescriptionPopUp"); //artifact description interact (museum)
            currentInteraction = null;
        }

        if (Input.GetButtonDown("Interact"))
        {
            Debug.Log("Interacting");
        }
        
    }
}
