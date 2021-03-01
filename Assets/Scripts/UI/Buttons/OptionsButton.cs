using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionsButton : MonoBehaviour
{
    private Button _optionsButton;

    private void Awake()
    {
        _optionsButton = GetComponent<Button>();
        _optionsButton.onClick.AddListener(EventBroker.CallOptions);
    }
}
