using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnim : MonoBehaviour
{
    GameObject menu;
    Animator anim;

    public void MenuMove()
    {

        anim.SetTrigger("move");

    }
}
