using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _titleScreen;
    [SerializeField] private GameObject _skinShopScreen;
    
    [Header("Hats")]
    [SerializeField] private GameObject[] hats;

    [SerializeField] private GameObject _menuChicken;

    private GameObject _hat;
    private int i;

    private void Awake()
    {
        EventBroker.ShopButtonHandler += OnShopButtonPressed;
        EventBroker.BackButtonHandler += OnBackButtonPressed;
        EventBroker.StartButtonHandler += OnStartButtonPressed;
        EventBroker.ChangeHatHandler += ChangeHat;
    }

    private void Start()
    {
        _hat = hats[i];
    }

    void OnShopButtonPressed()
    {
        _titleScreen.gameObject.SetActive(false);
        _skinShopScreen.gameObject.SetActive(true);
        _menuChicken.gameObject.SetActive(true);
    }

    void OnBackButtonPressed()
    {
        _menuChicken.gameObject.SetActive(false);
        _skinShopScreen.gameObject.SetActive(false);
        _titleScreen.gameObject.SetActive(true);
    }

    void OnStartButtonPressed()
    {
        _titleScreen.gameObject.SetActive(false);
        Unsubscribe();
        SceneManager.LoadScene("FlappyChicken");
    }

    void ChangeHat(int index)
    {
        if (i == hats.Length) i = 0;
        
        _hat.gameObject.SetActive(false);
        _hat = hats[i++];
        _hat.gameObject.SetActive(true);
    }

    void Unsubscribe()
    {
        EventBroker.ShopButtonHandler -= OnShopButtonPressed;
        EventBroker.BackButtonHandler -= OnBackButtonPressed;
        EventBroker.StartButtonHandler -= OnStartButtonPressed;
        EventBroker.ChangeHatHandler -= ChangeHat;
    }
}
