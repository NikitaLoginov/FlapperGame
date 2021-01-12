using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupButton : MonoBehaviour
{
    // Forward dash
    float _forwardDashSpeed;
    float _forwardDashFactor = 5f;
    float _forwardDashDuration = 0.1f;
    Button _forwardDashButton;

    //Slow Motion
    Button _slowMotionButton;

    //Invincibility
    Button _invincibilityButton;

    //Managers
    GameManager _gameManager;
    TimeManager _timeManager;
    SpeedManager _speedManager;
    InvincibilityManager _invincibilityManager;

    //Temp
    string _powerupName;


    void Awake()
    {
        //Managers
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        _timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();

        _speedManager = GameObject.Find("SpeedManager").GetComponent<SpeedManager>();

        _invincibilityManager = GameObject.Find("InvincibilityManager").GetComponent<InvincibilityManager>();

        _forwardDashButton = _gameManager.ForwardDashButton.GetComponent<Button>();

        _slowMotionButton = _gameManager.SlowMotionButton.GetComponent<Button>();

        _invincibilityButton = _gameManager.InvincibilityButton.GetComponent<Button>();

        _forwardDashSpeed = _speedManager.Speed * _forwardDashFactor;
        _forwardDashButton.onClick.AddListener(ForwardDash);

        _slowMotionButton.onClick.AddListener(SlowMotion);

        _invincibilityButton.onClick.AddListener(Invincibility);
    }

    void ForwardDash()
    {
        _speedManager.Speed = _forwardDashSpeed;
        Debug.Log("Powerup speed: " + _forwardDashSpeed);
        _gameManager.ForwardDashButton.gameObject.SetActive(false);
        StartCoroutine(ForwardDashSwitcher(_forwardDashDuration));
    }

    
    void SlowMotion()
    {
        _powerupName = "Slow Motion"; 
        Debug.Log(_powerupName + " Initiated");

        //powerup logic
        _timeManager.DoSlowMotion();
        _gameManager.SlowMotionButton.gameObject.SetActive(false);
    }

    void Invincibility()
    {
        Debug.Log(_invincibilityManager.name +" Initiated");

        //powerup logic
        _invincibilityManager.IsInvincible = true;

        _gameManager.InvincibilityButton.gameObject.SetActive(false);
        StartCoroutine(InvincSwitcher(_invincibilityManager.InvincibilityDuration));
    }

    //helpers that counts down time to switch powerup off
    IEnumerator ForwardDashSwitcher(float powerupDuration)
    {
        yield return new WaitForSeconds(powerupDuration);

        //switches speed to normal to finish the dash
        _speedManager.Speed = _speedManager.NormalSpeed;
        Debug.Log(_powerupName + " Stopped");
    }

    IEnumerator InvincSwitcher(float powerupDuration)
    { 
        yield return new WaitForSeconds(powerupDuration);

        if (_invincibilityManager.IsInsideObstacle)
        {
            //StartCoroutine(CheckIfNotInObstacle(0.1f));
        }
        else
        { 
            _invincibilityManager.IsInvincible = false;
        }
    }

    IEnumerator CheckIfNotInObstacle(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("Start checking to end invincibility");
        if (_invincibilityManager.IsInsideObstacle)
        {
            StartCoroutine(CheckIfNotInObstacle(time));
        }
        else
        {
            _invincibilityManager.IsInvincible = false;
        }
    }
}
