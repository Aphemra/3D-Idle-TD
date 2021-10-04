using System;
using System.Collections.Generic;
using Code.Components;
using UnityEngine;

namespace Code.Managers
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private List<EnemyComponent> spawnedEnemies;

        public bool spawnEnemies = false;
        
        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        private void Awake()
        {
            if (Game.EnemyManager == null)
                Game.EnemyManager = this;
        }

        public void AddSpawnedEnemy(EnemyComponent enemyToAdd)
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