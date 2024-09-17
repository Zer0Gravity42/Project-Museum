using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArtifactPopUpSystem : MonoBehaviour
{
    public GameObject popUpBox;
    public Animator animator;
    public TMP_Text popUpText;

    public void PopUp(string text)
    {
        popUpBox.SetActive(true);
        popUpText.text = text;
        animator.SetTrigger("pop");
    }


    // I'll keep working on this - Lucy
}
