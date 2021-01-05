using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    //Player
    [SerializeField] GameObject player;
    DifficultyManager difficultyManager;
    TimeManager timeManager;

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
    int score;
    int continuesLeft = 3;

    //Spawn
    Vector3 spawnPos;
    int ySpawnPos;
    float spawnRate = 2f;

    Vector3 spawnPowerupPos;
    int xPowerupSpawnPos;
    int yPowerupSpawnPos;
    float powerupSpawnRate;
    float coinsSpawnRate = 2f;

    public void CanStartGame()
    {
        //UI
        titleScreen.gameObject.SetActive(false);
        tapTheScreenText.gameObject.SetActive(true);

        score = 0;
        scoreText.gameObject.SetActive(true);

        //Manager set active
        powerupManager.gameObject.SetActive(true);

        player.gameObject.SetActive(true);

        difficultyManager = FindObjectOfType<DifficultyManager>();
        timeManager = FindObjectOfType<TimeManager>();
        timeManager.StopTime(true);
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

        EventBroker.DifficultyHandler += difficultyManager.ModifyDifficulty;
        difficultyManager.CreateShrinkersList();
        
        //Coroutines
        StartCoroutine(SpawnObstacle());
        StartCoroutine(SpawnPowerup());
        StartCoroutine(SpawnCoins());

        UpdateScore(score);
        timeManager.StopTime(false);
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
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    void GameOver()
    {
        _isGameOver = true;
        gameOverScreen.gameObject.SetActive(true);

        PowerupButtonsOn(false);
        timeManager.StopTime(true);
        StopAllCoroutines();
    }
    private void ContinueGame()
    {
        _isGameOver = false;
        gameOverScreen.gameObject.SetActive(false);

        timeManager.StopTime(false);

        continuesLeft--;
        //Show ad
        if (continuesLeft < 1)
            gameOverScreen.transform.GetChild(2).gameObject.SetActive(false);

        StartCoroutine(SpawnObstacle());
        StartCoroutine(SpawnPowerup());
        StartCoroutine(SpawnCoins());
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

        EventBroker.DifficultyHandler -= difficultyManager.ModifyDifficulty;
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
            yield return new WaitForSeconds(spawnRate * difficultyManager.DifficultyModifier);

            ySpawnPos = Random.Range(-3, 10);
            spawnPos = new Vector3(15, ySpawnPos, 0);

            GameObject pooledObstacle = ObjectPooler.SharedInstance.GetPooledObstacle();
            if (pooledObstacle != null)
            {
                pooledObstacle.SetActive(true);
                pooledObstacle.transform.position = spawnPos;
            }
        }
    }

    IEnumerator SpawnPowerup()
    {
        while (!_isGameOver)
        {
            powerupSpawnRate = GetSpawnRate();
            yield return new WaitForSeconds(powerupSpawnRate * difficultyManager.DifficultyModifier);

            yPowerupSpawnPos = Random.Range(-3, 10); 
            xPowerupSpawnPos = Random.Range(10, 14);
            spawnPowerupPos = new Vector3(xPowerupSpawnPos, yPowerupSpawnPos, 0);

            GameObject pooledPowerup = ObjectPooler.SharedInstance.GetPooledPowerup();
            if (pooledPowerup != null)
            {
                pooledPowerup.SetActive(true);
                pooledPowerup.transform.position = spawnPowerupPos;
            }
        }
    }

    IEnumerator SpawnCoins()
    {
        while (!_isGameOver)
        {
            yield return new WaitForSeconds(coinsSpawnRate * difficultyManager.DifficultyModifier);


            Vector3 spawnCoinsPosition = new Vector3(13, 0, 3); //reconfigure hard coded values!

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
