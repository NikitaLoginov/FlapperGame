using System;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{
    Button _continueButton;
    //ContinueManager _continueManager;

    //public event Action ContinueHandler; 
    void Start()
    {
        _continueButton = GetComponent<Button>();
        //_continueManager = GameObject.Find("ContinueManager").GetComponent<ContinueManager>();

        _continueButton.onClick.AddListener(Continue);
    }

    void Continue()
    {
        ContinueManager.CallContinue();
    }
}
