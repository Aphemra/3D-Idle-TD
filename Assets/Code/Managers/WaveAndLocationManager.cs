using System;
using System.Collections;
using Sirenix.Utilities;
using UnityEngine;

namespace Code.Managers
{
    public class WaveAndLocationManager : CoreManager
    {
        [SerializeField] [Range(0.5f, 3f)] private float difficultyMultiplier;
        [SerializeField] private bool autoMoveWave;
        [SerializeField] private bool waveInProgress;
        [SerializeField] private float secondsBetweenAutoMoveWaves;

        private void Awake()
        {
            if (Game.WaveAndLocationManager == null)
                Game.WaveAndLocationManager = this;
            
            // Debug -> TODO: Eventually these should be initialized from save data
            Game.Location = 1;
            Game.Wave = 1;
        }
        
        // When Start Wave button is pressed, check if auto move toggle box is checked
        // If it is checked, continue spawning waves with a delay between each wave starting
        // If it is not checked, wait for player to press Start Wave button again

        private void Update()
        {
            CheckForLoss();
            CheckForWin();
        }

        private void CheckForLoss()
        {
            if (waveInProgress && Game.TowerManager.GetActiveTowers().IsNullOrEmpty())
            {
                if (Game.Wave - 1 < 1)
                {
                    if (Game.Location - 1 < 1)
                    {
                        Game.Wave = 1;
                        Game.Location = 1;
                    }
                    else
                    {
                        Game.Wave = 10; // Set this to max wave per location eventually
                        Game.Location--;
                    }
                }
                else
                {
                    Game.Wave--;
                }
                waveInProgress = false;
                SetAutoMove(false);
                Game.Events.OnInfoUpdated.Invoke();

                ResetLevel();
            }
        }

        private void CheckForWin()
        {
            if (waveInProgress && Game.EnemyManager.GetSpawnedEnemiesList().IsNullOrEmpty())
            {
                if (Game.Wave + 1 > 10) // Check this against max wave per location eventually
                {
                    Game.Wave = 1;
                    Game.Location++;
                }
                else
                {
                    Game.Wave++;
                }
                waveInProgress = false;
                Game.Events.OnInfoUpdated.Invoke();
                
                if (autoMoveWave)
                    StartWaveAuto();
            }
        }

        private void ResetLevel()
        {
            StartCoroutine(ResetLevelCoroutine());
        }

        IEnumerator ResetLevelCoroutine()
        {
            yield return new WaitForSeconds(1.5f);
            
            foreach (var enemy in Game.EnemyManager.GetSpawnedEnemiesList())
            {
                Destroy(enemy.gameObject);
            }
            Game.EnemyManager.GetSpawnedEnemiesList().Clear();
            HealAllTowers();
        }

        private void SetAutoMove(bool state)
        {
            Game.HUDManager.SetAutoToggle(state);
            autoMoveWave = state;
        }

        private void HealAllTowers()
        {
            Game.TowerManager.MoveAllInactiveTowersToActive();
            
            foreach (var towerComponent in Game.TowerManager.GetActiveTowers())
            {
                towerComponent.ReviveTower();
                towerComponent.HealToMaxHealth();
            }
        }

        private void StartWaveAuto()
        {
            StartCoroutine(StartWaveAutoCoroutine());
        }
        
        IEnumerator StartWaveAutoCoroutine()
        {
            yield return new WaitForSeconds(secondsBetweenAutoMoveWaves);
            StartWave();
        }

        public void StartWave()
        {
            waveInProgress = true;
            Game.SpawnManager.SpawnEnemiesWithEnemyCount(CalculateEnemiesToSpawn());
        }

        private int CalculateEnemiesToSpawn()
        {
            return Mathf.CeilToInt((Game.Wave * difficultyMultiplier) + (Game.Location * difficultyMultiplier));
        }

        public void ToggleAutoMove()
        {
            SetAutoMove(!autoMoveWave);
        }
    }
}