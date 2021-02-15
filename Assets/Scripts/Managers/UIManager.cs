using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _titleScreen;
    [SerializeField] private GameObject _skinShopScreen;

    private void Awake()
    {
        EventBroker.ShopButtonHandler += OnShopButtonPressed;
        EventBroker.BackButtonHandler += OnBackButtonPressed;
        EventBroker.StartButtonHadler += OnStartButtonPressed;
    }

    void OnShopButtonPressed()
    {
        _titleScreen.gameObject.SetActive(false);
        _skinShopScreen.gameObject.SetActive(true);
    }

    void OnBackButtonPressed()
    {
        _skinShopScreen.gameObject.SetActive(false);
        _titleScreen.gameObject.SetActive(true);
    }

    void OnStartButtonPressed()
    {
        _titleScreen.gameObject.SetActive(false);
        Unsubscribe();
        SceneManager.LoadScene("FlappyChicken");
    }

    void Unsubscribe()
    {
        EventBroker.ShopButtonHandler -= OnShopButtonPressed;
        EventBroker.BackButtonHandler -= OnBackButtonPressed;
        EventBroker.StartButtonHadler -= OnStartButtonPressed;
    }
}
