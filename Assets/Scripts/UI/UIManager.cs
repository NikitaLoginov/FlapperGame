using System;
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
    [SerializeField] private GameObject optionsScreen;
    private int _highScore;

    private void Awake()
    {
        _highScore = _hsSO.highScore;
        _hsText.text = "High Score: " + _highScore;
        EventBroker.StartButtonHandler += OnStartButtonPressed;
        EventBroker.ShopButtonHandler += OnShopButtonPressed;
        EventBroker.BackButtonHandler += OnBackButtonPressed;
        EventBroker.OptionsButtonHandler += OnOptionsButtonPressed;
    }
    private void OnShopButtonPressed()
    {
        _titleScreen.gameObject.SetActive(false);
        _skinShopScreen.gameObject.SetActive(true);
        _menuChicken.gameObject.SetActive(true);
    }

    private void OnBackButtonPressed()
    {
        if (_menuChicken.gameObject.activeInHierarchy)
        {
            _menuChicken.gameObject.SetActive(false);
            _skinShopScreen.gameObject.SetActive(false);
        }
        else if (optionsScreen.gameObject.activeInHierarchy)
        {
            optionsScreen.gameObject.SetActive(false);
        }
        _titleScreen.gameObject.SetActive(true);
    }

    private void OnStartButtonPressed()
    {
        _titleScreen.gameObject.SetActive(false);
        //Unsubscribe();
        SceneManager.LoadScene("FlappyChicken");
    }

    private void OnOptionsButtonPressed()
    {
        _titleScreen.gameObject.SetActive(false);
        optionsScreen.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void Unsubscribe()
    {
        EventBroker.ShopButtonHandler -= OnShopButtonPressed;
        EventBroker.BackButtonHandler -= OnBackButtonPressed;
        EventBroker.StartButtonHandler -= OnStartButtonPressed;
        EventBroker.OptionsButtonHandler -= OnOptionsButtonPressed;
    }
}
