using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Code.Resources;
using Code.Utilities;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;

namespace Code.Components
{
    public class TowerComponent : CoreComponent
    {
        [Title("Tower Statistics")]
        [SerializeField] private int groupTowerTier;
        [SerializeField] private double cost;
        [SerializeField] private double health;
        [SerializeField] private double damage;
        [SerializeField] private double armor;
        [SerializeField] private double shotSpeed;
        [SerializeField] private double armorPenetration;

        [SerializeField] private CellComponent occupiedCell;
        [SerializeField] private Vector2 gridLocation;
        [SerializeField] private GameObject currentTarget;

        private float shotCounter = 0;

        [SerializeField] private Neighbors neighbors;

        [SerializeField] private TowerComponent potentialNeighbor;

        private void OnEnable()
        {
            Game.Events.OnEnemyEnteringBattlefield += GetTarget;

            Game.Events.OnTowerPlaced += SetNeighbors;
        }

        private void OnDisable()
        {
            Game.Events.OnEnemyEnteringBattlefield -= GetTarget;
            
            Game.Events.OnTowerPlaced -= SetNeighbors;
        }

        private void SetNeighbors(TowerComponent newTower)
        {
            newTower.InitializeNeighbors();
            
            foreach (var activeTower in Game.TowerManager.GetActiveTowers())
            {
                for (var i = 0; i < Game.TowerManager.GetDirections().Count; i++)
                {
                    if (new Vector2(activeTower.GetTowerGridPosition().x + Game.TowerManager.GetDirections()[i].x, activeTower.GetTowerGridPosition().y + Game.TowerManager.GetDirections()[i].y) == newTower.gridLocation)
                    {
                        switch (i)
                        {
                            case 0:
                                activeTower.neighbors.neighborN = newTower;
                                newTower.neighbors.neighborS = activeTower;
                                break;
                            case 1:
                                activeTower.neighbors.neighborNE = newTower;
                                newTower.neighbors.neighborSW = activeTower;
                                break;
                            case 2:
                                activeTower.neighbors.neighborE = newTower;
                                newTower.neighbors.neighborW = activeTower;
                                break;
                            case 3:
                                activeTower.neighbors.neighborSE = newTower;
                                newTower.neighbors.neighborNW = activeTower;
                                break;
                            case 4:
                                activeTower.neighbors.neighborS = newTower;
                                newTower.neighbors.neighborN = activeTower;
                                break;
                            case 5:
                                activeTower.neighbors.neighborSW = newTower;
                                newTower.neighbors.neighborNE = activeTower;
                                break;
                            case 6:
                                activeTower.neighbors.neighborW = newTower;
                                newTower.neighbors.neighborE = activeTower;
                                break;
                            case 7:
                                activeTower.neighbors.neighborNW = newTower;
                                newTower.neighbors.neighborSE = activeTower;
                                break;
                        }
                    }
                }
            }
        }

        private void Start()
        {
            InitializeNeighbors();
        }

        private void InitializeNeighbors()
        {
            if (neighbors != null) return;
            
            neighbors = new Neighbors();
        }

        public void InitializeTowerStatistics(TowerResource towerResource)
        {
            if (Game.SelectedCell != null)
                occupiedCell = Game.SelectedCell;
            
            occupiedCell.SetTowerInCell(this);
            
            groupTowerTier = towerResource.groupTowerTier;
            cost = towerResource.baseCost;
            health = towerResource.maxHealth;
            damage = towerResource.baseDamage;
            armor = towerResource.baseArmor;
            shotSpeed = towerResource.shotSpeed;
            armorPenetration = towerResource.baseArmorPenetration;
        }

        private void Update()
        {
            Attack();
        }

        private void GetTarget(GameObject target)
        {
            if (currentTarget == null)
                currentTarget = target;
            
            if (Vector3.Distance(transform.position, currentTarget.transform.position) >
                Vector3.Distance(transform.position, target.transform.position))
            {
                currentTarget = target;
            }
        }

        private void Attack()
        {
            if (currentTarget == null) return;
            
            if (shotCounter < shotSpeed)
            {
                //print("Not Shooting");
                shotCounter += Time.deltaTime;
                return;
            }

            if (shotCounter >= shotSpeed)
            {
                print("Shooting");
                shotCounter = 0;
                ShootEnemy();
            }
        }

        private void ShootEnemy()
        {
            print(name + " is shooting enemy named " + currentTarget.name);
            // Draw Raycast
            // Call Damage method on enemy script when raycast hits
        }

        public void SetTowerGridPosition(Vector2 gridPosition)
        {
            gridLocation = gridPosition;
        }
        
        public Vector2 GetTowerGridPosition()
        {
            return gridLocation;
        }

        public bool IsNeighborsWith(List<TowerComponent> towersToCheck)
        {
            foreach (var neighbor in neighbors.GetNeighborsInAllDirections())
            {
                foreach (var tower in towersToCheck)
                {
                    print("Comparing " + tower.name + " and " + neighbor.name);
                    if (tower != neighbor) return false;
                }
            }
            return true;
        }

        public Neighbors GetNeighbors()
        {
            return neighbors;
        }
    }
}
