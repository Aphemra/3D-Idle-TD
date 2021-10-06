using Code.Resources;
using Code.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Components
{
    public class TowerComponent : CoreComponent
    {
        [Title("Tower Statistics")]
        [SerializeField] private int towerSize;
        [SerializeField] private double cost;
        [SerializeField] private double health;
        [SerializeField] private double damage;
        [SerializeField] private double armor;
        [SerializeField] private double shotSpeed;
        [SerializeField] private double armorPenetration;

        [SerializeField] private CellComponent occupiedCell;
        [SerializeField] private Vector2 gridLocation;

        [SerializeField] private bool isDestroyed;
        
        [SerializeField] private Transform lazerOriginPoint;

        private double maxHealth;

        private Neighbors neighbors;

        private void OnEnable()
        {
            Game.Events.OnTowerPlaced += PopulateNeighbors;
        }

        private void OnDisable()
        {
            Game.Events.OnTowerPlaced -= PopulateNeighbors;
        }

        [Button]
        private void PrintNeighbors()
        {
            print(neighbors.ToString());
        }

        private void Start()
        {
            InitializeNeighbors();
            isDestroyed = false;
            SetTowerCost(Random.Range(25, 100)); // Debug
        }

        private void PopulateNeighbors(TowerComponent newTower)
        {
            newTower.InitializeNeighbors();

            foreach (var activeTower in Game.TowerManager.GetActiveTowers())
            {
                for (var i = 0; i < Game.TowerManager.GetDirections().Count; i++)
                {
                    print("(" + (activeTower.GetTowerGridPosition().x + Game.TowerManager.GetDirections()[i].x * newTower.towerSize) + "," + (activeTower.GetTowerGridPosition().y + Game.TowerManager.GetDirections()[i].y * newTower.towerSize) + ") || (" + newTower.gridLocation.x + "," + newTower.gridLocation.y + ")");
                    if (new Vector2(activeTower.GetTowerGridPosition().x + Game.TowerManager.GetDirections()[i].x * newTower.towerSize, activeTower.GetTowerGridPosition().y + Game.TowerManager.GetDirections()[i].y * newTower.towerSize) == newTower.gridLocation)
                    {
                        if (activeTower.towerSize != newTower.towerSize) continue;
                        
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

        public int GetTowerSize()
        {
            return towerSize;
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
            
            // Eventually multiply these values by location and wave difficulty multiplier
            
            towerSize = towerResource.towerSize;
            cost = towerResource.baseCost;
            health = towerResource.maxHealth;
            damage = towerResource.baseDamage;
            armor = towerResource.baseArmor;
            shotSpeed = towerResource.shotSpeed;
            armorPenetration = towerResource.baseArmorPenetration;

            maxHealth = health;
        }

        public void InflictDamage(double damageToInflict)
        {
            health = (health - damageToInflict) <= 0 ? 0 : health - damageToInflict;
            
            if (health <= 0) DestroyTower();
        }
        
        private void DestroyTower()
        {
            gameObject.SetActive(false);
            Game.TowerManager.MoveTowerFromActiveToInactive(this);
            isDestroyed = true;
        }

        public void ReviveTower()
        {
            gameObject.SetActive(true);
            isDestroyed = false;
        }

        public double GetDamagePerSecondCalculation(double enemyArmor)
        {
            return damage;
        }

        public void HealToMaxHealth()
        {
            health = maxHealth;
        }

        public double GetArmor()
        {
            return armor;
        }

        public double GetHealth()
        {
            return health;
        }

        public bool GetDestructionStatus()
        {
            return isDestroyed;
        }
        
        #region Getters and Setters

        public Transform GetLazerOrigin()
        {
            return lazerOriginPoint;
        }
        
        public void SetTowerGridPosition(Vector2 gridPosition)
        {
            gridLocation = gridPosition;
        }
        
        public Vector2 GetTowerGridPosition()
        {
            return gridLocation;
        }

        public CellComponent GetOccupiedCell()
        {
            return occupiedCell;
        }

        public Neighbors GetNeighbors()
        {
            return neighbors;
        }

        public double GetTowerCost()
        {
            return cost;
        }

        public void SetTowerCost(double towerCost)
        {
            cost = towerCost;
        }
        
        #endregion
    }
}
