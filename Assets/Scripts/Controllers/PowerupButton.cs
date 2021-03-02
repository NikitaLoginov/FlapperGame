using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PowerupButton : MonoBehaviour
{
    // Forward dash
    private float _forwardDashSpeed;
    private float _forwardDashFactor = 5f;
    private float _forwardDashDuration = 0.1f;
    private Button _forwardDashButton;

    //Slow Motion
    private Button _slowMotionButton;

    //Invincibility
    private Button _invincibilityButton;

    //Managers
    private InGameUIManager _inGameUIManager;
    private TimeManager _timeManager;
    private SpeedManager _speedManager;
    private InvincibilityManager _invincibilityManager;
    private void Awake()
    {
        //Managers
        _inGameUIManager = GameObject.Find("InGameUIManager").GetComponent<InGameUIManager>();

        _timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();

        _speedManager = GameObject.Find("SpeedManager").GetComponent<SpeedManager>();

        _invincibilityManager = GameObject.Find("InvincibilityManager").GetComponent<InvincibilityManager>();

        _forwardDashSpeed = _speedManager.Speed * _forwardDashFactor;
        

        EventBroker.DashHandler += ForwardDash;
        EventBroker.SlowMotionHandler += SlowMotion;
        EventBroker.InvincibilityHandler += Invincibility;
    }

    private void ForwardDash()
    {
        _speedManager.Speed = _forwardDashSpeed;
        _inGameUIManager.ForwardDashButton.gameObject.SetActive(false);
        StartCoroutine(ForwardDashSwitcher(_forwardDashDuration));
    }


    private void SlowMotion()
    {
        //powerup logic
        _timeManager.DoSlowMotion();
        _inGameUIManager.SlowMotionButton.gameObject.SetActive(false);
    }

    private void Invincibility()
    {
        //powerup logic
        _invincibilityManager.IsInvincible = true;

        _inGameUIManager.InvincibilityButton.gameObject.SetActive(false);
        StartCoroutine(InvincSwitcher(_invincibilityManager.InvincibilityDuration));
    }

    //helpers that counts down time to switch powerup off
    private IEnumerator ForwardDashSwitcher(float powerupDuration)
    {
        yield return new WaitForSeconds(powerupDuration);

        //switches speed to normal to finish the dash
        _speedManager.NormaliseSpeed();
    }

    private IEnumerator InvincSwitcher(float powerupDuration)
    { 
        yield return new WaitForSeconds(powerupDuration);
        _invincibilityManager.IsInvincible = false;
    }

    private void OnDestroy()
    {
        EventBroker.DashHandler -= ForwardDash;
        EventBroker.SlowMotionHandler -= SlowMotion;
        EventBroker.InvincibilityHandler -= Invincibility;
    }
}
