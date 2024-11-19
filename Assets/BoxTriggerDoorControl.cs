using UnityEngine;

public class BoxTriggerDoorControl : MonoBehaviour
{
    public DoorController doorController;
    public bool permanentlyLocked = false;  // Set this in the inspector to lock the door permanently

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (doorController.isOpen)
        {
            doorController.CloseDoor();
        }
    }

    // Call this method to lock the door permanently from other scripts or internally
    public void LockDoorPermanently()
    {
        permanentlyLocked = true;
    }
}
