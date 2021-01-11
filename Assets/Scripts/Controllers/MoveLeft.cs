using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    float _leftBound = -30.0f;
    float _leftCloudBound = -100f;
    GameManager _gameManager;
    SpeedManager _speedManager;

    void Start()
    {
        _speedManager = GameObject.Find("SpeedManager").GetComponent<SpeedManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }


    void Update()
    {
        if (!_gameManager.IsGameOver)
        { 
            MoveLeftWithSpeed();
            TurnOffIfOutOfBounds();
        }
    }

    private void TurnOffIfOutOfBounds()
    {
        if (transform.position.x < _leftBound && gameObject.CompareTag("Obstacle"))
        {
            gameObject.SetActive(false);
        }
        else if (transform.position.x < _leftBound && gameObject.CompareTag("Powerup"))
        {
            gameObject.SetActive(false);
        }
        else if (transform.position.x < _leftBound && gameObject.CompareTag("CoinTapPattern"))
        {
            gameObject.SetActive(false);
        }
        else if (transform.position.x < _leftCloudBound && gameObject.CompareTag("CloudPattern"))
        {
            gameObject.SetActive(false);
        }
    }

    public void MoveLeftWithSpeed()
    {
        if (gameObject.CompareTag("Obstacle"))
        {
            transform.Translate(Vector3.back * Time.deltaTime * _speedManager.Speed);
        }
        else if (gameObject.CompareTag("Powerup"))
        {
            MoveObject(_speedManager.Speed);
        }
        else if (gameObject.CompareTag("CoinTapPattern"))
        {
            MoveObject(_speedManager.Speed);
        }
        else if (gameObject.CompareTag("CloudPattern"))
        {
            MoveObject(_speedManager.CloudSpeed);
        }
    }

    void MoveObject(float movementSpeed)
    {
        transform.Translate(Vector3.left * Time.deltaTime * movementSpeed);
    }

}
