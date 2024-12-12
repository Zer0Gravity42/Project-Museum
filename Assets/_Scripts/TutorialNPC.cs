using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialNPC : NPC
{
    [SerializeField] private Transform portal;
    [SerializeField] private Transform trainingRoom;
    [SerializeField] private Transform couch;
    [SerializeField] private Transform crystal;
    private bool hasCutsceneRun = false;

    private void Update()
    {
        if (!hasCutsceneRun && (index == 3 || index == 4 || index == 5 || index == 7))
        {
            
            StartCoroutine(TutorialCutscene(base.index));
            
        }

        if (Input.GetKeyDown(KeyCode.E) && dialoguePanel.activeInHierarchy)
        {
            hasCutsceneRun = false;
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

    public override void StartDialogue()
    {
        base.StartDialogue();
        
    }

    private IEnumerator TutorialCutscene(int index)
    {
        Debug.Log("Coroutine Started! Index: " + index);
        hasCutsceneRun = true;
        // Lock player input
        base.playerController.LockMovement();

        if (index == 3)
        {
            cameraFollow.StartCutscene(portal, 1.0f);

            yield return new WaitForSeconds(3.0f);

            // Return the camera to the player
            cameraFollow.ReturnToPlayer(1.0f);

            // Wait for the camera to return
            yield return new WaitForSeconds(1.0f);
        }
        if (index == 4) 
        {
            
            cameraFollow.StartCutscene(couch, 1.0f);

            yield return new WaitForSeconds(3.0f);

            // Return the camera to the player
            cameraFollow.ReturnToPlayer(1.0f);

            // Wait for the camera to return
            yield return new WaitForSeconds(1.0f);
        }
        if (index == 5) 
        {
            cameraFollow.StartCutscene(trainingRoom, 1.0f);
            yield return new WaitForSeconds(3.0f);

            // Return the camera to the player
            cameraFollow.ReturnToPlayer(1.0f);

            // Wait for the camera to return
            yield return new WaitForSeconds(1.0f);
        }
        if (index == 7) 
        {
            cameraFollow.StartCutscene(crystal, 1.0f);
            yield return new WaitForSeconds(2.0f);

            // Return the camera to the player
            cameraFollow.ReturnToPlayer(1.0f);

            // Wait for the camera to return
            yield return new WaitForSeconds(1.0f);

        }
        playerController.UnlockMovement();

    }
}
