using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TapTheScreen : MonoBehaviour
{
    private Button tapTheScreenButton;
    private GameManager gameManager;

    private void Start()
    {
        tapTheScreenButton = GetComponent<Button>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        tapTheScreenButton.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        gameManager.StartGame();
    }
}
