using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactSlots : MonoBehaviour
{
    public Button btnSlot1;
    public Button btnSlot2;

    public Image imgSlot1;
    public Image imgSlot2;

    GameObject slot1;
    GameObject slot2;

    public GameObject artifactInteracted;

    private void Start()
    {
        Button btn1 = btnSlot1.GetComponent<Button>();
        Button btn2 = btnSlot2.GetComponent<Button>();

        btn1.onClick.AddListener(AddToSlot1); //event systems wooooo
        btn2.onClick.AddListener(AddToSlot2);
    }

    void AddToSlot1() //on button press (Slot 1 Button)
    {
        //Debug.Log("Slot 1 Button Pressed");
        if ( slot1 == null || slot1 != artifactInteracted) // If slot doesn't have an artifact equipped, add the current artifact you're interacting with to the slot OR replace the artifact equipped
        {
            //Debug.Log("Artifact added to slot 1");
            slot1 = artifactInteracted;
        }
        else if ( slot1 == artifactInteracted) // if the artifact is already equipped in the slot, assume player wishes to remove it, and remove it
        {
            //Debug.Log("Artifact removed from slot 1");
            slot1 = null;
        }


        if ( slot2 == artifactInteracted) // if the artifact is already equipped in a different slot, remove it from that slot and move it to this one
        {
            //Debug.Log("Artifact remvoed from slot 2");
            slot2 = null;
            slot1 = artifactInteracted;
        }

        if (slot1 != null) //if there is a game object in slot 1
        {
            imgSlot1.sprite = slot1.GetComponent<ArtifactDescription>().ArtifactSpriteReference; //get reference to slot 1's sprite reference
            

            if (slot2 == null) //check if slot 2 got nuked from orbit
            {
                imgSlot2.sprite = null; //remove slot 2's sprite reference
                
            }
        }
        else
        {
            imgSlot1.sprite = null; //if we dont have slot 1 (removed by the player), remove sprite reference
           
        }

        LoadEquippedArtifacts();
    }

    void AddToSlot2() // same as AddToSlot1()
    {
        //Debug.Log("Slot 2 Button Pressed");
        if (slot2 == null || slot2 != artifactInteracted) // If slot doesn't have an artifact equipped, add the current artifact you're interacting with to the slot OR replace the artifact equipped
        {
            //Debug.Log("Artifact added to slot 2");
            slot2 = artifactInteracted;
        }
        else if (slot2 == artifactInteracted) // if the artifact is already equipped in the slot, assume player wishes to remove it, and remove it
        {
            //Debug.Log("Artifact removed from slot 2");
            slot2 = null;
        }


        if (slot1 == artifactInteracted) // if the artifact is already equipped in a different slot, remove it from that slot and move it to this one
        {
            //Debug.Log("Artifact remvoed from slot 1");
            slot1 = null;
            slot2 = artifactInteracted;
        }

        if (slot2 != null)
        {
            imgSlot2.sprite = slot2.GetComponent<ArtifactDescription>().ArtifactSpriteReference;


            if (slot1 == null)
            {
                imgSlot1.sprite = null;
                
            }
        }
        else
        {
            imgSlot2.sprite = null;
            
        }

        LoadEquippedArtifacts();
    }

    void OnClose() // when the player closes the pop up screen, make it so that it's not constantly referring to the artifact last interacted with
    {
        artifactInteracted = null; 
    }

    void LoadEquippedArtifacts()
    {
        GameObject mainManager = GameObject.FindGameObjectWithTag("MainManager");
        if (slot1 != null)
        {
            mainManager.GetComponent<MainManager>().equippedSlotOneId = slot1.GetComponent<ArtifactDescription>().ArtifactID;
        }
        else mainManager.GetComponent<MainManager>().equippedSlotOneId = 0;
        if (slot2 != null)
        {
            mainManager.GetComponent<MainManager>().equippedSlotTwoId = slot2.GetComponent<ArtifactDescription>().ArtifactID;
        }
        else mainManager.GetComponent<MainManager>().equippedSlotTwoId = 0;
        
    }
}
