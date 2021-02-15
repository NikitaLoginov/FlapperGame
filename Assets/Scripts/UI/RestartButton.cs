using System;
using UnityEngine;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    private Button _restartButton;

    private void Start()
    {
        _restartButton = GetComponent<Button>();
        _restartButton.onClick.AddListener(Restart);
    }

    private void Restart()
    {
        EventBroker.CallRestartGame();
    }
}
