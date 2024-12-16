using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Couch : MonoBehaviour
{
    public bool receivedMessage = false;


    [SerializeField]
    private GameObject sleepMaskObject;
    [SerializeField]
    private Animator sleepMask;
    [SerializeField]
    private PlayerController playerController;

    private void Awake()
    {
        sleepMaskObject.SetActive(false);
    }

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
        sleepMaskObject.SetActive(true);
        sleepMask.Play("sleepCycle");
        playerController.LockMovement();
        yield return new WaitForSeconds(2.0f);
        playerController.UnlockMovement();
        sleepMask.Play("idle");
        sleepMaskObject.SetActive(false);
    }
}
