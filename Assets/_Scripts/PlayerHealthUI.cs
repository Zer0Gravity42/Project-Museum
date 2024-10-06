using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private PlayerController player; // Reference to the PlayerController
    [SerializeField] private Image heartPrefab; // Prefab or UI Image for a heart
    [SerializeField] private Sprite fullHeartSprite; // Sprite for a full heart
    [SerializeField] private Sprite emptyHeartSprite; // Sprite for an empty heart
    [SerializeField] private int heartSpacing = 10; // Spacing between hearts

    private List<Image> heartImages = new List<Image>();

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("PlayerController reference is missing in HealthUI.");
            return;
        }

        // Subscribe to the OnHealthChanged event
        player.OnHealthChanged += UpdateHearts;

        // Initialize hearts based on maxHealth
        InitializeHearts(player.maxHealth, player.health);
    }

    /// <summary>
    /// Initializes the heart UI based on maximum health.
    /// </summary>
    /// <param name="maxHealth">The maximum health value.</param>
    /// <param name="currentHealth">The current health value.</param>
    void InitializeHearts(int maxHealth, int currentHealth)
    {
        // Clear existing hearts if any
        foreach (var heart in heartImages)
        {
            Destroy(heart.gameObject);
        }
        heartImages.Clear();

        // Create heart images based on maxHealth
        for (int i = 0; i < maxHealth; i++)
        {
            Image heart = Instantiate(heartPrefab, transform);
            RectTransform rt = heart.GetComponent<RectTransform>();

            // Position hearts horizontally with spacing
            rt.anchoredPosition = new Vector2(i * (rt.sizeDelta.x + heartSpacing), 0);
            rt.localScale = Vector3.one; // Ensure scale is correct

            // Set the initial sprite based on current health
            if (i < currentHealth)
            {
                heart.sprite = fullHeartSprite;
            }
            else
            {
                heart.sprite = emptyHeartSprite;
            }

            heartImages.Add(heart);
        }
    }

    /// <summary>
    /// Updates the heart UI when health changes.
    /// </summary>
    /// <param name="currentHealth">The current health value.</param>
    /// <param name="maxHealth">The maximum health value.</param>
    void UpdateHearts(int currentHealth, int maxHealth)
    {
        // If maxHealth has increased, add more hearts
        if (maxHealth > heartImages.Count)
        {
            for (int i = heartImages.Count; i < maxHealth; i++)
            {
                Image heart = Instantiate(heartPrefab, transform);
                RectTransform rt = heart.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(i * (rt.sizeDelta.x + heartSpacing), 0);
                rt.localScale = Vector3.one;

                // Initially, the new heart is full if currentHealth >= new index
                if (i < currentHealth)
                {
                    heart.sprite = fullHeartSprite;
                }
                else
                {
                    heart.sprite = emptyHeartSprite;
                }

                heartImages.Add(heart);
            }
        }
        // If maxHealth has decreased, remove excess hearts
        else if (maxHealth < heartImages.Count)
        {
            for (int i = heartImages.Count - 1; i >= maxHealth; i--)
            {
                Destroy(heartImages[i].gameObject);
                heartImages.RemoveAt(i);
            }
        }

        // Update heart sprites based on currentHealth
        for (int i = 0; i < heartImages.Count; i++)
        {
            if (i < currentHealth)
            {
                heartImages[i].sprite = fullHeartSprite;
            }
            else
            {
                heartImages[i].sprite = emptyHeartSprite;
            }
        }
    }

    private void OnDestroy()
    {
        if (player != null)
        {
            player.OnHealthChanged -= UpdateHearts;
        }
    }
}
