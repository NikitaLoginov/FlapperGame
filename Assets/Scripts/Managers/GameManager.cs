using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    //Player
    [SerializeField] private GameObject player;
    private DifficultyManager _difficultyManager;
    private TimeManager _timeManager;
    private SpeedManager _speedManager;

    //Variables
    private bool _isGameOver;
    public bool IsGameOver { get { return _isGameOver; } }

    //Spawn
    private Vector3 _spawnPos;
    private float _ySpawnPos;
    private float _spawnRate = 5f;

    private Vector3 _spawnPowerupPos;
    private int _xPowerupSpawnPos;
    private int _yPowerupSpawnPos;

    private float _powerupSpawnRate;

    private void Awake()
    {
        _isGameOver = true;
        
        _difficultyManager = FindObjectOfType<DifficultyManager>();
        _timeManager = FindObjectOfType<TimeManager>();
        EventBroker.CanStartGameHandler += CanStartGame;
        EventBroker.StartGameHandler += StartGame;
    }

    private void Start()
    {
        EventBroker.CallCanStartGame();
    }

    private void CanStartGame()
    {
        player.gameObject.SetActive(true);

        _timeManager.StopTime(true);
    }

    private void Update()
    {
        //for debuging
        if (Input.GetKeyDown(KeyCode.H))
        {
            player.gameObject.SetActive(false);
        }
    }

    public void StartGame()
    {
        //Variables
        _isGameOver = false;

        //Events
        EventBroker.GameOverHandler += GameOver;
        EventBroker.RestartGameHandler += RestartGame;

        EventBroker.DifficultyHandler += _difficultyManager.ModifyDifficulty;
        _difficultyManager.CreateShrinkersList();
        
        //Coroutines
        StartCoroutine(SpawnObstacle());
        StartCoroutine(SpawnPowerups());
        StartCoroutine(SpawnCoins());
        StartCoroutine(SpawnClouds());
        
        _timeManager.StopTime(false);
        
    }

    private void GameOver()
    {
        _isGameOver = true;
        _timeManager.StopTime(true);
        
        StopAllCoroutines();
    }

    //Restarting scene 
    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void Unsubscribe()
    {
        EventBroker.DifficultyHandler -= _difficultyManager.ModifyDifficulty;
        EventBroker.GameOverHandler -= GameOver;
        EventBroker.RestartGameHandler -= RestartGame;

        EventBroker.CanStartGameHandler -= CanStartGame;
        EventBroker.StartGameHandler -= StartGame;
    }

    private IEnumerator SpawnObstacle()
    {
        while (!_isGameOver)
        {
            yield return new WaitForSeconds(_spawnRate * _difficultyManager.DifficultyModifier);

            _ySpawnPos = Random.Range(1, 10); //-3, 10
            _spawnPos = new Vector3(20, _ySpawnPos, 0);

            GameObject pooledObstacle = ObjectPooler.SharedInstance.GetPooledObstacle();
            if (pooledObstacle != null)
            {
                pooledObstacle.SetActive(true);
                pooledObstacle.transform.position = _spawnPos;

                for (int i = 0; i < pooledObstacle.transform.childCount; i++)
                {
                    Transform child = pooledObstacle.transform.GetChild(i);
                    //we need one more loop since we trying to reference a grandchild object
                    for (int j = 0; j < child.transform.childCount; j++)
                    {
                        child.transform.GetChild(j).gameObject.SetActive(true);
                    }
                    
                }
            }
        }
    }

    private IEnumerator SpawnPowerups()
    {
        while (!_isGameOver)
        {
            _powerupSpawnRate = GetSpawnRate();
            yield return new WaitForSeconds(_powerupSpawnRate * _difficultyManager.DifficultyModifier);

            _yPowerupSpawnPos = Random.Range(-3, 10); 
            _xPowerupSpawnPos = Random.Range(12, 18);
            _spawnPowerupPos = new Vector3(_xPowerupSpawnPos, _yPowerupSpawnPos, 0);

            GameObject pooledPowerup = ObjectPooler.SharedInstance.GetPooledPowerup();
            if (pooledPowerup != null)
            {
                pooledPowerup.SetActive(true);
                pooledPowerup.transform.position = _spawnPowerupPos;
            }
        }
    }

    private IEnumerator SpawnCoins()
    {
        while (!_isGameOver)
        {
            yield return new WaitForSeconds(_spawnRate * _difficultyManager.DifficultyModifier);

            Vector3 spawnCoinsPosition = new Vector3(12, -2.5f, 0); //reconfigure hard coded values!

            GameObject pooledCoinsPattern =ObjectPooler.SharedInstance.GetPooledCoinPattern();
            if (pooledCoinsPattern != null)
            {
                pooledCoinsPattern.SetActive(true);
                pooledCoinsPattern.transform.position = spawnCoinsPosition;

                //check if needs refactoring
                for (int i = 0; i < pooledCoinsPattern.transform.childCount; i++)
                {
                    pooledCoinsPattern.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
    }


    private IEnumerator SpawnClouds()
    {
        while (!_isGameOver)
        {
            yield return new WaitForSeconds(.5f);

            Vector3 spawnCloudsPos = new Vector3(100, 0, 0);

            GameObject pooledCouldsPatter = ObjectPooler.SharedInstance.GetPooledCloudPattern();
            
            if (pooledCouldsPatter != null)
            {
                pooledCouldsPatter.SetActive(true);
                pooledCouldsPatter.transform.position = spawnCloudsPos;
            }

            yield return new WaitForSeconds(28f); //magic number is a spawn rate of clouds which is constant throughout the game
        }
    }

    //Chooses between 2 spawn rates of powerups
    private float GetSpawnRate()
    {
        float randomValue = Random.value;
        if (randomValue > .50)
            return 15f;
        else
            return 20f;
    }

}
