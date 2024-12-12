using System.Collections;
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
    protected int index = 0;

    public float wordSpeed;
    public bool playerIsClose;

    protected Animalese _animalese;
    protected bool conversationOver = false;

    [SerializeField] private string[] firstDialogue;      // For the first time (0)
    [SerializeField] private string[] secondDialogue;     // For 1-3 visits
    [SerializeField] private string[] thirdDialogue;      // For 4-5 visits
    [SerializeField] private string[] defaultDialogue;    // For 6+ visits
    
    // New serialized fields
    public DoorController doorController;
    [SerializeField] protected PlayerController playerController;
    [SerializeField] protected CameraFollow cameraFollow;
    [SerializeField] private Transform doorTransform;
    [SerializeField] private Transform cutsceneCameraTarget;
    
    // Add a bool to control cutscene activation
    [SerializeField] private bool hasCutscene = false;

    protected string[] dialogue; // Holds the current dialogue
    private int dayNumber = 1; // Tracks the number of visits

    void Start()
    {
        dialogueText.text = "";
        _animalese = GetComponent<Animalese>();

        // Retrieve the dungeonVisits count from MainManager
        if (MainManager.Instance != null)
        {
            dayNumber = MainManager.Instance.dayNumber;
        }

        SetDialogueBasedOnVisits();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && dialoguePanel.activeInHierarchy)
        {
            if (dialogueText.text == dialogue[index] && !conversationOver)
            {
                NextLine();
                if (!conversationOver)
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
        conversationOver = true;
    }

    IEnumerator Typing()
    {
        foreach (char letter in dialogue[index].ToCharArray())
        {
            if (!playerIsClose)
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
            conversationOver = true;
            Reset();
            // Check if the cutscene should start
            if (hasCutscene)
            {
                StartCoroutine(Cutscene());
            }
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

    public virtual void StartDialogue()
    {
        if (!dialoguePanel.activeInHierarchy)
        {
            dialoguePanel.SetActive(true);
            dialoguePortrait.sprite = myPortrait;
            dialogueName.text = myName;
            conversationOver = false;
            _animalese.Speak(dialogue[index]);
            StartCoroutine(Typing());
        }
    }

    private void SetDialogueBasedOnVisits()
    {
        if (dayNumber == 1)
        {
            dialogue = firstDialogue;
        }
        else if (dayNumber >= 2 && dayNumber <= 3)
        {
            dialogue = secondDialogue;
        }
        else if (dayNumber >= 4 && dayNumber <= 5)
        {
            dialogue = thirdDialogue;
        }
        else
        {
            dialogue = defaultDialogue;
        }
    }
    
    // New coroutine for the cutscene
    private IEnumerator Cutscene()
    {
        // Lock player input
        playerController.LockMovement();

        // Move the camera to the cutscene target
        cameraFollow.StartCutscene(cutsceneCameraTarget, 1.0f);

        // Wait for the camera to reach the target
        yield return new WaitForSeconds(1.0f);

        // Open the door
        doorController.OpenDoor();

        // Wait for the door to open
        yield return new WaitForSeconds(2.0f);

        // Return the camera to the player
        cameraFollow.ReturnToPlayer(1.0f);

        // Wait for the camera to return
        yield return new WaitForSeconds(1.0f);

        // Unlock player input
        playerController.UnlockMovement();
    }
}
