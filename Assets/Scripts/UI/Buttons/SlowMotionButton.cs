using UnityEngine;
using UnityEngine.UI;

public class SlowMotionButton : MonoBehaviour
{
    private Button _slowMotionButton;

    private void Awake()
    {
        _slowMotionButton = GetComponent<Button>();
        _slowMotionButton.onClick.AddListener(EventBroker.CallSlowMotion);
    }
}
