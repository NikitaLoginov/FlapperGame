using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    //difficulty
    private float _difficultyRaise = 0.02f;
    private float _difficultyModifier = 1f;
    private float _maxDifficultyModifier = 0.7f;
    private float _timeToShrink = .25f;
    private int _scoreToRaiseDifficulty = 0;
    private float _lower = -28;
    private float _upper = 28;

    public float DifficultyModifier { get { return _difficultyModifier; } }


    private List<GameObject> _obstacleShrinkers;
    private readonly List<ObstacleShrinking> _shrinkers = new List<ObstacleShrinking>();
    private SpeedManager _speedManager;

    private void Start()
    {
        _speedManager = GameObject.Find("SpeedManager").GetComponent<SpeedManager>();
    }

    public void CreateShrinkersList()
    {
        _obstacleShrinkers = ObjectPooler.SharedInstance.pooledObstacles;
        
        foreach (var obstacle in _obstacleShrinkers)
        {
            _shrinkers.Add(obstacle.GetComponent<ObstacleShrinking>());
        }
    }

    public void ModifyDifficulty()
    {
        _scoreToRaiseDifficulty++;
        if (_scoreToRaiseDifficulty > 9 && _difficultyModifier > _maxDifficultyModifier) //1>0.7
        {
            _lower = _lower + 1f;
            _upper = _upper - 1f;
            _difficultyModifier -= _difficultyRaise; // 1-= 0.02
            
            //by dividing speed by difficulty coefficient we make so that speed rises in sync with difficulty
            _speedManager.Speed /= _difficultyModifier;
            
            foreach (var shrinker in _shrinkers)
            {
                shrinker.CanShrink = true;
                StartCoroutine(StopShrinking());
            }
            _scoreToRaiseDifficulty = 0;
        }
    }

    private IEnumerator StopShrinking()
    {
        yield return new WaitForSeconds(_timeToShrink);
        foreach (var shrinker in _shrinkers)
        {
            shrinker.CanShrink = false;
        }
    }

}
