using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    private DifficultyManager _difficultyManager;
    private GameManager _gameManager;
    
    //Spawn
    private Vector3 _spawnPos;
    private float _ySpawnPos;
    private float _spawnRate = 5f;

    private Vector3 _spawnPowerupPos;
    private int _xPowerupSpawnPos;
    private int _yPowerupSpawnPos;

    private float _powerupSpawnRate = 20f;

    private void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _difficultyManager = GameObject.Find("DifficultyManager").GetComponent<DifficultyManager>();
        
        
        EventBroker.StartGameHandler += StartSpawning;
        EventBroker.GameOverHandler += GameOver;
    }
    private void StartSpawning()
    {
        EventBroker.DifficultyHandler += _difficultyManager.ModifyDifficulty;
        //_difficultyManager.CreateShrinkersList();
        EventBroker.CallCreateShrinkerList();
        
        InvokeRepeating(nameof(SpawnObstacle),0.5f,_spawnRate * _difficultyManager.DifficultyModifier);
        InvokeRepeating(nameof(SpawnPowerups),0.5f,_powerupSpawnRate * _difficultyManager.DifficultyModifier);
        InvokeRepeating(nameof(SpawnCoins), 0.5f,_spawnRate * _difficultyManager.DifficultyModifier);
        InvokeRepeating(nameof(SpawnClouds),0.5f,28f);
    }

    private void GameOver()
    {
        StopAllCoroutines();
    }

    private void SpawnObstacle()
    {

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

    private void SpawnPowerups()
    {

            _yPowerupSpawnPos = Random.Range(-3, 10); 
            _xPowerupSpawnPos = Random.Range(12, 18);
            _spawnPowerupPos = new Vector3(_xPowerupSpawnPos, _yPowerupSpawnPos, 0);

            GameObject pooledPowerup = ObjectPooler.SharedInstance.GetPooledPowerup();
            if (pooledPowerup != null)
            {
                pooledPowerup.SetActive(true);
                pooledPowerup.transform.position = _spawnPowerupPos;
                
                //check if needs refactoring
                for (int i = 0; i < pooledPowerup.transform.childCount; i++)
                {
                    pooledPowerup.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
            _powerupSpawnRate = GetSpawnRate();
    }

    private void SpawnCoins()
    {
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


    private void SpawnClouds()
    {
            Vector3 spawnCloudsPos = new Vector3(100, 0, 0);

            GameObject pooledCouldsPatter = ObjectPooler.SharedInstance.GetPooledCloudPattern();
            
            if (pooledCouldsPatter != null)
            {
                pooledCouldsPatter.SetActive(true);
                pooledCouldsPatter.transform.position = spawnCloudsPos;
            }
    }

    //Chooses between 2 spawn rates of powerups
    private float GetSpawnRate()
    {
        var randomValue = Random.value;
        if (randomValue > .50)
            return 15f;
        
        return 20f;
    }

    private void OnDestroy()
    {
        EventBroker.StartGameHandler -= StartSpawning;
        EventBroker.GameOverHandler -= GameOver;
        EventBroker.DifficultyHandler -= _difficultyManager.ModifyDifficulty;
    }

}
