using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    private float _leftBound = -30.0f;
    private float _leftCloudBound = -100f;
    private GameManager _gameManager;
    private SpeedManager _speedManager;
    private Transform objTransform;

    private void Start()
    {
        _speedManager = GameObject.Find("SpeedManager").GetComponent<SpeedManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        objTransform = transform;
    }


    private void Update()
    {
        if (!_gameManager.IsGameOver)
        { 
            Vector3 objPosition = objTransform.position;
            
            MoveLeftWithSpeed();
            TurnOffIfOutOfBounds(objPosition);
        }
    }

    private void TurnOffIfOutOfBounds(Vector3 objPosition)
    {
        if (IsOutOfBounds("Obstacle", objPosition) || IsOutOfBounds("Powerup",objPosition) || IsOutOfBounds("CoinTapPattern",objPosition))
        {
            gameObject.SetActive(false);
        }
        else if (objPosition.x < _leftCloudBound && gameObject.CompareTag("CloudPattern"))
        {
            gameObject.SetActive(false);
        }
    }

    bool IsOutOfBounds(string tag, Vector3 objPosition)
    {
        bool outOfBouds = objPosition.x < _leftBound;
        return outOfBouds && gameObject.CompareTag(tag);
    }

    public void MoveLeftWithSpeed()
    {
        if (gameObject.CompareTag("Obstacle"))
        {
            transform.Translate(Vector3.back * (Time.deltaTime * _speedManager.Speed));
        }
        else if (gameObject.CompareTag("Powerup") || gameObject.CompareTag("CoinTapPattern"))
        {
            MoveObject(_speedManager.Speed);
        }
        else if (gameObject.CompareTag("CloudPattern"))
        {
            MoveObject(_speedManager.CloudSpeed);
        }
    }

    private void MoveObject(float movementSpeed)
    {
        transform.Translate(Vector3.left * (Time.deltaTime * movementSpeed));
    }

}
