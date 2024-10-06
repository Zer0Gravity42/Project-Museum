using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArtifactDescription : MonoBehaviour
{
    
    public string popUp; //text for artifact description
    public bool receivedMessage = false; //message trigger
    GameObject MuseumManager; //access the museum manager
    GameObject MainManager;

    public Sprite ArtifactSpriteReference; //sprite reference for ArtifactSlots

    public int ArtifactID;

    bool isArtifactActive = false;

    private void Start()
    {
        MuseumManager = GameObject.FindGameObjectWithTag("MuseumManager"); //get the museum manager to manage artifact slots and pop up system
        MainManager = GameObject.FindGameObjectWithTag("MainManager");

        foreach(int i in MainManager.GetComponent<MainManager>().permanentArtifactIds)
        {
            if(ArtifactID == i) isArtifactActive = true;
        }

        this.gameObject.SetActive(isArtifactActive);
    }

    private void Update()
    {
        if (receivedMessage) //if interaction message gets sent here
        {
            //Debug.Log("Update received message, changed received message to true");
            PopUpSystem pop = MuseumManager.GetComponent<PopUpSystem>(); //pop up system enabled
            pop.PopUp(popUp);
            receivedMessage = false; //disable pop up loop

            MuseumManager.GetComponent<ArtifactSlots>().artifactInteracted = gameObject; //give museum manager's artifact slots a reference to this game object
        }
    }

    
}
