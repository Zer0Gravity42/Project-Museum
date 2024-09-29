using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    //get access to the main manager artifact list
    public GameObject mainManager;
    

    void DoPickUp()
    {
        
        mainManager.GetComponent<MainManager>().artifacts.Add(gameObject.name); //add the artifact to the global artifact list
        gameObject.SetActive(false); //disables the interacted item
    }

    void DoDescriptionPopUp()
    {
        Debug.Log("Received Pop Up message");
        gameObject.GetComponent<ArtifactDescription>().receivedMessage = true;
    }
}
