using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if enemy collides with the weapon
        if (collision.gameObject.tag == "Enemy")
        {
            if (tag == "Weapon")
            {
                //deal damage based on the damge of the weapon set enemy to red while taking damage
                collision.gameObject.GetComponent<Enemy>().takeDamage(GetComponent<DamageManager>().damage);
                if(collision.gameObject.GetComponent<SpriteRenderer>() != null)
                {
                    collision.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                }
                else
                {
                   collision.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
                }
            }
        }
        //if player collides with enemy
        if (collision.gameObject.tag == "Player")
        {
            if (tag == "Enemy" || tag == "EnemyProjectile")
            {
                //deal damage based on the damge of the enemy set player to red while taking damage
                collision.gameObject.GetComponent<PlayerController>().takeDamage(GetComponent<DamageManager>().damage);
                collision.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // this section is just to set color back to default

        if (collision.gameObject.tag == "Enemy")
        {
            if (collision.gameObject.GetComponent<SpriteRenderer>() != null)
            {
                collision.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else
            {
                collision.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
            }
        }
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

   
}
