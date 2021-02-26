using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [Header("Hats")]
    [SerializeField] private GameObject[] hats;

    [SerializeField] private Hat hatSO;
    [SerializeField] private HighScore highScore;

    private GameObject _hat;
    private int _i;

    private void Awake()
    {
        _hat = hats[_i];
        
        EventBroker.ChangeHatHandler += ChangeHat;
    }
    
    private void ChangeHat(int index)
    {
        _i = index;
        if (_i == hats.Length) _i = 0;

        _hat.gameObject.SetActive(hatSO.isHatActive = false);
        if (highScore.highScore <= 100 && _i == 0) return; // checks if high score is already more than 100 or if hat was already payed for. if so player can have the first hat
        
        _hat = hats[_i];
        _hat.gameObject.SetActive(hatSO.isHatActive = true);

        hatSO.index = _i;
    }

    private void OnDisable()
    {
        EventBroker.ChangeHatHandler -= ChangeHat;
    }
}


