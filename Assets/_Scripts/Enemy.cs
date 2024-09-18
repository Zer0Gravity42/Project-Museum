using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public GameObject player;
    protected Vector2 directionToPlayer;
    protected float distanceFromPlayer;
    protected float speed;
    protected int health;
    protected float timer;
    // Start is called before the first frame update
    void Start()
    {
        //sets the attribute for the specific enemy type
        setSpeedAndHealth();
    }

    // Update is called once per frame
    void Update()
    {
        //for use in enemy ai
        timer += Time.deltaTime;

        //every enemy type will probably need these
        directionToPlayer = (transform.position - player.transform.position).normalized;
        distanceFromPlayer = MathF.Abs(transform.position.x - player.transform.position.x) + MathF.Abs(transform.position.y - player.transform.position.y);

        //call move for enemy specific move ai, attack is probably called conditionally within move, we could add it here if its easier
        move();

        //if dead then die
        if(health == 0)
        {
            Destroy(gameObject);
        }
    }

    protected abstract void setSpeedAndHealth();

    protected abstract void move();

    protected abstract void attack();
}
