using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private Button _startButton;

    private void Awake()
    {
        _startButton = GetComponent<Button>();

        _startButton.onClick.AddListener(EventBroker.CallOnStartButtonPressed);
    }
}
