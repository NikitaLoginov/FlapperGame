using UnityEngine;
using UnityEngine.UI;

public class TapTheScreen : MonoBehaviour
{
    private Button tapTheScreenButton;

    private void Start()
    {
        tapTheScreenButton = GetComponent<Button>();

        tapTheScreenButton.onClick.AddListener(EventBroker.CallStartGame);
    }
}
