using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    float _leftBound = -30.0f;
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
            MoveLeftWithSpeed(_speedManager.Speed);
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
    }

    public void MoveLeftWithSpeed(float movementSpeed)
    {
        if (gameObject.CompareTag("Obstacle"))
        {
            transform.Translate(Vector3.back * Time.deltaTime * movementSpeed);
        }
        else if (gameObject.CompareTag("Powerup"))
        {
            transform.Translate(Vector3.left * Time.deltaTime * movementSpeed);
        }
    }

}
