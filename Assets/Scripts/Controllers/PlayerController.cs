using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    //Gravity
    [SerializeField] private float _verticalThrust = 8;
    [SerializeField] private float _gravityModifier = 2f;
    [SerializeField] private float _maxSpeed = 11; //magnitude
    private const float GravityConstant = 9.81f;

    //Input
    private bool _canMove;

    private Rigidbody _playerRb;
    private Vector3 _gameOverPos;
    public Vector3 GameOverPos { set => _gameOverPos = value; }

    //Managers
    private InvincibilityManager _invincibilityManager;
    
    //Animation
    private Animator _playerAnim;
    private static readonly int Tapped = Animator.StringToHash("Tapped");
    private bool isOnAndroid;


    private void Start()
    {
        _playerRb = GetComponent<Rigidbody>();
        _invincibilityManager = GameObject.Find("InvincibilityManager").GetComponent<InvincibilityManager>();

        _playerAnim = GetComponent<Animator>();

        if (Application.platform == RuntimePlatform.Android)
            isOnAndroid = true;

    }

    private void Update()
    {
        if(isOnAndroid)
            CheckingTapInput();
        else
            CheckingClickInput();
    }

    private void CheckingClickInput()
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

    private void CheckingTapInput()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
                _canMove = true;
                _playerAnim.SetBool(Tapped, true);
            }
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
            _playerRb.AddForce(Vector3.down * (GravityConstant * _gravityModifier), ForceMode.Force); //changed multiplication order paranteces

            //Bird goes up by tapping the screen
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
}
