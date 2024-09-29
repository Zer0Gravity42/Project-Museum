using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactDescription : MonoBehaviour
{
    // Empty class for now (only to be attached to museum artifact)
    //Add Text object here so we can attach this script to the artifacts inside the museum
    //and the pop up system can refer to it and get unique texts per artifact
    public string popUp;
    public bool receivedMessage = false;

    private void Update()
    {
        if (receivedMessage) //somehow do this
        {
            Debug.Log("Update received message, changed received message to true");
            PopUpSystem pop = GameObject.FindGameObjectWithTag("MuseumManager").GetComponent<PopUpSystem>();
            pop.PopUp(popUp);

            receivedMessage = false;
        }
    }

    
}
