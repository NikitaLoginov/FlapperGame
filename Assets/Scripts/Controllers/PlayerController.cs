using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    //Gravity
    [SerializeField] private float _verticalThrust = 8;
    [SerializeField] private float _gravityModifier = 2f;
    [SerializeField] private float _maxSpeed = 11; //magnitude
    private const float _gravityConstant = 9.81f;

    //Input
    private bool _canMove;

    private int _pointValue = 1;
    private int _regularCoinValue = 1;
    private int _bigCoinValue = 10;
    private Rigidbody _playerRb;
    private Vector3 _gameOverPos;
    private Vector3 _continuePos;
    
    //Managers
    private InvincibilityManager _invincibilityManager;
    
    //Animation
    private Animator _playerAnim;
    private static readonly int Tapped = Animator.StringToHash("Tapped");


    private void Start()
    {
        _playerRb = GetComponent<Rigidbody>();
        _invincibilityManager = GameObject.Find("InvincibilityManager").GetComponent<InvincibilityManager>();

        _playerAnim = GetComponent<Animator>();

        _continuePos = _playerRb.transform.position;
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
            _playerAnim.SetBool(Tapped, true);
        }
    }

    private void FixedUpdate()
    {
        ApplyForce();
        CalculateVelocity();
    }

    private void ApplyForce()
    {
        // Instead of using property to check if game is over we instead are using event subscribtion to check it.
        // When game is over we unsubscribe from event which in turn make else part start working
        // You probably should test this somehow!
        if (!_invincibilityManager.IsInvincible)
        {
            //Applying constant force to bird to simulate gravity
            _playerRb.AddForce(Vector3.down * (_gravityConstant * _gravityModifier), ForceMode.Force); //changed multiplication order paranteces

            //Bird goes up by tapping the screen or pushing a button
            //Checking for input
            if (_canMove)
            {
                _playerRb.AddForce(Vector3.up * _verticalThrust, ForceMode.Impulse);
                _canMove = false;
                _playerAnim.SetBool(Tapped, false);
            }
        }
        else if (_invincibilityManager.IsInvincible)
        {
            _playerRb.Sleep(); // to stop applying force
            _playerAnim.SetBool(Tapped,true);
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
            //Game Over!
            EventBroker.CallGameOver();
        }
        else if (collision.gameObject.CompareTag("Obstacle") && _invincibilityManager.IsInvincible)
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
