using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : MonoBehaviour
{
    //empty class used to mark artifacts as artifacts (only to be used on Dungeon Artifacts)
    //may add functionality later
    public int ID;

    private void Awake()
    {
        GameObject mainManager = GameObject.FindGameObjectWithTag("MainManager");
        mainManager.GetComponent<MainManager>().artifactsInScene.Add(gameObject);
    }
}
