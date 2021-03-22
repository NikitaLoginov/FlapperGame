using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    //difficulty
    private float _difficultyRaise = 0.02f;
    private float _difficultyModifier = 1f;
    private float _maxDifficultyModifier = 0.7f;
    private int _scoreToRaiseDifficulty = 0;
    private float _lower = -28;
    private float _upper = 28;

    public float DifficultyModifier =>_difficultyModifier; 
    private SpeedManager _speedManager;

    private void Start()
    {
        _speedManager = GameObject.Find("SpeedManager").GetComponent<SpeedManager>();
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
            
            EventBroker.CallShrinking();
            _scoreToRaiseDifficulty = 0;
        }
    }
}
