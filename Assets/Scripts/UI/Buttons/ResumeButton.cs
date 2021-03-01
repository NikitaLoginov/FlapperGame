using System;
using UnityEngine;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour
{
    private Button _resumeButton;

    private void Awake()
    {
        _resumeButton = GetComponent<Button>();
        _resumeButton.onClick.AddListener(EventBroker.CallResumeGame);
    }
}
