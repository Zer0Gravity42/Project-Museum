using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashThroughObject : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("hello");
            if(collision.GetComponent<PlayerController>().isDashing == true)
            {
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
