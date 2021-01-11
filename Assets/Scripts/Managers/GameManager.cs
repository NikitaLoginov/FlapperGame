using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    //Player
    [SerializeField] GameObject player;
    DifficultyManager _difficultyManager;
    TimeManager _timeManager;
    SpeedManager _speedManager;

    //UI
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject titleScreen;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject powerupManager;
    [SerializeField] GameObject forwardDashButton;
    [SerializeField] GameObject tapTheScreenText;
    public GameObject ForwardDashButton { get { return forwardDashButton; } }
    [SerializeField] GameObject slowMotionButton;
    public GameObject SlowMotionButton { get { return slowMotionButton; } }
    [SerializeField] GameObject invincibilityButton;
    public GameObject InvincibilityButton { get { return invincibilityButton; } }

    //Variables
    bool _isGameOver;
    public bool IsGameOver { get { return _isGameOver; } }
    int _score;
    int _continuesLeft = 3;

    //Spawn
    Vector3 _spawnPos;
    int _ySpawnPos;
    float _spawnRate = 5f;

    Vector3 _spawnPowerupPos;
    int _xPowerupSpawnPos;
    int _yPowerupSpawnPos;
    float _powerupSpawnRate;
    float _coinsSpawnRate = 5f;

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

        _difficultyManager = FindObjectOfType<DifficultyManager>();
        _timeManager = FindObjectOfType<TimeManager>();

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
        StartCoroutine(SpawnPowerup());
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

    void UpdateScore(int scoreToAdd)
    {
        _score += scoreToAdd;
        scoreText.text = "Score: " + _score;
    }

    void GameOver()
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
        StartCoroutine(SpawnPowerup());
        StartCoroutine(SpawnCoins());
        StartCoroutine(SpawnClouds());
    }

    void PowerupButtonsOn(bool isOn)
    {
        forwardDashButton.gameObject.SetActive(isOn);
        slowMotionButton.gameObject.SetActive(isOn);
        invincibilityButton.gameObject.SetActive(isOn);
    }

    //Restarting scene and unsubscribing from events here
    void RestartGame()
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

    IEnumerator SpawnObstacle()
    {
        while (!_isGameOver)
        {
            yield return new WaitForSeconds(_spawnRate * _difficultyManager.DifficultyModifier);

            _ySpawnPos = Random.Range(-3, 10);
            _spawnPos = new Vector3(20, _ySpawnPos, 0);

            GameObject pooledObstacle = ObjectPooler.SharedInstance.GetPooledObstacle();
            if (pooledObstacle != null)
            {
                pooledObstacle.SetActive(true);
                pooledObstacle.transform.position = _spawnPos;
            }
        }
    }

    IEnumerator SpawnPowerup()
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

    IEnumerator SpawnCoins()
    {
        while (!_isGameOver)
        {
            yield return new WaitForSeconds(_coinsSpawnRate * _difficultyManager.DifficultyModifier);


            Vector3 spawnCoinsPosition = new Vector3(12, 0, 0); //reconfigure hard coded values!

            GameObject pooledCoinsPattern = ObjectPooler.SharedInstance.GetPooledCoinPattern();
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

    IEnumerator SpawnClouds()
    {
        while (!_isGameOver)
        {
            yield return new WaitForSeconds(1f);

            Vector3 spawnCloudsPos = new Vector3(100, 0, 0);

            GameObject pooledCouldsPatter = ObjectPooler.SharedInstance.GetPooledCloudPattern();
            
            if (pooledCouldsPatter != null)
            {
                pooledCouldsPatter.SetActive(true);
                pooledCouldsPatter.transform.position = spawnCloudsPos;
            }

            yield return new WaitForSeconds(28f);
        }
    }

    //Chooses between 2 spawn rates of powerups
    private float GetSpawnRate()
    {
        float randomValue = Random.value;
        if (randomValue > .50)
            return 6f;
        else
            return 8f;
    }

}
