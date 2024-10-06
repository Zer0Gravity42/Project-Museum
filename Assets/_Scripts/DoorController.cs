using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    //optimized door and key classes to manage a larger amount of keys/doors
    public int doorID;

    private void Awake()
    {
        GameObject mainManager = GameObject.FindGameObjectWithTag("MainManager");
        mainManager.GetComponent<MainManager>().doors.Add(this.gameObject);
    }
}
