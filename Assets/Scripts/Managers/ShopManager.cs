using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [Header("Hats")]
    [SerializeField] private GameObject[] hats;

    [SerializeField] private Hat hatSO;

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
        
        _hat.gameObject.SetActive(false);
        _hat = hats[_i++];
        _hat.gameObject.SetActive(true);
        
        hatSO.index = _i - 1;// because we can't have 0 here for some reason
    }

    private void OnDisable()
    {
        EventBroker.ChangeHatHandler -= ChangeHat;
    }
}


