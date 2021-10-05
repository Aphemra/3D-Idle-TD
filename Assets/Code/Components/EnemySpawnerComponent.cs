using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Components
{
    public class EnemySpawnerComponent : MonoBehaviour
    {
        [SerializeField] private SpawnerLocation location;
        [SerializeField] private GameObject spawnerPrefab;
        
        [SerializeField] private List<GameObject> spawnPoints;

        private void Awake()
        {
            spawnPoints = new List<GameObject>();
        }

        public void GenerateSpawnPoints()
        {
            int spawnPointsToSpawn = 0;

            switch (location)
            {
                case SpawnerLocation.North:
                    spawnPointsToSpawn = Mathf.FloorToInt(transform.localScale.x / (transform.localScale.x * .15f));
                    InstantiateSpawnPoint(transform.localScale.x, spawnPointsToSpawn, SpawnerLocation.North);
                    break;
                case SpawnerLocation.South:
                    spawnPointsToSpawn = Mathf.FloorToInt(transform.localScale.x / (transform.localScale.x * .15f));
                    InstantiateSpawnPoint(transform.localScale.x, spawnPointsToSpawn, SpawnerLocation.South);
                    break;
                case SpawnerLocation.East:
                    spawnPointsToSpawn = Mathf.FloorToInt(transform.localScale.y / (transform.localScale.y * .15f));
                    InstantiateSpawnPoint(transform.localScale.y, spawnPointsToSpawn, SpawnerLocation.East);
                    break;
                case SpawnerLocation.West:
                    spawnPointsToSpawn = Mathf.FloorToInt(transform.localScale.y / (transform.localScale.y * .15f));
                    InstantiateSpawnPoint(transform.localScale.y, spawnPointsToSpawn, SpawnerLocation.West);
                    break;
            }
        }

        private void InstantiateSpawnPoint(float transformScale, int numberOfSpawnPoints, SpawnerLocation spawnLocation)
        {
            var incrementsToSpawn = (transformScale - 1) / numberOfSpawnPoints;

            if (spawnLocation == SpawnerLocation.North || spawnLocation == SpawnerLocation.South)
            {
                // Spawns spawners inside walls at uniform lengths across the walls
                for (float i = (transform.position.x - ((transform.localScale.x - 1) / 2)) + incrementsToSpawn;
                    i <= (transform.position.x + ((transform.localScale.x - 1) / 2)) - incrementsToSpawn;
                    i += incrementsToSpawn)
                {
                    var position = new Vector3(i, transform.position.y, transform.position.z);
                    var spawner = Instantiate(spawnerPrefab, position, Quaternion.identity, transform);
                    spawner.name = spawnLocation + " Spawner at " + i;
                    spawnPoints.Add(spawner);
                }
                
                Game.SpawnManager.AddSpawnersToAllSpawnersList(spawnPoints);
            }
            
            if (spawnLocation == SpawnerLocation.East || spawnLocation == SpawnerLocation.West)
            {
                // Spawns spawners inside walls at uniform lengths across the walls
                for (float i = (transform.position.y - ((transform.localScale.y - 1) / 2)) + incrementsToSpawn;
                    i <= (transform.position.y + ((transform.localScale.y - 1) / 2)) - incrementsToSpawn;
                    i += incrementsToSpawn)
                {
                    var position = new Vector3(transform.position.x, i, transform.position.z);
                    var spawner = Instantiate(spawnerPrefab, position, Quaternion.identity, transform);
                    spawner.name = spawnLocation + " Spawner at " + i;
                    spawnPoints.Add(spawner);
                }
                
                Game.SpawnManager.AddSpawnersToAllSpawnersList(spawnPoints);
            }
        }

        public SpawnerLocation GetLocation()
        {
            return location;
        }
    }

    public enum SpawnerLocation
    {
        North,
        South,
        East,
        West
    }
}
