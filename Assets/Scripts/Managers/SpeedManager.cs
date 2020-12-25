using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    float speed;
    float normalSpeed = 10f;
    public float NormalSpeed { get { return normalSpeed; }  }
    public float Speed { get { return speed; } set { speed = value; } }

    private void Awake()
    {
        speed = normalSpeed;
    }
}
