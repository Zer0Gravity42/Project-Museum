using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeUIController : MonoBehaviour
{
    public Transform playerTransform;   // Drag the player's Transform here in the Inspector
    public Camera mainCamera;           // Assign the main camera in the Inspector
    public RectTransform canvasRect;    // Drag the Canvas RectTransform here
    public Vector3 offset = new Vector3(0, 2f, 0); // Offset above player's head in world units

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        // Get the player's position in world space and add the offset
        Vector3 worldPosition = playerTransform.position + offset;

        // Convert the world position to screen space
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);

        // Convert the screen position to local position in the UI canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPosition,
            mainCamera,
            out Vector2 localPosition
        );

        // Update the slider's position
        GetComponent<RectTransform>().localPosition = localPosition;
    }
}
