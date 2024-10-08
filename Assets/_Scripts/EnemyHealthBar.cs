using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider; // Assign this in the Inspector
    private int maxHealth;

    public void SetMaxHealth(int health)
    {
        maxHealth = health;
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }

    void LateUpdate()
    {
        // Ensure the health bar's scale on X is always positive
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        transform.localScale = scale;

        // Optionally, reset rotation to prevent any unintended rotation
        transform.rotation = Quaternion.identity;
    }
}
