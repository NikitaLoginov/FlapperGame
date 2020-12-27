using System;
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

    //Events
    public event Action GameOverHandler;
    public event Action<int> UpdateScoreHandler;
    public event Action ModifyDifficultyHandler;

    public event Action ForwardDashHandler;
    public event Action SlowMoHandler;
    public event Action InvincibilityHandler;
    public event Action ContinueHandler;

    void Start()
    {
        _playerRb = GetComponent<Rigidbody>();
        _invincibilityManager = GameObject.Find("InvincibilityManager").GetComponent<InvincibilityManager>();
        EventBroker.PlayerControllerContinueHandler += Continue;

        _continuePos = _playerRb.transform.position;
    }

    public void Continue()
    {
        transform.position = _continuePos;
        if (ContinueHandler != null)
            ContinueHandler();
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
        if (GameOverHandler != null && !_invincibilityManager.IsInvincible)
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
        else if (GameOverHandler != null && _invincibilityManager.IsInvincible)
        {
            transform.position = _invincibilityManager.InvincibilityPosition;
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
            if (GameOverHandler != null)
                GameOverHandler();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ScoreTrigger"))
        {
            if (UpdateScoreHandler != null)
                UpdateScoreHandler(_pointValue);

            if (ModifyDifficultyHandler != null)
                ModifyDifficultyHandler();

        }
        else if (other.gameObject.CompareTag("Powerup"))
        {
            Debug.Log(other.gameObject.name);
            // set powerup button active
            if (ForwardDashHandler != null && other.gameObject.name == "Dash(Clone)")
            {
                ForwardDashHandler();
                other.gameObject.SetActive(false); // disable powerup
            }

            if (SlowMoHandler != null && other.gameObject.name == "Slow(Clone)")
            {
                SlowMoHandler();
                other.gameObject.SetActive(false);
            }

            if (InvincibilityHandler != null && other.gameObject.name == "Invincibility(Clone)")
            {
                InvincibilityHandler();
                other.gameObject.SetActive(false);
            }
            // remove powerup from queue until it used?
        }
    }

}
