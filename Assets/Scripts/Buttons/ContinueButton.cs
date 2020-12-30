using System;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{
    Button _continueButton;
    void Start()
    {
        _continueButton = GetComponent<Button>();

        _continueButton.onClick.AddListener(Continue);
    }

    void Continue()
    {
        EventBroker.PlayerControllerCallContinue();
    }


}
