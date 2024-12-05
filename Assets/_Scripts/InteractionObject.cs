using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    //get access to the main manager artifact list
    GameObject mainManager;
    public string message;

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
        // For now, we're making the keys generic (one key can unlock any door)
        if (mainManager.GetComponent<MainManager>().keys.Count != 0)
        {
            mainManager.GetComponent<MainManager>().keys.RemoveAt(0);

            // Get the DoubleDoorController and open the door
            DoorController doubleDoorController = GetComponent<DoorController>();
            if (doubleDoorController != null)
            {
                doubleDoorController.isOpen = true; // This will trigger the door to open
            }
            SetText("");
        }
        else
        {
            DoorController doubleDoorController = GetComponent<DoorController>();
            doubleDoorController.PlayDeclineSound();
            SetText("Locked");
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
    
    //Elevator
    void DoElevator()
    {
        Debug.Log("Interaction Manager: DoElevator");
        this.GetComponent<ElevatorController>().OpenElevatorUI();
    }
    
    //NPCs
    void DoNPC()
    {
        Debug.Log("Interaction Manager: DoNPC");
        NPC npc = GetComponent<NPC>();
        npc.StartDialogue();
    }

    public void SetText(string text)
    {
        gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = text;
        Debug.Log(text);
    }

    void DoExitTutorial()
    {
        Application.LoadLevel("Museum");
    }
    
}
