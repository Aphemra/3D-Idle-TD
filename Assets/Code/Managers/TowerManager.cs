using System;
using System.Collections.Generic;
using Code.Components;
using Code.Resources;
using UnityEngine;

namespace Code.Managers
{
    public class TowerManager : CoreManager
    {
        [SerializeField] private List<TowerResource> towerResources;
        [SerializeField] private int defaultGroupTowerTier = 0;
        [SerializeField] private Transform towerParentTransform;

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
        }

        private void PlaceTower(CellComponent cellToPlaceTower)
        {
            TowerResource towerResource = towerResources[defaultGroupTowerTier];
            TowerComponent tower = Instantiate(towerResource.prefab, new Vector3(cellToPlaceTower.GetGridPosition().x, cellToPlaceTower.GetGridPosition().y), Quaternion.identity, towerParentTransform).GetComponent<TowerComponent>();
            tower.gameObject.name = "Tower at (" + cellToPlaceTower.GetGridPosition().x + "," + cellToPlaceTower.GetGridPosition().y + ")";
            
            InitializeTower(tower, towerResource);
        }
    }
}