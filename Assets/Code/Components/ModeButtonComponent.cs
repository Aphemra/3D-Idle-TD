using System;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Components
{
    public class ModeButtonComponent : CoreComponent
    {
        [SerializeField] private Button button;
        [SerializeField] private GameState associatedState;

        private void OnEnable()
        {
            Game.Events.OnModeSwitched += ToggleInteractableState;
            Game.Events.OnCellSelected += ToggleInteractableState;
        }

        private void OnDisable()
        {
            Game.Events.OnModeSwitched -= ToggleInteractableState;
            Game.Events.OnCellSelected -= ToggleInteractableState;
        }

        private void ToggleInteractableState()
        {
            bool interactable;

            if (Game.SelectedCell == null)
            {
                button.interactable = false;
                return;
            }
            
            switch (associatedState)
            {
                case GameState.TowerTierMode when Game.TowerManager.GetSelectedTowers().Count == 3:
                    interactable = true;
                    break;
                case GameState.CellBuyingMode when Game.SelectedCell.GetCellOwned():
                case GameState.TowerBuyingMode when !Game.SelectedCell.GetCellOwned():
                case GameState.TowerBuyingMode when Game.SelectedCell.GetTowerInCell() != null:
                    interactable = false;
                    break;
                case GameState.CellBuyingMode when !Game.SelectedCell.GetCellOwned():
                case GameState.TowerBuyingMode when Game.SelectedCell.GetCellOwned():
                    interactable = true;
                    break;
                default:
                    interactable = false;
                    break;
            }
            button.interactable = interactable;
        }
    }
}