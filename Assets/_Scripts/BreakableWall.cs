using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : Enemy
{
    protected override void setSpeedAndHealth()
    {
        speed = 0f;
        health = 9;
        isAnEnemy= false;
    }

    protected override void move()
    {
        // i put ascii art of a moyai here and it was really funny but i had to remove it because it was unicode
    }

    protected override void attack()
    {
        // i put ascii art of a moyai here and it was really funny but i had to remove it because it was unicode
    }
}
