using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour
{
    private Button _mainMenuButton;

    private void Awake()
    {
        _mainMenuButton = GetComponent<Button>();
        _mainMenuButton.onClick.AddListener(EventBroker.CallMainMenu);
    }
}
