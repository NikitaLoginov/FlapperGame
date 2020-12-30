﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    //Gravity
    [SerializeField]float _verticalThrust = 8;
    [SerializeField]float _gravityModifier = 2f;
    [SerializeField]float _maxSpeed = 11; //magnitude
    const float _gravityConstant = 9.81f;

    //Input
    bool _canMove;

    int _pointValue = 1;
    Rigidbody _playerRb;
    Vector3 _gameOverPos;
    Vector3 _continuePos;
    
    //Managers
    InvincibilityManager _invincibilityManager;


    void Start()
    {
        _playerRb = GetComponent<Rigidbody>();
        _invincibilityManager = GameObject.Find("InvincibilityManager").GetComponent<InvincibilityManager>();

        EventBroker.PlayerControllerContinueHandler += Continue;

        _continuePos = _playerRb.transform.position;
    }

    private void OnDisable()
    {
        EventBroker.PlayerControllerContinueHandler -= Continue;
    }

    public void Continue()
    {
        transform.position = _continuePos;
        EventBroker.CallContinueGame();
    }

    private void Update()
    {
        CheckingTapInput();
    }

    private void CheckingTapInput()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            _canMove = true;
        }
    }

    void FixedUpdate()
    {
        ApplyForce();
        CalculateVelocity();
    }

    private void ApplyForce()
    {
        //if (!gameManager.isGameOver) - old implementation

        // Instead of using property to check if game is over we instead are using event subscribtion to check it.
        // When game is over we unsubscribe from event which in turn make else part start working
        // You probably should test this somehow!
        if (!_invincibilityManager.IsInvincible)
        {
            //Applying constant force to bird to simulate gravity
            _playerRb.AddForce(Vector3.down * _gravityConstant * _gravityModifier, ForceMode.Force);

            //Bird goes up by tapping the screen or pushing a button
            //Checking for input
            if (_canMove)
            {
                _playerRb.AddForce(Vector3.up * _verticalThrust, ForceMode.Impulse);
                _canMove = false;
            }
        }
        else if (_invincibilityManager.IsInvincible)
        {
            _playerRb.Sleep(); // to stop applying force
        }
        else
        {
            transform.position = _gameOverPos;
        }
    }

    private void CalculateVelocity()
    {
        Vector3 velocity = _playerRb.velocity;
        if (velocity.magnitude > _maxSpeed)
        {
            _playerRb.velocity = velocity.normalized * _maxSpeed;
        }

    }

    //Collisions

    private void OnCollisionEnter(Collision collision)
    {
        _gameOverPos = transform.position;
        if (collision.gameObject.CompareTag("Obstacle") && !_invincibilityManager.IsInvincible)
        {
            Debug.Log("Game over!");
            EventBroker.CallGameOver();
        }
        else if (collision.gameObject.CompareTag("Obstacle") && _invincibilityManager.IsInvincible)
        {
            _invincibilityManager.IsInsideObstacle = true;
            Debug.Log("Is inside obstacle: "+_invincibilityManager.IsInsideObstacle);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        { 
            _invincibilityManager.IsInsideObstacle = false;
            Debug.Log("Is inside obstacle: " + _invincibilityManager.IsInsideObstacle);
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
            Debug.Log(other.gameObject.name);
            // set powerup button active
            if (other.gameObject.name == "Dash(Clone)")
            {
                EventBroker.CallForwardDash();
                other.gameObject.SetActive(false); // disable powerup
            }

            if (other.gameObject.name == "Slow(Clone)")
            {
                EventBroker.CallSlowMotion();
                other.gameObject.SetActive(false);
            }

            if (other.gameObject.name == "Invincibility(Clone)")
            {
                EventBroker.CallInvincibility();
                other.gameObject.SetActive(false);
            }
            // remove powerup from queue until it used?
        }
    }

}
