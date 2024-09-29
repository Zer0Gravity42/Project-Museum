using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotZoneCheck : MonoBehaviour
{
    private MeleeSkeleton skeletonParent;
    private bool inRange;
    private Animator anim;

    private void Awake()
    {
        skeletonParent = GetComponentInParent<MeleeSkeleton>();
        anim = GetComponentInParent<Animator>();
    }

    private void Update()
    {
        if (inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("Skeleton_Attack")) 
        {
            skeletonParent.Flip();
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            inRange = false;
            gameObject.SetActive(false);
            skeletonParent.triggerArea.SetActive(true);
            skeletonParent.inRange = false;
            skeletonParent.SelectTarget();
        }
    }
}
