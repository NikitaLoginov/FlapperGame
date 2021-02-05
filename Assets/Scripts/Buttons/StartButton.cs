using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private Button startButton;
    private GameManager gameManager;

    private void Start()
    {
        startButton = GetComponent<Button>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        startButton.onClick.AddListener(gameManager.CanStartGame);
    }
}
