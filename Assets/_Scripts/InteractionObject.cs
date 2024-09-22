using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    //get access to the main manager artifact list
    public GameObject mainManager;

    void DoInteraction()
    {
        if (gameObject.GetComponent<Artifact>()) //check if interacted object is an artifact
        {
            mainManager.GetComponent<MainManager>().artifacts.Add(gameObject.name); //add the artifact to the global artifact list
        }

        gameObject.SetActive(false); //disables the interacted item
    }
}
