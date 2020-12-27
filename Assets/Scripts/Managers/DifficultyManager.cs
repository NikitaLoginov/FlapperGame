using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{

    //difficulty
    float _difficultyRaise = 0.02f;
    float _difficultyModifier = 1f;
    float _maxDifficultyModifier = 0.7f;
    float _timeToShrink = .25f;
    int _scoreToRaiseDifficulty = 0;
    float _lower = -28;
    float _upper = 28;

    public float DifficultyModifier { get { return _difficultyModifier; } }


    List<GameObject> _obstacleShrinkers;
    List<ObstacleShrinking> _shrinkers = new List<ObstacleShrinking>();

   
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

            foreach (var shrinker in _shrinkers)
            {
                shrinker.CanShrink = true;
                StartCoroutine(StopShrinking());
            }
            _scoreToRaiseDifficulty = 0;
        }

    }

    IEnumerator StopShrinking()
    {
        yield return new WaitForSeconds(_timeToShrink);
        foreach (var shrinker in _shrinkers)
        {
            shrinker.CanShrink = false;
        }
    }

}
