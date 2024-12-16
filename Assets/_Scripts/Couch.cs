using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Couch : MonoBehaviour
{
    public bool receivedMessage = false;

    [SerializeField]
    private Animator sleepMask;
    [SerializeField]
    private PlayerController playerController;

    private void Update()
    {
        if (receivedMessage)
        {
            StartCoroutine(FreezePlayer());
            PersistentDataManager.Instance.currentHealth = PersistentDataManager.Instance.maxHealth;
            receivedMessage = false;
        }
    }


    private IEnumerator FreezePlayer()
    {   
        sleepMask.Play("sleepCycle");
        playerController.LockMovement();
        yield return new WaitForSeconds(2.0f);
        playerController.UnlockMovement();
        sleepMask.Play("idle");
    }
}
