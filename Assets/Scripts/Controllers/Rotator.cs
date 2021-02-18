using UnityEngine;

public class Rotator : MonoBehaviour
{
    private float _rotationSpeed = 50f;
    private Transform _transform;

    private void Awake()
    {
        _transform = this.transform;
    }

    private void Update()
    {
        _transform.Rotate(new Vector3(0, 1,0) * (Time.deltaTime * _rotationSpeed)); 
    }
}
