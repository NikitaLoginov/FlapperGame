using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    private Button _pauseButton;

    private void Awake()
    {
        _pauseButton = GetComponent<Button>();
        _pauseButton.onClick.AddListener(EventBroker.CallPauseGame);
    }
}
