using System;
using UnityEngine;

public class ObstacleCollisionDetection : MonoBehaviour
{
    private InvincibilityManager _invincibilityManager;

    private void Awake()
    {
        _invincibilityManager = GameObject.Find("InvincibilityManager").GetComponent<InvincibilityManager>();
    }

    //to push off hats
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hat"))
        {
            EventBroker.CallKnockDownHat();
        }
        else if (other.CompareTag("Player") && !_invincibilityManager.IsInvincible)
        {
            EventBroker.CallGameOver();
        }
        else if (other.CompareTag("Player") && _invincibilityManager.IsInvincible)
        {
            //destruction animation
            gameObject.SetActive(false);
        }
    }
}
