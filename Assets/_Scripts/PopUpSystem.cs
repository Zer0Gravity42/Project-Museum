using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpSystem : MonoBehaviour
{
    public GameObject popUpBox; //pop up box in UI
    public Animator animator; //Animator for the pop up box
    public TMP_Text popUpText; //Text replaceable by ArtifactDescription's string 

    public void PopUp(string text)
    {
        popUpBox.SetActive(true); //ensure the pop up box is visible
        popUpText.text = text;
        animator.SetTrigger("pop"); //trigger for animator's pop (opens up the pop up box)
    }

}
