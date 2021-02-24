using UnityEngine;

public class HatManager : MonoBehaviour
{
    [SerializeField] private GameObject[] hats;
    [SerializeField] private Hat hatSO;
    private GameObject _hat;
    private int _index;

    private void Awake()
    {
        _index = hatSO.index;
        _hat = hats[_index];
        _hat.gameObject.SetActive(true);
    }
}
