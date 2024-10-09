using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    public float amplitude = 0.5f;   // Controls how high the object floats
    public float frequency = 1f;     // Controls how fast the object floats
    public GameObject shadow;        // Assign the shadow object (the circle) in the Inspector
    public float maxShadowScaleX = 1.0f;  // Max X size of the shadow when object is low
    public float minShadowScaleX = 0.7f;  // Min X size of the shadow when object is high
    public float scaleSpeed = 1f;    // Controls how fast the shadow scales

    private Vector3 startPos;
    private Vector3 shadowStartPos;

    void Start()
    {
        // Store the starting position of the object
        startPos = transform.position;
        // Store the starting position of the shadow
        shadowStartPos = shadow.transform.position;
    }

    void Update()
    {
        // Floating effect for the object
        float newY = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(startPos.x, startPos.y + newY, startPos.z);

        // Lock the shadow's position to its starting position
        shadow.transform.position = new Vector3(shadowStartPos.x, shadowStartPos.y, shadowStartPos.z);

        // Calculate a normalized value based on the object's height relative to the start position
        float heightFactor = (startPos.y + newY - startPos.y) / amplitude;

        // Lerp between min and max X shadow scales based on the height
        float shadowScaleX = Mathf.Lerp(minShadowScaleX, maxShadowScaleX, 1 - Mathf.Abs(heightFactor) * scaleSpeed);

        // Apply the scale to the shadow's X axis only, keeping the Y and Z scale unchanged
        shadow.transform.localScale = new Vector3(shadowScaleX, shadow.transform.localScale.y, shadow.transform.localScale.z);
    }
}
