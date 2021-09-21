using System;
using Code.Components;
using UnityEngine;

namespace Code.Managers
{
    public class TowerManager : CoreManager
    {
        [SerializeField] private GameObject testTowerPrefab;
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

        private void PlaceTower(CellComponent cellToPlaceTower)
        {
            Instantiate(testTowerPrefab,
                new Vector3(cellToPlaceTower.GetGridPosition().x, cellToPlaceTower.GetGridPosition().y), 
                Quaternion.identity, 
                towerParentTransform);
        }
    }
}