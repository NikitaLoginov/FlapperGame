using System;
using UnityEngine;

public class Wiggler : MonoBehaviour
{
    private float _yPos;
    private float _yOrig;

    private float _top;
    private float _bot;

    private readonly float _speed = 0.8f;

    private Vector3 _newPosition;
    private Vector3 _positive;
    private Vector3 _negative;
    private Transform _transform;
    private Vector3 _originalPos;

    private void Awake()
    {
        _transform = this.transform;
        _originalPos = _transform.localPosition;
    }

    private void Start()
    {
        _yOrig = _originalPos.y;
        _top = _yOrig + 0.3f;
        _bot = _yOrig - 0.3f;

        _newPosition = new Vector3(0,1,0);
        _positive = new Vector3(0, 1, 0);
        _negative = new Vector3(0, -1, 0);
    }

    private void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        _yPos = _transform.localPosition.y;

        if (_yPos > _top)
        {
            _newPosition = _negative;
        }
        else if (_yPos < _bot)
        {
            _yPos++;
            _newPosition = _positive;
        }
        
        _transform.Translate(_newPosition * (Time.deltaTime * _speed));
    }
}
