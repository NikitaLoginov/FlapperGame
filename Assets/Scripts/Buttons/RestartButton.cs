using System;
using UnityEngine;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    Button _restartButton;

    private void Start()
    {
        _restartButton = GetComponent<Button>();
        _restartButton.onClick.AddListener(Restart);
    }

    void Restart()
    {
        EventBroker.CallRestartGame();
    }
}
