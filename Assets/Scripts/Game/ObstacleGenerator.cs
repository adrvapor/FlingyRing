using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    private string[] obstacleTags = { "RockLeft", "RockRight", "RockCenter"};
    private string[] enemyTags = { "Urchin", "Eel" };

    private int maxObstacleIdx = 3;
    private int maxEnemyIdx = 0;

    public bool enableEnemies;

    private float nextY;

    ObjectPooler objectPooler;

    private void Start()
    {
        enableEnemies = false;
        nextY = 6;
        objectPooler = FindObjectOfType<ObjectPooler>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var currentPos = transform.position;

        IncreaseDifficulty(currentPos.y);

        if (currentPos.y >= nextY)
        {
            nextY = currentPos.y + UnityEngine.Random.Range(2f, 5f);

            string tag = RandomizeNextObstacle();
            var margin = new Vector3(0, 1, 0);

            if (tag == "Urchin") margin += new Vector3(UnityEngine.Random.Range(-2f, 2f), 0, 0);
            
            var obstacle = objectPooler.SpawnFromPool(tag, currentPos + margin);
            if (tag == "Eel")
                obstacle.transform.localScale += new Vector3(obstacle.transform.localScale.x * -2f * UnityEngine.Random.Range(0, 2), 0, 0);

            if (UnityEngine.Random.Range(0f, 1f) <= 0.3)
            objectPooler.SpawnFromPool("Pearl", currentPos + new Vector3(UnityEngine.Random.Range(-2f, 2f), 2.5f, 0));
        }
    }

    public string RandomizeNextObstacle()
    {
        float randomType = UnityEngine.Random.Range(0f, 1f);

        if (randomType <= 0.6 && enableEnemies)
        {
            return enemyTags[UnityEngine.Random.Range(0, maxEnemyIdx)];
        }
        else
        {
            return obstacleTags[UnityEngine.Random.Range(0, maxObstacleIdx)];
        }
    }

    private void IncreaseDifficulty(float currentY)
    {
        if (currentY > 20f)
            enableEnemies = true;

        if (currentY > 50f && maxEnemyIdx < 2)
            maxEnemyIdx = 2;
    }
}
