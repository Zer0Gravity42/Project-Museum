using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LevelTimerUI : MonoBehaviour
{
    private float timer = 600;
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
