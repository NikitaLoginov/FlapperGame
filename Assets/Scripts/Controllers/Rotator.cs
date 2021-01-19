using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    private float _rotationSpeed = 50f;

    private void Update()
    {
        transform.Rotate(new Vector3(0, 1,0) * (Time.deltaTime * _rotationSpeed)); 
    }
}
