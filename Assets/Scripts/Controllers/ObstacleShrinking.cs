using UnityEngine;

public class ObstacleShrinking : MonoBehaviour
{
    GameObject _lowerObstacle;
    GameObject _upperObstacle;

    public Transform LowerObstacle { get { return _lowerObstacle.transform; } }
    public Transform UpperObstacle { get { return _upperObstacle.transform; } }

    float _minGap = 30f;
    float _gapModifier = 1f;
    float _narrowingSpeed = 4f; // adjust speed to make closing the gap look good

    bool _canShrink;
    public bool CanShrink { set { _canShrink = value; } }

    void Awake()
    {
        _upperObstacle = transform.GetChild(0).gameObject;
        _lowerObstacle = transform.GetChild(1).gameObject;
    }

    private void Update()
    {
        if (CanNarrowGap() && _canShrink)
            NarrowTheGap();
    }


    void NarrowTheGap()
    {
        _lowerObstacle.transform.position = Vector3.MoveTowards(_lowerObstacle.transform.position, new Vector3(
                _lowerObstacle.transform.position.x, (_lowerObstacle.transform.position.y + _gapModifier), _lowerObstacle.transform.position.z),(_narrowingSpeed * Time.deltaTime));

        _upperObstacle.transform.position = Vector3.MoveTowards(_upperObstacle.transform.position, new Vector3(
                _upperObstacle.transform.position.x, (_upperObstacle.transform.position.y - _gapModifier), _upperObstacle.transform.position.z),(_narrowingSpeed * Time.deltaTime));
    }

    bool CanNarrowGap()
    {
        float lowerBound = _lowerObstacle.transform.position.y;
        float upperBound = _upperObstacle.transform.position.y;
        float gapValue = Mathf.Abs(lowerBound) + Mathf.Abs(upperBound);

        if (gapValue < _minGap)
            return false;
        else
            return true;
    }
}
