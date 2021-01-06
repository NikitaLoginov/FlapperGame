using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    float _speed;
    float _normalSpeed = 10f;
    public float NormalSpeed { get { return _normalSpeed; }  }
    public float Speed { get { return _speed; } set { _speed = value; } }

    private void Awake()
    {
        _speed = _normalSpeed;
    }
}
