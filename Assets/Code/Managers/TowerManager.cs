using System;
using System.Collections.Generic;
using System.Linq;
using Code.Components;
using Code.Resources;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Managers
{
    public class TowerManager : CoreManager
    {
        [SerializeField] private List<TowerResource> towerResources;
        [SerializeField] private int defaultGroupTowerTier = 0;
        [SerializeField] private Transform towerParentTransform;
        [SerializeField] private List<TowerComponent> spawnedTowers;

        [SerializeField] private List<TowerComponent> selectedTowers;

        private readonly List<Vector2> neighborDirections = new List<Vector2>
        {
            new Vector2(0, 1),      // North        (  0, +1 )
            new Vector2(1, 1),      // Northeast    ( +1, +1 )
            new Vector2(1, 0),      // East         ( +1,  0 )
            new Vector2(1, -1),     // Southeast    ( +1, -1 )
            new Vector2(0, -1),     // South        (  0, -1 )
            new Vector2(-1, -1),    // Southwest    ( -1, -1 )
            new Vector2(-1, 0),     // West         ( -1,  0 )
            new Vector2(-1, 1),     // Northwest    ( -1, +1 )
        };

        private void OnEnable()
        {
            Game.Events.OnTowerPurchased += PlaceTower;
        }

        private void OnDisable()
        {
            Game.Events.OnTowerPurchased -= PlaceTower;
        }

        private void Awake()
        {
            if (Game.TowerManager == null)
                Game.TowerManager = this;
        }

        private void InitializeTower(TowerComponent tower, TowerResource towerResource)
        {
            tower.InitializeTowerStatistics(towerResource);
            Game.Events.OnTowerPlaced.Invoke(tower);
        }

        private void PlaceTower(CellComponent cellToPlaceTower)
        {
            if (cellToPlaceTower.GetTowerInCell() != null) return;
            
            var towerResource = towerResources[defaultGroupTowerTier];
            var tower = Instantiate(towerResource.prefab, new Vector3(cellToPlaceTower.GetGridPosition().x, cellToPlaceTower.GetGridPosition().y), Quaternion.identity, towerParentTransform).GetComponent<TowerComponent>();
            tower.gameObject.name = "Tower at (" + cellToPlaceTower.GetGridPosition().x + "," + cellToPlaceTower.GetGridPosition().y + ")";
            spawnedTowers.Add(tower);
            
            tower.SetTowerGridPosition(cellToPlaceTower.GetGridPosition());

            InitializeTower(tower, towerResource);

            Game.GameManager.SubtractValueFromGameCash(tower.GetOccupiedCell().GetCellCost()); // Debug
            Game.Events.OnInfoUpdated.Invoke(); // Maybe Debug
        }
        
        public void CanTowersTierUp()
        {
            var truthCount = 0;
            
            if (selectedTowers.Count != 4) return;
            
            foreach (var tower in selectedTowers)
            {
                if (CheckTowerNeighbors(tower)) truthCount++;
            }

            if (truthCount == 4)
            {
                print("TOWERS CAN TIER UP!");
                Game.HUDManager.SetNotificationLabel("Selected towers can tier up!");
            }

            if (truthCount < 4)
            {
                print("TOWERS AREN'T NEIGHBORS!");
                Game.HUDManager.SetNotificationLabel("Selected towers need to be in a 2x2 grid!");
            }
        }
        
        private bool CheckTowerNeighbors(TowerComponent towerToCheck)
        {
            var isTrue = false;
            
            foreach (var selectedTower in selectedTowers)
            {
                if (selectedTower == towerToCheck) continue;

                foreach (var neighbor in towerToCheck.GetNeighbors().GetNeighborsInAllDirections())
                {
                    if (selectedTower != neighbor)
                    {
                        isTrue = false;
                        continue;
                    }
                    isTrue = true;
                    break;
                }
            }
            return isTrue;
        }

        public List<Vector2> GetDirections()
        {
            return neighborDirections;
        }

        public List<TowerComponent> GetActiveTowers()
        {
            return spawnedTowers;
        }

        public List<TowerComponent> GetSelectedTowers()
        {
            return selectedTowers;
        }

        public void AddToSelectedTowers(TowerComponent towerToSelect)
        {
            selectedTowers ??= new List<TowerComponent>();
            
            if (selectedTowers.Any(tower => tower == towerToSelect)) return;
            
            selectedTowers.Add(towerToSelect);
        }

        public void RemoveFromSelectedTowers(TowerComponent towerToRemove)
        {
            if (towerToRemove == null) return;
            
            selectedTowers.Remove(towerToRemove);
        }

        public void UpgradeTowerTier()
        {
            print("Tier upgraded!");
        }
    }
}