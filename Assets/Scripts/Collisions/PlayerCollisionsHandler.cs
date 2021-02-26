using System;
using UnityEngine;

public class PlayerCollisionsHandler : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    private Vector3 _gameOverPos;
    //Managers
    [SerializeField] private InvincibilityManager invincibilityManager;
    
    //Point Values
    private int _pointValue = 1;
    private int _regularCoinValue = 1;
    private int _bigCoinValue = 10;


    private void OnCollisionEnter(Collision collision)
    {
        _gameOverPos = transform.position;
        if (collision.gameObject.CompareTag("Obstacle") && !invincibilityManager.IsInvincible)
        {
            //Game Over!
            playerController.GameOverPos = _gameOverPos;
            EventBroker.CallGameOver();
        }
        else if (collision.gameObject.CompareTag("Obstacle") && invincibilityManager.IsInvincible)
        {
            //destruction animation
            collision.collider.gameObject.SetActive(false); // turns off whole obstacle
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ScoreTrigger"))
        {
            EventBroker.CallUpdateScore(_pointValue);

            EventBroker.CallDifficultyModifier();

        }
        else if (other.gameObject.CompareTag("Powerup"))
        {
            // set powerup button active
            if (other.gameObject.name == "Dash")
            {
                EventBroker.CallForwardDash();
                other.gameObject.SetActive(false); // disable powerup
            }

            if (other.gameObject.name == "Slow")
            {
                EventBroker.CallSlowMotion();
                other.gameObject.SetActive(false);
            }

            if (other.gameObject.name == "Invincibility")
            {
                EventBroker.CallInvincibility();
                other.gameObject.SetActive(false);
            }
        }
        else if (other.gameObject.CompareTag("Coin"))
        {
            other.gameObject.SetActive(false);
            EventBroker.CallUpdateScore(_regularCoinValue);
        }
        else if (other.gameObject.CompareTag("BCoin"))
        {
            other.gameObject.SetActive(false);
            EventBroker.CallUpdateScore(_bigCoinValue);
        }
    }
}
