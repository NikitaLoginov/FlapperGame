using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ObjectPooler : MonoBehaviour
{
    //Obstacles
    public static ObjectPooler SharedInstance;
    public List<GameObject> pooledObstacles;
    public GameObject obstaclesToPool;
    public int amountObstaclesToPool;

    //Powerups
    public List<GameObject> pooledPowerups;
    public GameObject dashPowerupToPool;
    public GameObject slowMoPowerupToPool;
    public GameObject invincibilityPowerupToPool;
    public int amountPowerupsToPool;

    //Coins
    public List<GameObject> pooledCoinPatterns;
    [FormerlySerializedAs("tapPatterToPool")] public GameObject trianglePatterToPool;
    [FormerlySerializedAs("slidePatternToPool")] public GameObject rombPatternToPool;
    [FormerlySerializedAs("abducePatternToPool")] public GameObject bCoinPatternToPool;
    public GameObject jumpCoinPatternToPool;
    public GameObject lineCoinPatternToPool;
    public int amountCoinPatternsToPool;

    //Clouds
    public List<GameObject> pooledCloudPatterns;
    public GameObject cloudPattern01ToPool;
    public GameObject cloudPattern02ToPool;
    public GameObject cloudPattern03ToPool;
    public int amountCloudPatternsToPool;

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        //Loop through list of pooled objects deactivating them and adding them to the list
        pooledObstacles = new List<GameObject>();
        for (int i = 0; i < amountObstaclesToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(obstaclesToPool);
            obj.SetActive(false);
            pooledObstacles.Add(obj);
            obj.transform.SetParent(this.transform); //set child of Game Manager or other object attached to script
        }


        pooledPowerups = new List<GameObject>();
        for (int i = 0; i < amountPowerupsToPool; i++)
        {
            GameObject dash = (GameObject)Instantiate(dashPowerupToPool);
            GameObject slow = (GameObject)Instantiate(slowMoPowerupToPool);
            GameObject invinc = (GameObject)Instantiate(invincibilityPowerupToPool);

            dash.SetActive(false);
            slow.SetActive(false);
            invinc.SetActive(false);

            pooledPowerups.Add(dash);
            pooledPowerups.Add(slow);
            pooledPowerups.Add(invinc);

            dash.transform.SetParent(this.transform);
            slow.transform.SetParent(this.transform);
            invinc.transform.SetParent(this.transform);
        }

        pooledCoinPatterns = new List<GameObject>();
        for (int i = 0; i < amountCoinPatternsToPool; i++)
        {
            GameObject tapPattern = (GameObject)Instantiate(trianglePatterToPool);
            GameObject slidePattern = (GameObject)Instantiate(rombPatternToPool);
            GameObject abducePattern = (GameObject)Instantiate(bCoinPatternToPool);
            GameObject linePattern = (GameObject) Instantiate(lineCoinPatternToPool);
            GameObject jumpPattern = (GameObject) Instantiate(jumpCoinPatternToPool);

            tapPattern.SetActive(false);
            slidePattern.SetActive(false);
            abducePattern.SetActive(false);
            linePattern.SetActive(false);
            jumpPattern.SetActive(false);

            pooledCoinPatterns.Add(tapPattern);
            pooledCoinPatterns.Add(slidePattern);
            pooledCoinPatterns.Add(abducePattern);
            pooledCoinPatterns.Add(linePattern);
            pooledCoinPatterns.Add(jumpPattern);

            tapPattern.transform.SetParent(this.transform);
            slidePattern.transform.SetParent(this.transform);
            abducePattern.transform.SetParent(this.transform);
            linePattern.transform.SetParent(this.transform);
            jumpPattern.transform.SetParent(this.transform);
        }

        pooledCloudPatterns = new List<GameObject>();
        for (int i = 0; i < amountCloudPatternsToPool; i++)
        {
            GameObject cloudPattern01 = (GameObject)Instantiate(cloudPattern01ToPool);
            GameObject cloudPattern02 = (GameObject)Instantiate(cloudPattern02ToPool);
            GameObject cloudPattern03 = (GameObject)Instantiate(cloudPattern03ToPool);

            cloudPattern01.SetActive(false);
            cloudPattern02.SetActive(false);
            cloudPattern03.SetActive(false);

            pooledCloudPatterns.Add(cloudPattern01);
            pooledCloudPatterns.Add(cloudPattern02);
            pooledCloudPatterns.Add(cloudPattern03);

            cloudPattern01.transform.SetParent(this.transform);
            cloudPattern02.transform.SetParent(this.transform);
            cloudPattern03.transform.SetParent(this.transform);
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
        for (int i = 0; i < pooledPowerups.Count; i++)
        {
            int j = SimpleDiceRoller(); 
            if (!pooledPowerups[j].activeInHierarchy)
            {
                return pooledPowerups[j];
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
                GameObject trianglePattern = pooledCoinPatterns.Find(x => x.name.Contains("CoinPatternTriangle"));

                if (!trianglePattern.activeInHierarchy) return trianglePattern;
            }
            else if (randomValue > .20 && randomValue < .40)
            {
                GameObject rombPattern = pooledCoinPatterns.Find(x => x.name.Contains("CoinPatternRomb"));

                if (!rombPattern.activeInHierarchy) return rombPattern;
            }
            else if (randomValue > .40 && randomValue < .50)
            {
                GameObject bCoinPattern = pooledCoinPatterns.Find((x => x.name.Contains("CoinPatternBCoin")));

                if (!bCoinPattern.activeInHierarchy) return bCoinPattern;
            }
            else if (randomValue > .50 && randomValue < .75)
            {
                GameObject linePattern = pooledCoinPatterns.Find(x => x.name.Contains("CoinPatternLine"));

                if (!linePattern.activeInHierarchy) return linePattern;
            }
            else if (randomValue > .75)
            {
                GameObject jumpPattern = pooledCoinPatterns.Find(x => x.name.Contains("CoinPatternJump"));
                if (!jumpPattern.activeInHierarchy) return jumpPattern;
            }
        }
    }

    public GameObject GetPooledCloudPattern()
    {
        for (int i = 0; i < pooledCloudPatterns.Count; i++)
        {
            if (!pooledCloudPatterns[i].activeInHierarchy)
            {
                return pooledCloudPatterns[i];
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
