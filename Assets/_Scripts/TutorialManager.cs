using UnityEngine;
using TMPro;  // For TextMeshPro
using System.Collections;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private CanvasGroup tutorialPanelCanvasGroup;
    // Replaces GameObject for panel. We’ll fade this CanvasGroup.

    [SerializeField] private TextMeshProUGUI instructionTextTMP;
    [SerializeField] private float fadeDuration = 0.5f;  // Time to fade in/out

    [Header("Tutorial Steps")]
    [SerializeField] private List<TutorialStep> steps;

    [Header("Portal Settings")]
    [SerializeField] private GameObject portalPrefab;   // Portal prefab to spawn
    [SerializeField] private Transform portalSpawnPoint;

    [Header("Camera Reference")]
    [SerializeField] private CameraFollow cameraFollow;

    private int currentStepIndex = 0;
    private bool stepCompleted = false;

    

    private void Start()
    {
        // Initialize panel alpha to 0 (invisible)
        tutorialPanelCanvasGroup.alpha = 0f;
        StartCoroutine(RunTutorialSequence());
    }

    private IEnumerator RunTutorialSequence()
    {
        while (currentStepIndex < steps.Count)
        {
            // Get current step's text
            string instruction = steps[currentStepIndex].instructionText;
            instructionTextTMP.text = instruction;

            // Fade in the panel
            yield return StartCoroutine(FadeInPanel(fadeDuration));

            // Wait until this step is completed
            stepCompleted = false;
            yield return StartCoroutine(WaitForStepCompletion(currentStepIndex));

            // Optional: show some quick "Step Complete!" effect
            // This could be a short message, a color flash, or a quick icon. 
            // For now, let's just do a short delay.
            yield return new WaitForSeconds(0.3f);

            // Fade out
            yield return StartCoroutine(FadeOutPanel(fadeDuration));

            // Move on to the next step
            currentStepIndex++;
        }

        // After all steps, spawn the portal and do the fancy cutscene flow
        yield return StartCoroutine(SpawnPortalCutsceneFlow());
    }

    private IEnumerator WaitForStepCompletion(int stepIndex)
    {
        while (!stepCompleted)
        {
            switch (stepIndex)
            {
                case 0:
                    // Example: WASD to move
                    if (Input.GetKeyDown(KeyCode.W) ||
                        Input.GetKeyDown(KeyCode.A) ||
                        Input.GetKeyDown(KeyCode.S) ||
                        Input.GetKeyDown(KeyCode.D))
                    {
                        stepCompleted = true;
                       
                    }
                    break;

                case 1:
                    // Left-click
                    if (Input.GetMouseButtonDown(0))
                    {
                        stepCompleted = true;
                        
                    }
                    break;

                case 2:
                    // Scroll wheel or 1 or 2
                    if (Input.GetAxis("Mouse ScrollWheel") != 0f ||
                        Input.GetKeyDown(KeyCode.Alpha1) ||
                        Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        stepCompleted = true;
                        
                    }
                    break;

                case 3:
                    // Space to dash
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        stepCompleted = true;
                    }
                    break;

                case 4:
                    // Press T to use golem attack
                    if (Input.GetKeyDown(KeyCode.T))
                    {
                        stepCompleted = true;
                    }
                    break;

                case 5:
                    // Sword special: press F
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        stepCompleted = true;
                    }   
                    break;

                case 6:
                    // Shotgun fireball: hold right click (detect initial right click down)
                    if (Input.GetMouseButtonDown(1))
                    {
                        stepCompleted = true;
                    }
                    break;
            }

            yield return null;
        }
    }

    // Fade in the tutorial panel from alpha=0 to alpha=1
    private IEnumerator FadeInPanel(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / duration);
            tutorialPanelCanvasGroup.alpha = alpha;
            yield return null;
        }
        tutorialPanelCanvasGroup.alpha = 1f;
    }

    // Fade out the tutorial panel from alpha=1 to alpha=0
    private IEnumerator FadeOutPanel(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(elapsed / duration);
            tutorialPanelCanvasGroup.alpha = alpha;
            yield return null;
        }
        tutorialPanelCanvasGroup.alpha = 0f;
    }

    private IEnumerator SpawnPortalCutsceneFlow()
    {
        // 1. Instantiate the portal
        GameObject portal = Instantiate(portalPrefab, portalSpawnPoint.position, Quaternion.identity);

        // 2. Camera pans to portal (cutscene) over 2 seconds
        cameraFollow.StartCutscene(portal.transform, 2f);
        yield return new WaitForSeconds(2f); // Wait for camera to arrive

        // 3. Now fade in final instruction text
        instructionTextTMP.text = "Enter the portal to begin.";
        yield return StartCoroutine(FadeCanvasGroup(tutorialPanelCanvasGroup, 0f, 1f, fadeDuration));

        // 4. Let them see the portal text for 2 seconds, then camera returns
        yield return new WaitForSeconds(2f);
        cameraFollow.ReturnToPlayer(2f);

        // Wait for return pan
        yield return new WaitForSeconds(2f);

        // Optionally fade out the final tutorial text if desired:
        yield return StartCoroutine(FadeCanvasGroup(tutorialPanelCanvasGroup, 1f, 0f, fadeDuration));
    }

    /// <summary>
    /// Helper function to fade a CanvasGroup between two alpha values over a given duration
    /// </summary>
    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        if (canvasGroup == null) yield break; // Safety check

        float elapsed = 0f;
        canvasGroup.alpha = startAlpha;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }
}
