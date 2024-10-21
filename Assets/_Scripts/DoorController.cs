using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    public GameObject leftDoor;
    public GameObject rightDoor;
    public float openSpeed = 1.0f; // Adjustable speed in the inspector
    public float openDistance = 2.0f; // Distance the doors slide apart
    public bool isOpen = false; // Toggleable in the inspector

    private Vector3 leftClosedPos;
    private Vector3 rightClosedPos;
    private Vector3 leftOpenPos;
    private Vector3 rightOpenPos;

    private Coroutine doorCoroutine;
    private bool previousIsOpen;

    //optimized door and key classes to manage a larger amount of keys/doors
    public int doorID;

    private void Awake()
    {
        GameObject mainManager = GameObject.FindGameObjectWithTag("MainManager");
        mainManager.GetComponent<MainManager>().doors.Add(this.gameObject);
    }

    void Start()
    {
        // Save initial positions as closed positions
        leftClosedPos = leftDoor.transform.localPosition;
        rightClosedPos = rightDoor.transform.localPosition;

        // Calculate open positions
        leftOpenPos = leftClosedPos + new Vector3(-openDistance, 0, 0);
        rightOpenPos = rightClosedPos + new Vector3(openDistance, 0, 0);

        previousIsOpen = isOpen;

        // Set initial door state
        if (isOpen)
        {
            leftDoor.transform.localPosition = leftOpenPos;
            rightDoor.transform.localPosition = rightOpenPos;
            rightDoor.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            leftDoor.transform.localPosition = leftClosedPos;
            rightDoor.transform.localPosition = rightClosedPos;
            rightDoor.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    void Update()
    {
        // Check if the isOpen state has changed
        if (previousIsOpen != isOpen)
        {
            if (isOpen)
            {
                OpenDoor();
            }
            else
            {
                CloseDoor();
            }
            previousIsOpen = isOpen;
        }
    }

    public void OpenDoor()
    {
        if (doorCoroutine != null)
        {
            StopCoroutine(doorCoroutine);
        }
        doorCoroutine = StartCoroutine(MoveDoors(leftOpenPos, rightOpenPos));

        // Disable box collider on the right door
        rightDoor.GetComponent<BoxCollider2D>().enabled = false;
    }

    public void CloseDoor()
    {
        if (doorCoroutine != null)
        {
            StopCoroutine(doorCoroutine);
        }
        doorCoroutine = StartCoroutine(MoveDoors(leftClosedPos, rightClosedPos));

        // Enable box collider on the right door
        rightDoor.GetComponent<BoxCollider2D>().enabled = true;
    }

    IEnumerator MoveDoors(Vector3 leftTargetPos, Vector3 rightTargetPos)
    {
        float time = 0;
        Vector3 leftStartPos = leftDoor.transform.localPosition;
        Vector3 rightStartPos = rightDoor.transform.localPosition;

        while (time < 1f)
        {
            time += Time.deltaTime * openSpeed;

            leftDoor.transform.localPosition = Vector3.Lerp(leftStartPos, leftTargetPos, time);
            rightDoor.transform.localPosition = Vector3.Lerp(rightStartPos, rightTargetPos, time);

            yield return null;
        }

        leftDoor.transform.localPosition = leftTargetPos;
        rightDoor.transform.localPosition = rightTargetPos;

        doorCoroutine = null;
    }
}
