using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private float _slowdownFactor = 0.05f;
    private float _slowMotionLength = 3f;
    private bool _isTimeStoped;


    private void Update()
    {
        if (!_isTimeStoped)
        { 
            NormilizeTime();
        }
    }

    private void NormilizeTime()
    {
        Time.timeScale += (1f / _slowMotionLength) * Time.unscaledDeltaTime; //unscaledDeltaTime is like deltaTime, but calculated unrelated to timeScale
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f); // clamp time between 0 and normal time (1)
    }

    public void DoSlowMotion()
    {
        Time.timeScale = _slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.2f;
    }

    public void StopTime(bool canStop)
    {
        if (canStop)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
        
        _isTimeStoped = canStop;

    }
}
