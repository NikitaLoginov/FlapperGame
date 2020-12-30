using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupButton : MonoBehaviour
{
    // Forward dash
    float forwardDashSpeed;
    float forwardDashFactor = 5f;
    float forwardDashDuration = 0.1f;
    Button forwardDashButton;

    //Slow Motion
    Button slowMotionButton;

    //Invincibility
    Button invincibilityButton;

    //Managers
    GameManager gameManager;
    TimeManager timeManager;
    SpeedManager speedManager;
    InvincibilityManager invincibilityManager;

    //Temp
    string powerupName;


    void Awake()
    {
        //Managers
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();

        speedManager = GameObject.Find("SpeedManager").GetComponent<SpeedManager>();

        invincibilityManager = GameObject.Find("InvincibilityManager").GetComponent<InvincibilityManager>();

        forwardDashButton = gameManager.ForwardDashButton.GetComponent<Button>();

        slowMotionButton = gameManager.SlowMotionButton.GetComponent<Button>();

        invincibilityButton = gameManager.InvincibilityButton.GetComponent<Button>();

        forwardDashSpeed = speedManager.Speed * forwardDashFactor;
        forwardDashButton.onClick.AddListener(ForwardDash);

        slowMotionButton.onClick.AddListener(SlowMotion);

        invincibilityButton.onClick.AddListener(Invincibility);
    }

    void ForwardDash()
    {
        speedManager.Speed = forwardDashSpeed;
        Debug.Log("Powerup speed: " + forwardDashSpeed);
        gameManager.ForwardDashButton.gameObject.SetActive(false);
        StartCoroutine(ForwardDashSwitcher(forwardDashDuration));
    }

    
    void SlowMotion()
    {
        powerupName = "Slow Motion"; 
        Debug.Log(powerupName + " Initiated");

        //powerup logic
        timeManager.DoSlowMotion();
        gameManager.SlowMotionButton.gameObject.SetActive(false);
    }

    void Invincibility()
    {
        Debug.Log(invincibilityManager.name +" Initiated");

        //powerup logic
        invincibilityManager.IsInvincible = true;

        gameManager.InvincibilityButton.gameObject.SetActive(false);
        StartCoroutine(InvincSwitcher(invincibilityManager.InvincibilityDuration));
    }

    //helpers that counts down time to switch powerup off
    IEnumerator ForwardDashSwitcher(float powerupDuration)
    {
        yield return new WaitForSeconds(powerupDuration);

        //switches speed to normal to finish the dash
        speedManager.Speed = speedManager.NormalSpeed;
        Debug.Log(powerupName + " Stopped");
    }

    IEnumerator InvincSwitcher(float powerupDuration)
    { 
        yield return new WaitForSeconds(powerupDuration);

        if (invincibilityManager.IsInsideObstacle)
        {
            StartCoroutine(CheckIfNotInObstacle(0.1f));
        }
        else
        { 
            invincibilityManager.IsInvincible = false;
        }
    }

    IEnumerator CheckIfNotInObstacle(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("Start checking to end invincibility");
        if (invincibilityManager.IsInsideObstacle)
        {
            StartCoroutine(CheckIfNotInObstacle(time));
        }
        else
        {
            invincibilityManager.IsInvincible = false;
        }
    }
}
