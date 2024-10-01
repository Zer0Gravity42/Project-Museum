using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    
    [SerializeField] private GameObject key;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //On collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player checking if door can be unlocked");
        //if player collides with door
        if (collision.gameObject.tag == "Player")
        {
            //if key is null, destroy self
            if (key == null)
            {
                Destroy(gameObject);
            }
        }
    }
}
