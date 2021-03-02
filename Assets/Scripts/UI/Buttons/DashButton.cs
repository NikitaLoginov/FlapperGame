using UnityEngine;
using UnityEngine.UI;

public class DashButton : MonoBehaviour
{
    private Button _forwardDashButton;

    private void Awake()
    {
        _forwardDashButton = GetComponent<Button>();
        _forwardDashButton.onClick.AddListener(EventBroker.CallDash);
    }
}
