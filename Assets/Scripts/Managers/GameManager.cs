using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    //Player
    [SerializeField] GameObject player;
    PlayerController playerController;
    DifficultyManager difficultyManager;
    TimeManager timeManager;
    //ContinueButton continueButtonScript;

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
    bool isGameOver;
    public bool IsGameOver { get { return isGameOver; } }
    int score;

    //Spawn
    Vector3 spawnPos;
    int ySpawnPos;
    float spawnRate = 2f;

    Vector3 spawnPowerupPos;
    int xPowerupSpawnPos;
    int yPowerupSpawnPos;
    float powerupSpawnRate;

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
        playerController = FindObjectOfType<PlayerController>();
        difficultyManager = FindObjectOfType<DifficultyManager>();
        timeManager = FindObjectOfType<TimeManager>();
        //continueButtonScript = FindObjectOfType<ContinueButton>();
    }
    public void StartGame()
    {
        //UI
        tapTheScreenText.gameObject.SetActive(false);

        //Variables
        isGameOver = false;

        //Events
        playerController.GameOverHandler += GameOver;
        playerController.ForwardDashHandler += TurnOnDashButton; //On when getting powerup
        playerController.SlowMoHandler += TurnOnSlowMoButton; //On when getting powerup
        playerController.InvincibilityHandler += TurnOnInvincibilityButton; //On when getting powerup
        playerController.UpdateScoreHandler += UpdateScore;
        playerController.ContinueHandler += ContinueGame;

        playerController.ModifyDifficultyHandler += difficultyManager.ModifyDifficulty;
        difficultyManager.CreateShrinkersList();
        
        //Coroutines
        StartCoroutine(SpawnObstacle());
        StartCoroutine(SpawnPowerup());
        UpdateScore(score);
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
        isGameOver = true;
        gameOverScreen.gameObject.SetActive(true);

        PowerupButtonsOn(false);
        //Time.timeScale = 0f; // put game on pause
        timeManager.StopTime(true);
    }
    private void ContinueGame()
    {
        isGameOver = false;
        gameOverScreen.gameObject.SetActive(false);

        //Time.timeScale = 1f;
        timeManager.StopTime(false);
    }

    void PowerupButtonsOn(bool isOn)
    {
        forwardDashButton.gameObject.SetActive(isOn);
        slowMotionButton.gameObject.SetActive(isOn);
        invincibilityButton.gameObject.SetActive(isOn);
    }
    public void RestartGame()
    {
        playerController.UpdateScoreHandler -= UpdateScore;
        playerController.ModifyDifficultyHandler -= difficultyManager.ModifyDifficulty;
        playerController.GameOverHandler -= GameOver;

        playerController.ForwardDashHandler -= TurnOnDashButton; //On when getting powerup
        playerController.SlowMoHandler -= TurnOnSlowMoButton; //On when getting powerup
        playerController.InvincibilityHandler -= TurnOnInvincibilityButton;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator SpawnObstacle()
    {
        while (!isGameOver)
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
        while (!isGameOver)
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
