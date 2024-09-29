using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonTriggerArea : MonoBehaviour
{
    private MeleeSkeleton skeletonParent;

    private void Awake()
    {
        skeletonParent = GetComponentInParent<MeleeSkeleton>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            skeletonParent.target = collider.transform;
            skeletonParent.inRange = true;
            skeletonParent.hotZone.SetActive(true);
        }
    }
}
