using System;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{
    private Button _continueButton;

    private void Start()
    {
        _continueButton = GetComponent<Button>();

        _continueButton.onClick.AddListener(Continue);
    }

    private void Continue()
    {
        EventBroker.PlayerControllerCallContinue();
    }


}
