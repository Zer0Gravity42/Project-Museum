using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    //get access to the main manager artifact list
    GameObject mainManager;

    private void Awake()
    {
        mainManager = GameObject.FindGameObjectWithTag("MainManager");
    }

    void DoPickUp()
    {
        
        mainManager.GetComponent<MainManager>().tempArtifacts.Add(gameObject); //add the artifact to the global temporary artifact list
        gameObject.SetActive(false); //disables the interacted item
    }

    void DoDescriptionPopUp()
    {
        //Debug.Log("Received Pop Up message");
        gameObject.GetComponent<ArtifactDescription>().receivedMessage = true;
    }

    void DoPickUpKey()
    {
        //Debug.Log("Player picked up key");
        mainManager.GetComponent<MainManager>().keys.Add(gameObject);
        gameObject.SetActive(false);
    }

    void DoOpenDoor()
    {
        //for now im making the keys generic (as in one key can unlock any door)
        if(mainManager.GetComponent<MainManager>().keys != null)
        {
            gameObject.SetActive(false);
            mainManager.GetComponent<MainManager>().keys.RemoveAt(0);
        }

        //if we want a specific key to open a specific door, uncomment this code and comment out the code above
        /*foreach(GameObject k in mainManager.GetComponent<MainManager>().keys)
        {
            if (k.GetComponent<KeyController>().keyID == gameObject.GetComponent<DoorController>().doorID) //check to see if the player has any key that matches the door
            {
                gameObject.SetActive(false); //remove the door
                mainManager.GetComponent<MainManager>().keys.Remove(k); //remove the key
            }
        }*/

    }

    
}
