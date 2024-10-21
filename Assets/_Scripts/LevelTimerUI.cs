using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LevelTimerUI : MonoBehaviour
{
    public float timer;
    public GameObject player;

    
    void Update()
    {
        //decrement timer, if timer = 0 kill the player
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            player.GetComponent<PlayerController>().die();
        }

        //set UI
        gameObject.GetComponent<TMPro.TextMeshProUGUI>().text =(int)(timer / 60) + ":" + (int)(timer % 60);
    }
}
