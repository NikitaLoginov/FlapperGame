using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private TimeManager timeManager;

    private void Awake()
    {
        EventBroker.PauseGameHandler += PauseGame;
        EventBroker.ResumeGameHandler += ResumeGame;
        EventBroker.MainMenuButtonHandler += LoadMainMenu;
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
        EventBroker.PauseGameHandler -= PauseGame;
        EventBroker.ResumeGameHandler -= ResumeGame;
        EventBroker.MainMenuButtonHandler -= LoadMainMenu;
    }
}
