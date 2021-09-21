using Code.Components;
using Code.Managers;
using UnityEngine.Events;

namespace Code
{
    public class GameEvents
    {
        public UnityAction<CellComponent> OnUnownedCellSelected;
        public UnityAction<CellComponent> OnOwnedCellSelected;

        public UnityAction<CellComponent> OnCellPurchased;

        public UnityAction<CellComponent> OnTowerPurchased;
    }

    public static class Game
    {
        public static CellComponent SelectedCell;
        public static double Cash;
        
        // Manager Instances
        public static GridManager GridManager { get; set; }
        public static HUDManager HUDManager { get; set; }
        public static GameManager GameManager { get; set; }
        public static TowerManager TowerManager { get; set; }

        public static readonly GameEvents Events = new GameEvents();
    }
}