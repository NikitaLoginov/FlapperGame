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
    private GameManager _gameManager;
    private TimeManager _timeManager;
    private SpeedManager _speedManager;
    private InvincibilityManager _invincibilityManager;



    private void Awake()
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

    private void ForwardDash()
    {
        _speedManager.Speed = _forwardDashSpeed;
        _gameManager.ForwardDashButton.gameObject.SetActive(false);
        StartCoroutine(ForwardDashSwitcher(_forwardDashDuration));
    }


    private void SlowMotion()
    {
        //powerup logic
        _timeManager.DoSlowMotion();
        _gameManager.SlowMotionButton.gameObject.SetActive(false);
    }

    private void Invincibility()
    {
        //powerup logic
        _invincibilityManager.IsInvincible = true;

        _gameManager.InvincibilityButton.gameObject.SetActive(false);
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

    // private IEnumerator CheckIfNotInObstacle(float time)
    // {
    //     yield return new WaitForSeconds(time);
    //     Debug.Log("Start checking to end invincibility");
    //     if (_invincibilityManager.IsInsideObstacle)
    //     {
    //         StartCoroutine(CheckIfNotInObstacle(time));
    //     }
    //     else
    //     {
    //         _invincibilityManager.IsInvincible = false;
    //     }
    // }
}
