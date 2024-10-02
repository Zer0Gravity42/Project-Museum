using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    //player attributes
    public GameObject player;
    protected Vector2 directionToPlayer;
    protected float distanceFromPlayer;
    protected Animator anim;
    //enemy atributes
    protected float speed;
    public int health;
    protected int maxHealth;
    //for use in ai behaviour
    protected float timer;
    protected bool moving = true;
    protected bool attacking = false;
    protected bool alive = true;
    protected bool awake = false;

    //public int Health { get { return health; } }
    //public int MaxHealth { get { return maxHealth; } }

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        //sets the attributes for the specific enemy type
        setSpeedAndHealth();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //for use in enemy ai
        timer += Time.deltaTime;

        //every enemy type will probably need these
        directionToPlayer = (transform.position - player.transform.position).normalized;
        distanceFromPlayer = MathF.Abs(transform.position.x - player.transform.position.x) + MathF.Abs(transform.position.y - player.transform.position.y);

        if(alive && awake)
        {
            //call move and attack with specific ai for each enemy
            move();
            attack();
        }

        //if dead then die
        if(health <= 0 && alive)
        {
            anim.SetBool("dead", true);
            alive = false;
            timer = 0;
        }

        if (!alive && timer >= 1)
        {
            Destroy(gameObject);
        }
        if(!awake && timer >= 1)
        {
            awake= true;
            anim.SetBool("awake", true);
        }
    }

    public virtual void takeDamage (int damage)
    {
        health -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Health is now {health}.");
    }

    protected abstract void setSpeedAndHealth();

    protected abstract void move();

    protected abstract void attack();
}
