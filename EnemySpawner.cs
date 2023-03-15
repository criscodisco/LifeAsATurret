using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy")]
    public Enemy[] enemies;
    Enemy enemyInstance;

    void Start()
    {
        // Continue spawning asteroids above player every 2 seconds
        InvokeRepeating("SpawnEnemy", 3.0f, 2.0f);
    }

    void SpawnEnemy()
    {
        // stores random integer to be used to select which asteroid prefab to spawn from an array
        int randomIndex = UnityEngine.Random.Range(0, enemies.Length);

        // Stores a random vector to spawn each asteroid at
        Vector3 randomSpawnPosition = new Vector3(UnityEngine.Random.Range(-12, 20), 30, UnityEngine.Random.Range(0, 26));

        // Spawn asteroid
        enemyInstance = Instantiate(enemies[randomIndex], randomSpawnPosition, Quaternion.identity);

        // Set random scale of this asteroid
        float randomScale = (float) (UnityEngine.Random.Range(.01f, 1f));
        enemyInstance.transform.localScale = new Vector3(randomScale, randomScale, randomScale);     
    }
}
