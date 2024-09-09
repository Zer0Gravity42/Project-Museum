using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private float movementSpeed = 2.0f;
    private Rigidbody2D rb;
    private Vector2 movementDirection;
    
    // Start is called before the first frame update
    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
    }
    
    void FixedUpdate()
    {
        Move();
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
}
