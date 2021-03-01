using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    private Button _shopButton;

    private void Awake()
    {
        _shopButton = GetComponent<Button>();
        _shopButton.onClick.AddListener(EventBroker.CallOnShopButtonPressed);
    }
    
}
