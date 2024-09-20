using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SpectralEnemy")
        {
            if (tag == "Weapon")
            {
                collision.gameObject.GetComponent<Enemy>().takeDamage(1);
                collision.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
        if (collision.gameObject.tag == "Player")
        {
            if (tag == "Enemy")
            {
                collision.gameObject.GetComponent<PlayerController>().takeDamage(1);
                collision.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SpectralEnemy")
        {
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.white; // Change to default or previous color
        }
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (tag == "Weapon")
            {
                collision.gameObject.GetComponent<Enemy>().takeDamage(1);
                collision.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
            if (collision.gameObject.tag == "Player")
            {
                if (tag == "Enemy")
                {
                    collision.gameObject.GetComponent<PlayerController>().takeDamage(1);
                    collision.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.white; // Change to default or previous color
        }
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
