using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    private float _leftBound = -30.0f;
    private float _leftCloudBound = -100f;
    private GameManager _gameManager;
    private SpeedManager _speedManager;
    private Transform _objTransform;
    private string _gameObjectTag;

    private void Start()
    {
        _speedManager = GameObject.Find("SpeedManager").GetComponent<SpeedManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _objTransform = transform;
        _gameObjectTag = gameObject.tag;
    }


    private void Update()
    {
        if (!_gameManager.IsGameOver)
        { 
            Vector3 objPosition = _objTransform.position;
            
            MoveLeftWithSpeed();
            TurnOffIfOutOfBounds(objPosition);
        }
    }

    private void TurnOffIfOutOfBounds(Vector3 objPosition)
    {
        switch (_gameObjectTag)
        {
            case "Obstacle":
                gameObject.SetActive(!(objPosition.x < _leftBound && gameObject.CompareTag(_gameObjectTag)));
                break;
            case "Powerup":
                gameObject.SetActive(!(objPosition.x < _leftBound && gameObject.CompareTag(_gameObjectTag)));
                break;
            case "CoinTapPattern":
                gameObject.SetActive(!(objPosition.x < _leftBound && gameObject.CompareTag(_gameObjectTag)));
                break;
            case "CloudPattern":
                gameObject.SetActive(!(objPosition.x < _leftCloudBound && gameObject.CompareTag(_gameObjectTag)));
                break;
        }
    }

    public void MoveLeftWithSpeed()
    {
        switch (_gameObjectTag)
        {
            case "Obstacle":
                _objTransform.Translate(Vector3.back * (Time.deltaTime * _speedManager.Speed));
                break;
            case "CloudPattern":
                MoveObject(_speedManager.CloudSpeed);
                break;
            default:
                MoveObject(_speedManager.Speed);
                break; 
        }
    }

    private void MoveObject(float movementSpeed)
    {
        transform.Translate(Vector3.left * (Time.deltaTime * movementSpeed));
    }

}
