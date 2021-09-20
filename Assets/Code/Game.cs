using Code.Components;
using Code.Managers;
using UnityEngine.Events;

namespace Code
{
    public class GameEvents
    {
        public UnityAction<CellComponent> OnCellSelected;

        public UnityAction<CellComponent> OnCellPurchased;
    }

    public static class Game
    {
        public static CellComponent SelectedCell;
        public static double Cash;
        
        public static GridManager GridManager { get; set; }
        public static HUDManager HUDManager { get; set; }

        public static readonly GameEvents Events = new GameEvents();
    }
}