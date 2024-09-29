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
    
    
    // Start is called before the first frame update
    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
        UpdateAnimation();
        ProcessInteracts(); //interaction system
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
        rb.velocity = new Vector2(movementDirection.x * movementSpeed, movementDirection.y * movementSpeed);
    }

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
            Debug.Log("Interacted with Museum object");
            currentInteraction.SendMessage("DoDescriptionPopUp"); //artifact description interact (museum)
            currentInteraction = null;
        }

        if (Input.GetButtonDown("Interact"))
        {
            Debug.Log("Interacting");
        }
        
    }
}
