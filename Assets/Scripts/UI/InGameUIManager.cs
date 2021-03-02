using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private TimeManager timeManager;
    
    //UI
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject powerupManager;
    [SerializeField] private GameObject forwardDashButton;
    [SerializeField] private GameObject tapTheScreenText;
    [SerializeField] private HighScore highScoreSO;
    public GameObject ForwardDashButton { get { return forwardDashButton; } }
    [SerializeField] private GameObject slowMotionButton;
    public GameObject SlowMotionButton { get { return slowMotionButton; } }
    [SerializeField] private GameObject invincibilityButton;
    public GameObject InvincibilityButton { get { return invincibilityButton; } }
    
    private int _score;

    private void Awake()
    {
        EventBroker.StartGameHandler += StartGame;
        EventBroker.GameOverHandler += GameOver;
        EventBroker.PauseGameHandler += PauseGame;
        EventBroker.ResumeGameHandler += ResumeGame;
        EventBroker.MainMenuButtonHandler += LoadMainMenu;
        
        EventBroker.ForwardDashButtonHandler += TurnOnDashButtonButton; //On when getting powerup
        EventBroker.SlowMotionButtonHandler += TurnOnSlowMoButton; //On when getting powerup
        EventBroker.InvincibilityButtonHandler += TurnOnInvincibilityButton; //On when getting powerup

        EventBroker.UpdateScoreHandler += UpdateScore;
        EventBroker.CanStartGameHandler += CanStartGame;
    }

    private void CanStartGame()
    {
        //UI
        tapTheScreenText.gameObject.SetActive(true);

        _score = 0;
        scoreText.gameObject.SetActive(true);
        powerupManager.gameObject.SetActive(true);
    }

    private void StartGame()
    {
        UpdateScore(_score);
        tapTheScreenText.gameObject.SetActive(false);
    }

    private void GameOver()
    {
        gameOverScreen.gameObject.SetActive(true);
        PowerupButtonsOn(false);
        CheckIfHighScore();
    }
    
    private void PowerupButtonsOn(bool isOn)
    {
        forwardDashButton.gameObject.SetActive(isOn);
        slowMotionButton.gameObject.SetActive(isOn);
        invincibilityButton.gameObject.SetActive(isOn);
    }
    
    private void CheckIfHighScore()
    {
        if (_score > highScoreSO.highScore)
        {
            highScoreSO.highScore = _score;
        }
    }

    private void TurnOnInvincibilityButton()
    {
        invincibilityButton.gameObject.SetActive(true);
    }

    private void TurnOnSlowMoButton()
    {
        slowMotionButton.gameObject.SetActive(true);
    }

    private void TurnOnDashButtonButton()
    {
        forwardDashButton.gameObject.SetActive(true);
    }

    private void UpdateScore(int scoreToAdd)
    {
        _score += scoreToAdd;
        scoreText.text = "Score: " + _score;
    }

    private void PauseGame()
    {
        timeManager.StopTime(true);
        pauseScreen.gameObject.SetActive(true);
    }

    private void ResumeGame()
    {
        pauseScreen.gameObject.SetActive(false);
        timeManager.StopTime(false);
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void Unsubscribe()
    {
        EventBroker.StartGameHandler -= StartGame;
        EventBroker.GameOverHandler -= GameOver;
        EventBroker.PauseGameHandler -= PauseGame;
        EventBroker.ResumeGameHandler -= ResumeGame;
        EventBroker.MainMenuButtonHandler -= LoadMainMenu;
        
        EventBroker.ForwardDashButtonHandler -= TurnOnDashButtonButton; //On when getting powerup
        EventBroker.SlowMotionButtonHandler -= TurnOnSlowMoButton; //On when getting powerup
        EventBroker.InvincibilityButtonHandler -= TurnOnInvincibilityButton; //On when getting powerup

        EventBroker.UpdateScoreHandler -= UpdateScore;
        EventBroker.CanStartGameHandler -= CanStartGame;
    }
}
