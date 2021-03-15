using UnityEngine;
using UnityEngine.UI;

public class TapTheScreen : MonoBehaviour
{
    private Button _tapTheScreenButton;

    private void Awake()
    {
        _tapTheScreenButton = GetComponent<Button>();

        _tapTheScreenButton.onClick.AddListener(EventBroker.CallStartGame);
    }
}
