using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    private string[] terrainTags = { "RockLeft", "RockRight", "RockCenter" };
    private string[] enemyTags = { "Urchin", "Eel", "Oyster", "Shark", "Octopus" };

    private Queue<GameObject> lastObjects = new Queue<GameObject>(5);
    private Queue<string> lastObstacles = new Queue<string>(3);
    private Queue<string> lastEnemies = new Queue<string>(2);
    private string lastTerrain = "";

    private int maxObstacleIdx = 3;
    private int maxEnemyIdx = 0;

    public bool enableEnemies;

    private float nextY;

    public GameObject Pole;
    private float nextPowerUp;
    private float powerUpDistance = 50f;

    public GameObject SeaFloor;

    ObjectPooler objectPooler;


    private void Start()
    {
        enableEnemies = false;
        nextY = 6;
        nextPowerUp = powerUpDistance;
        objectPooler = FindObjectOfType<ObjectPooler>();

        GameManager.SetObstacleGenerator(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var currentPos = transform.position;

        IncreaseDifficulty(currentPos.y);

        if (currentPos.y >= nextY)
        {
            nextY = currentPos.y + UnityEngine.Random.Range(2.5f, 5f);

            // If it's time for a powerup we spawn it and return
            if(currentPos.y >= nextPowerUp)// && UnityEngine.Random.Range(0f, 1f) > 0.5)
            {
                nextPowerUp += powerUpDistance;
                
                Instantiate(Pole).GetComponent<PoleController>().SpawnPole(currentPos.y);

                return;
            } 

            string tag = RandomizeNextObstacle();
            var margin = new Vector3(0, 1, 0);

            if (tag == "Urchin") margin += new Vector3(UnityEngine.Random.Range(-2f, 2f), 0, 0);

            var obstacle = objectPooler.SpawnFromPool(tag, currentPos + margin);

            if (lastObjects.Count >= 5) lastObjects.Dequeue();
            lastObjects.Enqueue(obstacle);

            if (tag == "Eel" || tag == "Shark" || tag == "Octopus")
                obstacle.transform.localScale += new Vector3(obstacle.transform.localScale.x * -2f * UnityEngine.Random.Range(0, 2), 0, 0);

            if (tag == "Oyster")
            {
                var random = UnityEngine.Random.Range(0, 2);
                obstacle.transform.localScale += new Vector3(obstacle.transform.localScale.x * -2f * random, 0, 0);

                var oysterController = obstacle.GetComponent<OysterController>();
                oysterController.Pearl.transform.localScale += new Vector3(oysterController.Pearl.transform.localScale.x * -2f * random, 0, 0);
                oysterController.ResetPearl();

                nextY += 1;
            }

            if (tag == "Octopus") nextY += 6;

            if (UnityEngine.Random.Range(0f, 1f) <= 0.3)
            {
                GameObject pearl = objectPooler.SpawnFromPool("Pearl", currentPos + new Vector3(UnityEngine.Random.Range(-2f, 2f), 2.5f, 0));
                while (Physics.CheckSphere(pearl.transform.position, 1))
                {
                    pearl.transform.position += new Vector3(-pearl.transform.position.x + UnityEngine.Random.Range(-2f, 2f), 0, 0);
                }

            }
        }
    }

    public string RandomizeNextObstacle()
    {
        float randomType = UnityEngine.Random.Range(0f, 1f);
        string nextObstacle;

        int obstacles = 0;
        int enemies = 0;

        foreach (string obstacle in lastObstacles)
        {
            if (Array.Exists(terrainTags, o => o == obstacle)) obstacles++;
        }

        if ((randomType <= 0.6 || obstacles > 2) && enableEnemies)
        {
            int sameEnemyIterations;
            do
            {
                sameEnemyIterations = 0;
                nextObstacle = enemyTags[UnityEngine.Random.Range(0, maxEnemyIdx)];
                foreach (string enemy in lastEnemies)
                {
                    if (enemy == nextObstacle) sameEnemyIterations++;
                }
            }
            while (maxEnemyIdx > 1 && sameEnemyIterations > 2);

            if (lastEnemies.Count >= 3)
                lastEnemies.Dequeue();
            lastEnemies.Enqueue(nextObstacle);
        }
        else
        {
            do
            {
                nextObstacle = terrainTags[UnityEngine.Random.Range(0, maxObstacleIdx)];
            }
            while (nextObstacle == lastTerrain);

            lastTerrain = nextObstacle;
        }

        if (lastObstacles.Count >= 3)
            lastObstacles.Dequeue();
        lastObstacles.Enqueue(nextObstacle);

        return nextObstacle;
    }

    private void IncreaseDifficulty(float currentY)
    {
        if (currentY > 20f)
            enableEnemies = true;

        if (currentY > 40f && maxEnemyIdx < 2)
            maxEnemyIdx = 2;

        if (currentY > 60f && maxEnemyIdx < 3)
            maxEnemyIdx = 3;

        if (currentY > 80f && maxEnemyIdx < 4)
            maxEnemyIdx = 4;

        if (currentY > 100f && maxEnemyIdx < 5)
            maxEnemyIdx = 5;
    }

    public float GetRespawnPosition()
    {
        return this.transform.position.y - 6;
    }

    public void ClearObstaclesAtRespawn()
    {
        float margin = GetRespawnPosition() + 2;

        foreach(var obj in lastObjects)
        {
            if (obj.transform.position.y < margin) obj.transform.position = obj.transform.position - new Vector3(0, 10, 0);
        }

        SeaFloor.transform.position += new Vector3(0, GetRespawnPosition());
    }
}
