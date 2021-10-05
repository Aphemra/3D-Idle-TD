using System;
using System.Collections.Generic;
using Code.Components;
using Code.Resources;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Managers
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private Transform enemyParent;
        [SerializeField] private List<EnemyComponent> spawnedEnemies;

        private void Awake()
        {
            if (Game.EnemyManager == null)
                Game.EnemyManager = this;
        }

        public void SpawnEnemy(Transform whereToSpawn)
        {
            EnemyComponent enemy = Instantiate(enemyPrefab, whereToSpawn.position, Quaternion.identity, enemyParent).GetComponent<EnemyComponent>();
            
            var randomInt = Random.Range(0, 2);

            switch (randomInt)
            {
                case 0:
                    enemy.SetEnemyType(EnemyType.Melee);
                    break;
                case 1:
                    enemy.SetEnemyType(EnemyType.Range);
                    break;
            }
            
            AddSpawnedEnemy(enemy);
        }

        private void AddSpawnedEnemy(EnemyComponent enemyToAdd)
        {
            spawnedEnemies.Add(enemyToAdd);
        }

        public void RemoveSpawnedEnemy(EnemyComponent enemyToRemove)
        {
            if (!spawnedEnemies.Contains(enemyToRemove)) return;
            
            spawnedEnemies.Remove(enemyToRemove);
            Destroy(enemyToRemove.gameObject);
        }

        public List<EnemyComponent> GetSpawnedEnemiesList()
        {
            return spawnedEnemies;
        }
    }
}