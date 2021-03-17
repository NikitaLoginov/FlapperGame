using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance;
    
    //Obstacles
    public List<GameObject> obstaclesToPool;
    public int amountObstaclesToPool;
    [HideInInspector] public List<GameObject> pooledObstacles;

    //Powerups
    public List<GameObject> powerupsToPool;
    private List<GameObject> _pooledPowerups;
    public int amountPowerupsToPool;

    //Coins
    public List<GameObject> coinsToPool;
    private List<GameObject> _pooledCoinPatterns;
    public int amountCoinPatternsToPool;

    //Clouds
    public List<GameObject> cloudsToPool;
    private List<GameObject> _pooledCloudPatterns;
    public int amountCloudPatternsToPool;

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        //Loop through list of objects to pool
        //deactivating them and adding them
        //to the list of pooled objects
        pooledObstacles = new List<GameObject>();
        AddCoinsToPool(obstaclesToPool,amountObstaclesToPool,pooledObstacles);

        _pooledPowerups = new List<GameObject>();
        AddCoinsToPool(powerupsToPool,amountPowerupsToPool,_pooledPowerups);

        _pooledCoinPatterns = new List<GameObject>();
        AddCoinsToPool(coinsToPool, amountCoinPatternsToPool, _pooledCoinPatterns);
        
        _pooledCloudPatterns = new List<GameObject>();
        AddCoinsToPool(cloudsToPool,amountCloudPatternsToPool,_pooledCloudPatterns);
    }

    private void AddCoinsToPool(List<GameObject> objToPool, int amount, List<GameObject> pooledObj)
    {
        for (int i = 0; i < amount; i++)
        {
            for (int j = 0; j < objToPool.Count; j++)
            {
                GameObject obj = (GameObject) Instantiate(objToPool[j], this.transform, true);
                obj.SetActive(false);
                pooledObj.Add(obj);
            }
        }
    }

    public GameObject GetPooledObstacle()
    {
        for (int i = 0; i < pooledObstacles.Count; i++)
        {
            //If pooled object is not active, return that object
            if (!pooledObstacles[i].activeInHierarchy)
            {
                return pooledObstacles[i];
            }
        }
        //Otherwise return null
        return null;
    }

    public GameObject GetPooledPowerup()
    {
        for (int i = 0; i < _pooledPowerups.Count; i++)
        {
            int j = SimpleDiceRoller(); 
            if (!_pooledPowerups[j].activeInHierarchy)
            {
                return _pooledPowerups[j];
            }    
        }
        return null;
    }

    public GameObject GetPooledCoinPattern()
    {
        while (true)
        {
            float randomValue = Random.value;

            if (randomValue < .20)
            {
                GameObject trianglePattern = _pooledCoinPatterns.Find(x => x.name.Contains("CoinPatternTriangle"));

                if (!trianglePattern.activeInHierarchy) return trianglePattern;
            }
            else if (randomValue > .20 && randomValue < .40)
            {
                GameObject rombPattern = _pooledCoinPatterns.Find(x => x.name.Contains("CoinPatternRomb"));

                if (!rombPattern.activeInHierarchy) return rombPattern;
            }
            else if (randomValue > .40 && randomValue < .50)
            {
                GameObject bCoinPattern = _pooledCoinPatterns.Find((x => x.name.Contains("CoinPatternBCoin")));

                if (!bCoinPattern.activeInHierarchy) return bCoinPattern;
            }
            else if (randomValue > .50 && randomValue < .75)
            {
                GameObject linePattern = _pooledCoinPatterns.Find(x => x.name.Contains("CoinPatternLine"));

                if (!linePattern.activeInHierarchy) return linePattern;
            }
            else if (randomValue > .75)
            {
                GameObject jumpPattern = _pooledCoinPatterns.Find(x => x.name.Contains("CoinPatternJump"));
                if (!jumpPattern.activeInHierarchy) return jumpPattern;
            }
        }
    }

    public GameObject GetPooledCloudPattern()
    {
        for (int i = 0; i < _pooledCloudPatterns.Count; i++)
        {
            if (!_pooledCloudPatterns[i].activeInHierarchy)
            {
                return _pooledCloudPatterns[i];
            }
        }

        return null;
    }

    //Calculates probability of spawning different types of powerups
    private int SimpleDiceRoller()
    {
        float randValue = Random.value;

        int dashIndex = 0;
        int slowMoIndex = 1;
        int invulIndex = 2;

        if (randValue < .45f)
        {
            return dashIndex;
        }
        else if (randValue > .45f && randValue < .80f)
        {
            return slowMoIndex;
        }
        else
        {
            return invulIndex;
        }
    }
}
