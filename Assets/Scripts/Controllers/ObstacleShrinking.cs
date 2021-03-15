using System.Collections;
using UnityEngine;

public class ObstacleShrinking : MonoBehaviour
{
    private GameObject _lowerObstacle;
    private GameObject _upperObstacle;

    private float _minGap = 4.0f;
    private float _narrowingSpeed = 4f; // adjust speed to make closing the gap look good

    private Vector3 _lowerObstaclePos;
    private Vector3 _upperObstaclePos;

    private bool _canShrink;
    public bool CanShrink { set => _canShrink = value; }

    private void Awake()
    {
        _upperObstacle = transform.GetChild(1).gameObject;
        _lowerObstacle = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (CanNarrowGap() && _canShrink)
        {
            NarrowTheGap();
        } 
    }

    private void NarrowTheGap()
    {
        float step = _narrowingSpeed * Time.deltaTime;
        
        _lowerObstacle.transform.position = Vector3.MoveTowards(_lowerObstaclePos,_upperObstaclePos,step);
        
        _upperObstacle.transform.position = Vector3.MoveTowards(_upperObstaclePos, _lowerObstaclePos, step);
    }

    //works pretty well
    private bool CanNarrowGap()
    {
        //important for position to be defined dynamically for each objects since they are coming and going from the scene
        //and otherwise it could mess up links to them.
        _lowerObstaclePos = _lowerObstacle.transform.position;
        _upperObstaclePos = _upperObstacle.transform.position;
        
        float gapValue = (_lowerObstaclePos - _upperObstaclePos).magnitude;
        
        if (gapValue < _minGap)
            return false;
        return true;
    }
}
