using UnityEngine;
using UnityEngine.UI;

public class InvincibilityButton : MonoBehaviour
{
    private Button _invincibilityButton;

    private void Awake()
    {
        _invincibilityButton = GetComponent<Button>();
        _invincibilityButton.onClick.AddListener(EventBroker.CallInvincibility);
    }
}
