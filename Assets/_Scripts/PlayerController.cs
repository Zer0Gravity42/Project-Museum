using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private float movementSpeed = 2.0f;
    private Rigidbody2D rb;
    private Vector2 movementDirection;
    private int health = 3;
    
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DungeonPortal")
        {
            Application.LoadLevel("Dungeon");
        }

        if(collision.tag == "MuseumPortal")
        {
            Application.LoadLevel("Museum");
        }
    }
}
