using System;
using System.Linq;
using UnityEngine;

namespace Code.Components
{
    public class CellComponent : MonoBehaviour
    {
        [SerializeField] private bool isOwned;
        [SerializeField] private bool isMultiSelected = false;

        [SerializeField] private double cellCost;
        [SerializeField] private Vector2 gridPosition;
        [SerializeField] private TowerComponent towerInCell;

        [SerializeField] private MeshRenderer meshRenderer;

        private void OnEnable()
        {
            Game.Events.OnCellPurchased += BuyCell;
            Game.Events.OnCellPurchased += DeselectCell;
            Game.Events.OnModeSwitched += ResetCellsOnModeSwitch;
        }

        private void OnDisable()
        {
            Game.Events.OnCellPurchased -= BuyCell;
            Game.Events.OnCellPurchased -= DeselectCell;
            Game.Events.OnModeSwitched -= ResetCellsOnModeSwitch;
        }

        public void SetSpawnColor()
        {
            meshRenderer.material.color = isOwned ? Game.GridManager.GetOwnedColor() : Game.GridManager.GetUnownedColor();
        }
        
        private void OnMouseDown()
        {
            if (Game.GameState == GameState.TowerTierMode)
                MultiCellSelection();
            else
                SingleCellSelection();
        }

        private void SingleCellSelection()
        {
            if (CheckThisCellIsSelected())          // If Cell is selected
            {  
                DeselectCell();                     // Deselect cell
            }
            else if (!CheckThisCellIsSelected())    // If Cell is NOT selected
            {
                SelectCell();                       // Select cell
            }
        }

        private void MultiCellSelection()
        {
            if (towerInCell == null) return;
            
            if (CheckThisCellIsSelected())
            {
                if (!isMultiSelected) return;

                RemoveFromSelectedTowersList();
                DeselectCell();
            }
            else if (!CheckThisCellIsSelected())
            {
                if (isMultiSelected)
                {
                    SelectCell();
                    RemoveFromSelectedTowersList();
                    DeselectCell();
                    return;
                }
                
                if (Game.TowerManager.GetSelectedTowers().Count == 4) return;

                SelectCell();
                AddToSelectedTowersList();
            }
        }

        private void RemoveFromSelectedTowersList()
        {
            Game.TowerManager.RemoveFromSelectedTowers(Game.SelectedCell.GetTowerInCell());
        }

        private void AddToSelectedTowersList()
        {
            Game.TowerManager.AddToSelectedTowers(Game.SelectedCell.GetTowerInCell());
        }

        private bool CheckThisCellIsSelected()
        {
            return Game.SelectedCell == this;
        }

        private void SelectCell()
        {
            if (Game.GameState != GameState.TowerTierMode && Game.SelectedCell != null)
            {
                Game.SelectedCell.DeselectCell();
            }

            if (Game.GameState == GameState.TowerTierMode)
                isMultiSelected = true;
            
            Game.SelectedCell = this;
            HighlightSelectedCell(isOwned);
            Game.Events.OnCellSelected.Invoke();
            Game.Events.OnInfoUpdated.Invoke();
        }

        private void ResetCellsOnModeSwitch()
        {
            if (Game.GameState == GameState.TowerTierMode)
            {
                foreach (var tower in Game.TowerManager.GetSelectedTowers())
                {
                    tower.GetOccupiedCell().isMultiSelected = false;
                }
                Game.TowerManager.GetSelectedTowers().Clear();
            }
            
            UnhighlightSelectedCell(isOwned);
            Game.SelectedCell = null;
            Game.Events.OnInfoUpdated.Invoke();
        }
        
        private void DeselectCell()
        {
            if (Game.GameState == GameState.TowerTierMode)
                isMultiSelected = false;
            
            UnhighlightSelectedCell(isOwned);
            Game.SelectedCell = null;
            Game.Events.OnCellSelected.Invoke();
            Game.Events.OnInfoUpdated.Invoke();
        }

        private void DeselectCell(CellComponent cellComponent) // Used for event subscription only
        {
            if (Game.GameState == GameState.TowerTierMode)
                isMultiSelected = false;
            
            UnhighlightSelectedCell(isOwned);
            Game.SelectedCell = null;
            Game.Events.OnCellSelected.Invoke();
            Game.Events.OnInfoUpdated.Invoke();
        }

        #region Getters and Setters

        public void SetGridPosition(Vector2 gridPos)
        {
            gridPosition = gridPos;
        }

        public Vector2 GetGridPosition()
        {
            return gridPosition;
        }

        public void SetCellOwned(bool owned)
        {
            isOwned = owned;
        }
        
        public bool GetCellOwned()
        {
            return isOwned;
        }

        public void SetTowerInCell(TowerComponent tower)
        {
            towerInCell = tower;
        }

        public TowerComponent GetTowerInCell()
        {
            return towerInCell;
        }

        public void SetCellCost(double cost)
        {
            cellCost = cost;
        }
        public double GetCellCost()
        {
            return cellCost;
        }
        
        #endregion

        #region Already Refactored
        
        private void BuyCell(CellComponent cellToOwn)
        {
            if (cellToOwn == null || cellToOwn.isOwned) return;
            
            Game.GameManager.SubtractValueFromGameCash(cellToOwn.cellCost);
            
            OwnCell(cellToOwn);
        }
        
        private void OwnCell(CellComponent cellToChange)
        {
            cellToChange.SetCellOwned(true);
            cellToChange.SetCellMaterialColor(Game.GridManager.GetOwnedColor());
        }

        public void OwnThisCell()
        {
            SetCellOwned(true);
            SetCellMaterialColor(Game.GridManager.GetOwnedColor());
        }

        private void HighlightSelectedCell(bool owned)
        {
            SetCellMaterialColor(owned ? HighlightColor(Game.GridManager.GetOwnedColor()) : HighlightColor(Game.GridManager.GetUnownedColor()));
        }
        
        private void UnhighlightSelectedCell(bool owned)
        {
            SetCellMaterialColor(owned ? Game.GridManager.GetOwnedColor() : Game.GridManager.GetUnownedColor());
        }

        public void SetCellMaterialColor(Color cellColor)
        {
            meshRenderer.material.color = cellColor;
        }

        private Color HighlightColor(Color colorToHighlight)
        {
            return new Color(colorToHighlight.r + Game.GridManager.GetColorHighlightOffset().x, 
                colorToHighlight.g + Game.GridManager.GetColorHighlightOffset().y, 
                colorToHighlight.b + Game.GridManager.GetColorHighlightOffset().z);
        }
        
        #endregion
        
    }
}
