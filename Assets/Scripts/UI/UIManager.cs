using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _titleScreen;
    [SerializeField] private GameObject _skinShopScreen;
    [SerializeField] private TextMeshProUGUI _hsText;
    [SerializeField] private HighScore _hsSO;
    [SerializeField] private GameObject _menuChicken;
    private int _highScore;

    private void Awake()
    {
        _highScore = _hsSO.highScore;
        _hsText.text = "High Score: " + _highScore;
        EventBroker.StartButtonHandler += OnStartButtonPressed;
        EventBroker.ShopButtonHandler += OnShopButtonPressed;
        EventBroker.BackButtonHandler += OnBackButtonPressed;
    }
    private void OnShopButtonPressed()
    {
        _titleScreen.gameObject.SetActive(false);
        _skinShopScreen.gameObject.SetActive(true);
        _menuChicken.gameObject.SetActive(true);
    }

    private void OnBackButtonPressed()
    {
        _menuChicken.gameObject.SetActive(false);
        _skinShopScreen.gameObject.SetActive(false);
        _titleScreen.gameObject.SetActive(true);
    }

    private void OnStartButtonPressed()
    {
        _titleScreen.gameObject.SetActive(false);
        Unsubscribe();
        SceneManager.LoadScene("FlappyChicken");
    }

    private void Unsubscribe()
    {
        EventBroker.ShopButtonHandler -= OnShopButtonPressed;
        EventBroker.BackButtonHandler -= OnBackButtonPressed;
        EventBroker.StartButtonHandler -= OnStartButtonPressed;
    }
}
