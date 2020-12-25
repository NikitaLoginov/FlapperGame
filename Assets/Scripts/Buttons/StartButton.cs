using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    Button startButton;
    GameManager gameManager;
    void Start()
    {
        startButton = GetComponent<Button>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        startButton.onClick.AddListener(CanStartGame);
    }

    void CanStartGame()
    {
        gameManager.CanStartGame();
    }
}
