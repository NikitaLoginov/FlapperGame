using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    private Button _backButton;
    void Start()
    {
        _backButton = GetComponent<Button>();
        _backButton.onClick.AddListener(EventBroker.CallOnBackButtonPressed);
    }
}
