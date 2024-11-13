using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public TMP_Text damageText; // Reference to the Text component displaying damage

    private void Start()
    {
        // Destroy the popup after 0.5 seconds
        Invoke("DestroySelf", 0.5f);
    }

    public void SetDamage(int damage)
    {
        damageText.text = damage.ToString();
    }
    
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
