using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if player collides with door
        if (collision.gameObject.tag == "Player")
        {
            //Destroy Key
            Debug.Log("Player picked up key");
            Destroy(this.gameObject);
        }
    }
    
}
