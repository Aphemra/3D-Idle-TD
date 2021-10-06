using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Managers
{
    public class SpawnManager : CoreManager
    {
        [SerializeField] private float spawnDelay;
        [SerializeField] private int totalEnemyCount; // Replace with wave variable eventually

        [SerializeField] private List<GameObject> allSpawners;
        
        private void Awake()
        {
            allSpawners = new List<GameObject>();
            
            if (Game.SpawnManager == null)
                Game.SpawnManager = this;
        }

        public void SpawnEnemies()
        {
            StartCoroutine(SpawnEnemiesCoroutine());
        }
        
        public void SpawnEnemiesWithEnemyCount(int enemiesToSpawn)
        {
            totalEnemyCount = enemiesToSpawn;
            StartCoroutine(SpawnEnemiesCoroutine());
        }

        IEnumerator SpawnEnemiesCoroutine()
        {
            var count = 0;
            print("Enemies To Spawn: " + totalEnemyCount);

            while (count < totalEnemyCount)
            {
                Game.EnemyManager.SpawnEnemy(allSpawners[Random.Range(0, allSpawners.Count)].transform);
                yield return new WaitForSeconds(spawnDelay);
                count++;
                print("Enemies Spawned: " + count);
            }
        }

        public void AddSpawnersToAllSpawnersList(List<GameObject> listToAdd)
        {
            allSpawners.AddRange(listToAdd);
        }
    }
}