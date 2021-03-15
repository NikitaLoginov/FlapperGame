using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    //Player
    [SerializeField] private GameObject player;
    private DifficultyManager _difficultyManager;
    private TimeManager _timeManager;
    private SpeedManager _speedManager;

    //Variables
    private bool _isGameOver;
    public bool IsGameOver { get { return _isGameOver; } }

    //Spawn
    private Vector3 _spawnPos;
    private float _ySpawnPos;
    private float _spawnRate = 5f;

    private Vector3 _spawnPowerupPos;
    private int _xPowerupSpawnPos;
    private int _yPowerupSpawnPos;

    private float _powerupSpawnRate;
    
    private void Awake()
    {
        _isGameOver = true;
        
        _difficultyManager = FindObjectOfType<DifficultyManager>();
        _timeManager = FindObjectOfType<TimeManager>();
        EventBroker.CanStartGameHandler += CanStartGame;
        EventBroker.StartGameHandler += StartGame;
    }

    private void Start()
    {
        EventBroker.CallCanStartGame();
    }

    private void CanStartGame()
    {
        player.gameObject.SetActive(true);

        _timeManager.StopTime(true);
    }

    private void StartGame()
    {
        //Variables
        _isGameOver = false;

        //Events
        EventBroker.GameOverHandler += GameOver;
        EventBroker.RestartGameHandler += RestartGame;
        _timeManager.StopTime(false);
        
    }
    private void Update()
    {
        //for debuging
        if (Input.GetKeyDown(KeyCode.H))
        {
            player.gameObject.SetActive(false);
        }
    }


    private void GameOver()
    {
        _isGameOver = true;
        _timeManager.StopTime(true);
    }

    //Restarting scene 
    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void Unsubscribe()
    {
        EventBroker.GameOverHandler -= GameOver;
        EventBroker.RestartGameHandler -= RestartGame;

        EventBroker.CanStartGameHandler -= CanStartGame;
        EventBroker.StartGameHandler -= StartGame;
    }
}
