using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SpeedManager : MonoBehaviour
{
    private float _speed;
    [SerializeField] float startSpeed = 5f; //10 is starting point
    private float _cloudSpeed = 5f;
    private float _skyboxSpeed = -2f;
    private float _normalSpeed;
    public float Speed { get { return _speed; } set { _speed = value; } }
    public float CloudSpeed { get { return _cloudSpeed; } }
    public float SkyboxSpeed { get { return _skyboxSpeed; } }


    private void Awake()
    {
        _speed = startSpeed;
        _normalSpeed = startSpeed;
    }
    public void NormaliseSpeed()
    {
        Speed = _normalSpeed;
    }
}
