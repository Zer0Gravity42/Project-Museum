using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Include TextMeshPro namespace
using UnityEngine.SceneManagement; // Add Scene Management namespace

public class MainManager : MonoBehaviour
{ 
    public static MainManager Instance;

    // All info relating to persisting data
    public List<GameObject> tempArtifacts;
    public List<int> permanentArtifactIds;

    public int equippedSlotOneId;
    public int equippedSlotTwoId;

    public List<GameObject> keys;
    public List<GameObject> doors;
    public List<GameObject> artifactsInScene;

    public int dayNumber = 0; // Number of days passed in the game

    // Reference to the TextMeshPro component for the current day
    public TextMeshProUGUI currentDayText;

    private void Awake()
    {
        if (Instance != null) // If the MainManager has already been copied, don't copy it again
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Makes this object persist across scenes

        // Subscribe to the scene loaded event to re-initialize the current day text when a scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // This method will be called every time a new scene is loaded.
        InitializeDayText();
    }
    
    // All day tracking handling code is in this Region
    #region DayTracking

    private void InitializeDayText()
    {
        // Try to find the TextMeshProUGUI component for the "CurrentDay" GameObject
        GameObject currentDayObject = GameObject.Find("CurrentDay");
        if (currentDayObject != null)
        {
            currentDayText = currentDayObject.GetComponent<TextMeshProUGUI>();
            if (currentDayText != null)
            {
                // Update the text when the scene is loaded
                currentDayText.text = "Day " + dayNumber.ToString() + ":";
            }
            else
            {
                Debug.LogError("TextMeshProUGUI component not found on the 'CurrentDay' GameObject.");
            }
        }
        else
        {
            Debug.LogError("'CurrentDay' GameObject not found in the scene.");
        }
    }
    
    public void IncrementDay()
    {
        dayNumber++; // Increment the day counter
        currentDayText.text = "Day " + dayNumber.ToString() + ":"; // Set the text to "Day X:"
    }

    // Method to reset days passed if needed, for example after an event or condition
    public void ResetDays()
    {
        dayNumber = 1;
        currentDayText.text = "Day " + dayNumber.ToString() + ":"; // Set the text to "Day 1:"
    }

    #endregion

    // Example method for managing artifacts
    void AddAllToPermanents()
    {
        if (tempArtifacts != null) // Check to make sure the temporary list is not empty
        {
            foreach (GameObject g in tempArtifacts) // Loop through the temporary list and add all artifact IDs to the list
            {
                permanentArtifactIds.Add(g.GetComponent<Artifact>().ID);
            }
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event when the object is destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
