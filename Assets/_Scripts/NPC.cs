using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
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

    private Animalese _animalese;
    private bool conversationOver = false; // Flag
    
    void Start()
    {
        dialogueText.text = "";
        
        //Get animalese rreference
        _animalese = GetComponent<Animalese>();
        //InvokeRepeating ("ChangeWhoIsSpeaking", 0.0f, 3.0f);
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
                conversationOver = false; // Reset flag
                _animalese.Speak(dialogue[index]);
                StartCoroutine(Typing());
            }
            else if (dialogueText.text == dialogue[index] && !conversationOver)
            {
                NextLine();
                if (!conversationOver) // Check if conversation isn't over
                {
                    _animalese.Speak(dialogue[index]);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && dialoguePanel.activeInHierarchy)
        {
            _animalese.StopSpeaking();
            Reset();
        }
    }

    public void Reset()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
        conversationOver = true; // Set flag
    }

    IEnumerator Typing()
    {
        foreach(char letter in dialogue[index].ToCharArray())
        {
            if (playerIsClose == false)
            {
                _animalese.StopSpeaking();
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
            _animalese.StopSpeaking();
            conversationOver = true; //Redundant but just in case
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