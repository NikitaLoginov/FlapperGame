﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject tapPatterToPool;
    public GameObject slidePatternToPool;
    public GameObject abducePatternToPool;
    public int amountCoinPatternsToPool;

    private void Awake()
    {
        SharedInstance = this;
    }
    void Start()
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
            GameObject tapPattern = (GameObject)Instantiate(tapPatterToPool);
            GameObject slidePattern = (GameObject)Instantiate(slidePatternToPool);
            GameObject abducePattern = (GameObject)Instantiate(abducePatternToPool);

            tapPattern.SetActive(false);
            slidePattern.SetActive(false);
            abducePattern.SetActive(false);

            pooledCoinPatterns.Add(tapPattern);
            pooledCoinPatterns.Add(slidePattern);
            pooledCoinPatterns.Add(abducePattern);

            tapPattern.transform.SetParent(this.transform);
            slidePattern.transform.SetParent(this.transform);
            abducePattern.transform.SetParent(this.transform);
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
        for (int i = 0; i < pooledCoinPatterns.Count; i++)
        {
            if (!pooledCoinPatterns[i].activeInHierarchy)
            {
                return pooledCoinPatterns[i];
            }
        }

        return null;
    }

    //Calculates probability of spawning different types of powerups
    int SimpleDiceRoller()
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
