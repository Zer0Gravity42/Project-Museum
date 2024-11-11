using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float FollowSpeed = 1.5f;
    public float yOffset = 1f;
    public Transform target;

    // Zoom variables
    [Header("Zoom Settings")]
    public float defaultSize = 5f;    // The default orthographic size of the camera
    public float zoomedOutSize = 7f;  // The zoomed-out orthographic size when transformed
    public float zoomSpeed = 2f;      // Speed at which the camera zooms in/out

    // Screen shake variables
    [Header("Screen Shake Settings")]
    public float shakeDuration = 0.2f; // Duration of the shake effect
    public float shakeMagnitude = 0.3f; // Intensity of the shake

    private Camera cam;          // Reference to the Camera component
    private float targetSize;    // The desired orthographic size
    private Coroutine zoomCoroutine; // Reference to the current zoom coroutine

    private bool isCutscene = false;

    void Start()
    {
        cam = GetComponent<Camera>();
        targetSize = defaultSize; // Initialize to default size
        cam.orthographicSize = defaultSize;
    }

    void Update()
    {
        if (!isCutscene)
        {
            // Normal camera follow
            Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, -10f);
            transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
        }

        // Smoothly adjust the camera's orthographic size to the targetSize
        if (cam.orthographicSize != targetSize)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);
        }
    }
    // Method to start the cutscene camera movement
    public void StartCutscene(Transform targetTransform, float duration)
    {
        if (!isCutscene)
        {
            StartCoroutine(CutsceneRoutine(targetTransform, duration));
        }
    }
    // Method to return the camera to the player
    public void ReturnToPlayer(float duration)
    {
        if (isCutscene)
        {
            StartCoroutine(ReturnToPlayerRoutine(duration));
        }
    }

    private IEnumerator CutsceneRoutine(Transform targetTransform, float duration)
    {
        isCutscene = true;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new Vector3(targetTransform.position.x, targetTransform.position.y, -10f);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }

    private IEnumerator ReturnToPlayerRoutine(float duration)
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y + yOffset, -10f);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        isCutscene = false;
    }
    // Call this method to zoom out
    public void ZoomOut()
    {
        targetSize = zoomedOutSize;
    }

    // Call this method to zoom in
    public void ZoomIn()
    {
        targetSize = defaultSize;
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
