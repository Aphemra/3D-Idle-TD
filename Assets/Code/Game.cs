using Code.Components;
using Code.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace Code
{
    public enum GameState
    {
        GridGeneration,
        TowerBuyingMode,
        CellBuyingMode,
        TowerTierMode,
        TowerUpgradingMode,
        CombatMode,
        RebirthMode
    }
    
    public class GameEvents
    {
        public UnityAction OnGridGenerated;     // Event fires when Grid is finished generating
        public UnityAction OnCellUpdated;       // Event fires when a cell state is updated
        public UnityAction OnTowerBought;       // Event fires when a tower is bought
        public UnityAction OnTowerAdded;        // Event fires when a tower is placed down
        public UnityAction OnTowerUpdated;      // Event fires when a tower's statistics are updated
        public UnityAction OnTowerDestroyed;    // Event fires when a tower is destroyed
        public UnityAction OnEnemySpawn;        // Event fires when an enemy spawns
        public UnityAction OnEnemyDestroyed;    // Event fires when an enemy is destroyed
        public UnityAction OnEnemyEntersRange;  // Event fires when an enemy enters tower shooting range
        public UnityAction OnWaveWon;           // Event fires when a wave is completed
        public UnityAction OnWaveLost;          // Event fires when a wave is lost
        public UnityAction OnLocationWon;       // Event fires when a location is completed
        public UnityAction OnBossWaveEntered;   // Event fires when a boss wave is started
        public UnityAction OnBossDestroyed;     // Event fires when a boss wave is completed
        
        public UnityAction OnModeSwitched;      // Event fires when the boy mode is switched
        
        public UnityAction OnInfoUpdated;       // Event fires when any piece of information is updated
        public UnityAction OnCashValueUpdated;  // Event fires when cash value is updated
        
        
        public UnityAction OnCellSelected;

        public UnityAction<CellComponent> OnCellPurchased;

        public UnityAction<CellComponent> OnTowerPurchased;

        public UnityAction<TowerComponent> OnTowerPlaced;

        public UnityAction<EnemyComponent> OnEnemyEnteringBattlefield; // Probably switch to EnemyComponent and Transform, maybe
    }

    public static class Game
    {
        public static GameState GameState;
        public static CellComponent SelectedCell;
        public static double Cash;
        public static int Wave;
        public static int Location;
        
        // Manager Instances
        public static GridManager GridManager { get; set; }
        public static HUDManager HUDManager { get; set; }
        public static GameManager GameManager { get; set; }
        public static TowerManager TowerManager { get; set; }
        public static ControlsManager ControlsManager { get; set; }
        public static EnemyManager EnemyManager { get; set; }
        public static SpawnManager SpawnManager { get; set; }

        public static readonly GameEvents Events = new GameEvents();
    }
}