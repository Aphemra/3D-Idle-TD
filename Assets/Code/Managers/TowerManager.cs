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
        [SerializeField] private Transform towerParentTransform;
        [SerializeField] private List<TowerComponent> activeTowers;
        [SerializeField] private List<TowerComponent> inactiveTowers;

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

        private void PlaceTower(CellComponent cellToPlaceTower, int towerSizeIndex)
        {
            if (cellToPlaceTower.GetTowerInCell() != null) return;
            
            var towerResource = towerResources[towerSizeIndex - 1];
            var tower = Instantiate(towerResource.prefab, new Vector3(cellToPlaceTower.GetGridPosition().x, cellToPlaceTower.GetGridPosition().y), Quaternion.identity, towerParentTransform).GetComponent<TowerComponent>();
            activeTowers.Add(tower);
            
            tower.SetTowerGridPosition(cellToPlaceTower.GetGridPosition());

            InitializeTower(tower, towerResource);
            tower.gameObject.name = "Tower of size " + tower.GetTowerSize() + " at (" + cellToPlaceTower.GetGridPosition().x / tower.GetTowerSize() + "," + cellToPlaceTower.GetGridPosition().y / tower.GetTowerSize() + ")";

            Game.GameManager.SubtractValueFromGameCash(tower.GetOccupiedCell().GetCellCost()); // Debug
            Game.Events.OnInfoUpdated.Invoke(); // Maybe Debug
        }
        
        public void CanTowersBeCombined()
        {
            var truthCount = 0;
            
            if (selectedTowers.Count != 4) return;
            
            foreach (var tower in selectedTowers)
            {
                if (CheckTowerNeighbors(tower)) truthCount++;
            }

            if (truthCount == 4)
            {
                CombineTowers();
                Game.HUDManager.SetNotificationLabel("Towers Combined!");
            }

            if (truthCount < 4)
            {
                Game.HUDManager.SetNotificationLabel("Selected towers need to be in a 2x2 grid!");
            }
        }

        private void CombineTowers()
        {
            Vector2 spawnGridLocation = GetCombinedTowerSpawnGridLocation();

            foreach (var tower in selectedTowers)
            {
                Game.GridManager.DestroyCell(tower.GetOccupiedCell());
                DestroyTower(tower);
            }

            int newCellAndTowerSize = selectedTowers[0].GetTowerSize() + 1;
            var cell = Game.GridManager.SpawnCell(newCellAndTowerSize, spawnGridLocation);

            Game.SelectedCell = cell;
            PlaceTower(cell, newCellAndTowerSize);
            Game.SelectedCell = null;
            
            selectedTowers.Clear();
        }

        private Vector2 GetCombinedTowerSpawnGridLocation()
        {
            Vector2 newGridLocation = new Vector2(10000,10000);

            foreach (var tower in selectedTowers)
            {
                if (tower.GetTowerGridPosition().x < newGridLocation.x ||
                    tower.GetTowerGridPosition().y < newGridLocation.y)
                    newGridLocation = tower.GetTowerGridPosition();
            }

            return newGridLocation;
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
            return activeTowers;
        }
        
        public List<TowerComponent> GetInactiveTowers()
        {
            return inactiveTowers;
        }
        
        public void DestroyTower(TowerComponent tower)
        {
            activeTowers.Remove(tower);
            Destroy(tower.gameObject);
        }

        public void MoveTowerFromActiveToInactive(TowerComponent towerToMove)
        {
            activeTowers.Remove(towerToMove);
            inactiveTowers.Add(towerToMove);
        }
        
        public void MoveTowerFromInactiveToActive(TowerComponent towerToMove)
        {
            inactiveTowers.Remove(towerToMove);
            activeTowers.Add(towerToMove);
        }

        public void MoveAllInactiveTowersToActive()
        {
            activeTowers.AddRange(inactiveTowers);
            inactiveTowers.Clear();
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