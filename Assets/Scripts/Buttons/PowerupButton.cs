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
        powerupName = "Forward Dash";

        speedManager.Speed = forwardDashSpeed;
        Debug.Log("Powerup speed: " + forwardDashSpeed);
        gameManager.ForwardDashButton.gameObject.SetActive(false);
        StartCoroutine(PowerupSwitcher(forwardDashDuration, powerupName));
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
        StartCoroutine(PowerupSwitcher(invincibilityManager.InvincibilityDuration, invincibilityManager.Name));
    }

    
    //helper that counts down time to switch powerup off
    IEnumerator PowerupSwitcher(float powerupDuration, string powerupName)
    {
        yield return new WaitForSeconds(powerupDuration);

        if (powerupName == "Forward Dash")
        {
             //switches speed to normal to finish the dash
            speedManager.Speed = speedManager.NormalSpeed;
            Debug.Log(powerupName + " Stopped");
        }

        if (powerupName == "Slow Motion")
        {
            //reverse slomo effect
            Debug.Log(powerupName + " Stopped");
        }

        if (powerupName == invincibilityManager.Name)
        {
            //reverse invincibility effect
            invincibilityManager.IsInvincible = false;

            Debug.Log(powerupName + " Stopped");
        }

        //add powerup to queue so it could spawn again
    }
}
