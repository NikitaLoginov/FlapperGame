using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private Button startButton;

    private void Start()
    {
        startButton = GetComponent<Button>();

        startButton.onClick.AddListener(EventBroker.CallOnStartButtonPressed);
    }
}
