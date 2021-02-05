using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotator : MonoBehaviour
{
    private float _skyboxSpeed;
    private GameManager _gameManager;
    private SpeedManager _speedManager;
    private Material _skybox;

    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _speedManager = GameObject.Find("SpeedManager").GetComponent<SpeedManager>();
        
        _skyboxSpeed = _speedManager.SkyboxSpeed;
        _skybox = RenderSettings.skybox;
    }

    private void Update()
    {
        if (!_gameManager.IsGameOver)
        {
            _skybox.SetFloat("_Rotation",Time.time * _skyboxSpeed);
        }

    }
}
