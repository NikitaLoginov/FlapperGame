using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TapTheScreen : MonoBehaviour
{
    Button tapTheScreenButton;
    GameManager gameManager;
    void Start()
    {
        tapTheScreenButton = GetComponent<Button>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        tapTheScreenButton.onClick.AddListener(StartGame);
    }

    void StartGame()
    {
        gameManager.StartGame();
    }
}
