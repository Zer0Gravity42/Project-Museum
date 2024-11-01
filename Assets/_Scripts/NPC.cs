using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPC : MonoBehaviour
{
    public GameObject dialoguePanel;
    public string myName;
    public Sprite myPortrait;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI dialogueName;
    public Image dialoguePortrait;
    public string[] dialogue;
    private int index = 0;

    public float wordSpeed;
    public bool playerIsClose;


    void Start()
    {
        dialogueText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsClose)
        {
            if (!dialoguePanel.activeInHierarchy)
            {
                dialoguePanel.SetActive(true);
                dialoguePortrait.sprite = myPortrait;
                dialogueName.text = myName;
                StartCoroutine(Typing());
            }
            else if (dialogueText.text == dialogue[index])
            {
                NextLine();
            }

        }
        if (Input.GetKeyDown(KeyCode.Q) && dialoguePanel.activeInHierarchy)
        {
            Reset();
        }
    }

    public void Reset()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }

    IEnumerator Typing()
    {
        foreach(char letter in dialogue[index].ToCharArray())
        {
            if (playerIsClose == false)
            {
                Reset();
                yield break;
            }
            else
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(wordSpeed);
            }
        }
    }

    public void NextLine()
    {
        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            Reset();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            Reset();
        }
    }
}