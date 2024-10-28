using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float FollowSpeed = 1.5f;
    public float yOffset = 1f;
    public Transform target;

    // Screen shake variables
    public float shakeDuration = 0.2f; // Duration of the shake effect
    public float shakeMagnitude = 0.3f; // Intensity of the shake

    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
    }

    public void TriggerShake()
    {
        StartCoroutine(ScreenShake());
    }

    private IEnumerator ScreenShake()
    {
        Vector3 originalPos = transform.position;
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;

            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;
            transform.position = new Vector3(originalPos.x + offsetX, originalPos.y + offsetY, originalPos.z);

            yield return null;
        }

        transform.position = originalPos; // Reset to original position
    }
}
