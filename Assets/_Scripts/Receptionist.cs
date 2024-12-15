using UnityEngine;

public class ReceptionistNPC : NPC
{
    private bool hasGivenBoost = false; // To ensure the boost is given only once

    public override void StartDialogue()
    {
        if (!dialoguePanel.activeInHierarchy)
        {
            MuseumController museumController = FindObjectOfType<MuseumController>();
            if (museumController != null)
            {
                if (museumController.museumScore >= 10 && !hasGivenBoost)
                {
                    dialogue = new string[] { "Congratulations! You've earned a boost!" };

                    // Increase player's max health
                    PlayerController playerController = FindObjectOfType<PlayerController>();
                    if (playerController != null)
                    {
                        playerController.IncreaseMaxHealth(2);
                        playerController.Heal(2);
                    }

                    hasGivenBoost = true;
                }
                else
                {
                    // Regular dialogue when conditions aren't met
                    dialogue = new string[] { "Welcome to the museum! Check with me to receive boosts when you haave enough artifacts." };
                }
            }

            base.StartDialogue();
        }
    }
}
