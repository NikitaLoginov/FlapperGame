using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    //Player
    [SerializeField] private GameObject player;
    private DifficultyManager _difficultyManager;
    private TimeManager _timeManager;
    private SpeedManager _speedManager;

    //UI
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject powerupManager;
    [SerializeField] private GameObject forwardDashButton;
    [SerializeField] private GameObject tapTheScreenText;
    public GameObject ForwardDashButton { get { return forwardDashButton; } }
    [SerializeField] private GameObject slowMotionButton;
    public GameObject SlowMotionButton { get { return slowMotionButton; } }
    [SerializeField] private GameObject invincibilityButton;
    public GameObject InvincibilityButton { get { return invincibilityButton; } }

    //Variables
    private bool _isGameOver;
    public bool IsGameOver { get { return _isGameOver; } }
    private int _score;
    private int _continuesLeft = 3;

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
    }

    public void CanStartGame()
    {
        //UI
        titleScreen.gameObject.SetActive(false);
        tapTheScreenText.gameObject.SetActive(true);

        _score = 0;
        scoreText.gameObject.SetActive(true);

        //Manager set active
        powerupManager.gameObject.SetActive(true);

        player.gameObject.SetActive(true);


        _timeManager.StopTime(true);
    }
    public void StartGame()
    {
        //UI
        tapTheScreenText.gameObject.SetActive(false);

        //Variables
        _isGameOver = false;

        //Events
        EventBroker.GameOverHandler += GameOver;
        EventBroker.RestartGameHandler += RestartGame;
        EventBroker.ForwardDashHandler += TurnOnDashButton; //On when getting powerup
        EventBroker.SlowMotionHandler += TurnOnSlowMoButton; //On when getting powerup
        EventBroker.InvincibilityHandler += TurnOnInvincibilityButton; //On when getting powerup

        EventBroker.UpdateScoreHandler += UpdateScore;
        EventBroker.ContinueGameHandler += ContinueGame;

        EventBroker.DifficultyHandler += _difficultyManager.ModifyDifficulty;
        _difficultyManager.CreateShrinkersList();
        
        //Coroutines
        StartCoroutine(SpawnObstacle());
        StartCoroutine(SpawnPowerups());
        StartCoroutine(SpawnCoins());
        StartCoroutine(SpawnClouds());

        UpdateScore(_score);
        _timeManager.StopTime(false);
    }

    private void TurnOnInvincibilityButton()
    {
        invincibilityButton.gameObject.SetActive(true);
    }

    private void TurnOnSlowMoButton()
    {
        slowMotionButton.gameObject.SetActive(true);
    }

    private void TurnOnDashButton()
    {
        forwardDashButton.gameObject.SetActive(true);
    }

    private void UpdateScore(int scoreToAdd)
    {
        _score += scoreToAdd;
        scoreText.text = "Score: " + _score;
    }

    private void GameOver()
    {
        _isGameOver = true;
        gameOverScreen.gameObject.SetActive(true);

        PowerupButtonsOn(false);
        _timeManager.StopTime(true);
        StopAllCoroutines();
    }
    private void ContinueGame()
    {
        _isGameOver = false;
        gameOverScreen.gameObject.SetActive(false);

        _timeManager.StopTime(false);

        _continuesLeft--;
        
        //Show ad
        if (_continuesLeft < 1)
            gameOverScreen.transform.GetChild(2).gameObject.SetActive(false);

        StartCoroutine(SpawnObstacle());
        StartCoroutine(SpawnPowerups());
        StartCoroutine(SpawnCoins());
        StartCoroutine(SpawnClouds());
    }

    private void PowerupButtonsOn(bool isOn)
    {
        forwardDashButton.gameObject.SetActive(isOn);
        slowMotionButton.gameObject.SetActive(isOn);
        invincibilityButton.gameObject.SetActive(isOn);
    }

    //Restarting scene and unsubscribing from events here
    private void RestartGame()
    {
        EventBroker.UpdateScoreHandler -= UpdateScore;

        EventBroker.DifficultyHandler -= _difficultyManager.ModifyDifficulty;
        EventBroker.GameOverHandler -= GameOver;
        EventBroker.RestartGameHandler -= RestartGame;

        EventBroker.ForwardDashHandler -= TurnOnDashButton; //On when getting powerup
        EventBroker.SlowMotionHandler -= TurnOnSlowMoButton; //On when getting powerup
        EventBroker.InvincibilityHandler -= TurnOnInvincibilityButton;
        EventBroker.ContinueGameHandler -= ContinueGame;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator SpawnObstacle()
    {
        while (!_isGameOver)
        {
            yield return new WaitForSeconds(_spawnRate * _difficultyManager.DifficultyModifier);

            _ySpawnPos = Random.Range(1, 10); //-3, 10
            _spawnPos = new Vector3(20, _ySpawnPos, 0);

            GameObject pooledObstacle = ObjectPooler.SharedInstance.GetPooledObstacle();
            if (pooledObstacle != null)
            {
                pooledObstacle.SetActive(true);
                pooledObstacle.transform.position = _spawnPos;

                for (int i = 0; i < pooledObstacle.transform.childCount; i++)
                {
                    Transform child = pooledObstacle.transform.GetChild(i);
                    //we need one more loop since we trying to reference a grandchild object
                    for (int j = 0; j < child.transform.childCount; j++)
                    {
                        child.transform.GetChild(j).gameObject.SetActive(true);
                    }
                    
                }
            }
        }
    }

    private IEnumerator SpawnPowerups()
    {
        while (!_isGameOver)
        {
            _powerupSpawnRate = GetSpawnRate();
            yield return new WaitForSeconds(_powerupSpawnRate * _difficultyManager.DifficultyModifier);

            _yPowerupSpawnPos = Random.Range(-3, 10); 
            _xPowerupSpawnPos = Random.Range(12, 18);
            _spawnPowerupPos = new Vector3(_xPowerupSpawnPos, _yPowerupSpawnPos, 0);

            GameObject pooledPowerup = ObjectPooler.SharedInstance.GetPooledPowerup();
            if (pooledPowerup != null)
            {
                pooledPowerup.SetActive(true);
                pooledPowerup.transform.position = _spawnPowerupPos;
            }
        }
    }

    private IEnumerator SpawnCoins()
    {
        while (!_isGameOver)
        {
            yield return new WaitForSeconds(_spawnRate * _difficultyManager.DifficultyModifier);

            Vector3 spawnCoinsPosition = new Vector3(12, -2.5f, 0); //reconfigure hard coded values!

            GameObject pooledCoinsPattern =ObjectPooler.SharedInstance.GetPooledCoinPattern();
            if (pooledCoinsPattern != null)
            {
                pooledCoinsPattern.SetActive(true);
                pooledCoinsPattern.transform.position = spawnCoinsPosition;

                //check if needs refactoring
                for (int i = 0; i < pooledCoinsPattern.transform.childCount; i++)
                {
                    pooledCoinsPattern.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
    }


    private IEnumerator SpawnClouds()
    {
        while (!_isGameOver)
        {
            yield return new WaitForSeconds(.5f);

            Vector3 spawnCloudsPos = new Vector3(100, 0, 0);

            GameObject pooledCouldsPatter = ObjectPooler.SharedInstance.GetPooledCloudPattern();
            
            if (pooledCouldsPatter != null)
            {
                pooledCouldsPatter.SetActive(true);
                pooledCouldsPatter.transform.position = spawnCloudsPos;
            }

            yield return new WaitForSeconds(28f); //magic number is a spawn rate of clouds which is constant throughout the game
        }
    }

    //Chooses between 2 spawn rates of powerups
    private float GetSpawnRate()
    {
        float randomValue = Random.value;
        if (randomValue > .50)
            return 15f;
        else
            return 20f;
    }

}
