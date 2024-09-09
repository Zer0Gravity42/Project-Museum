using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "TestEnemy") // Ensure the target GameObject's name is "Target"
        {
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "TestEnemy")
        {
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.white; // Change to default or previous color
        }
    }
}
