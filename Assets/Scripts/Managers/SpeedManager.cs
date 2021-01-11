using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    float _speed;
    float _normalSpeed = 5f; //10 is starting point
    float _cloudSpeed = 5f;
    public float NormalSpeed { get { return _normalSpeed; }  }
    public float Speed { get { return _speed; } set { _speed = value; } }
    public float CloudSpeed { get { return _cloudSpeed; } }


    private void Awake()
    {
        _speed = _normalSpeed;
    }

    //should run when difficulty is rising
    void UpdateSpeed()
    { 
        
    }
}
