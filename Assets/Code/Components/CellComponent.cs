using System;
using Code.Managers;
using TMPro;
using UnityEditor.EditorTools;
using UnityEditor.UIElements;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Components
{
    public class CellComponent : MonoBehaviour
    {
        [SerializeField] private bool isOwned;

        [SerializeField] private double cellCost;
        [SerializeField] private Vector2 gridPosition;
        [SerializeField] private TowerComponent towerInCell;

        private Material baseMaterial { get; set; }

        private void OnEnable()
        {
            Game.Events.OnOwnedCellSelected += SelectOwnedCell;
            Game.Events.OnUnownedCellSelected += SelectUnownedCell;
            
            Game.Events.OnOwnedCellSelected += DeselectOwnedCell;
            Game.Events.OnUnownedCellSelected += DeselectUnownedCell;

            Game.Events.OnCellPurchased += SetCellToOwned;
        }

        private void OnDisable()
        {
            Game.Events.OnOwnedCellSelected -= SelectOwnedCell;
            Game.Events.OnUnownedCellSelected -= SelectUnownedCell;
            
            Game.Events.OnOwnedCellSelected -= DeselectOwnedCell;
            Game.Events.OnUnownedCellSelected -= DeselectUnownedCell;

            Game.Events.OnCellPurchased -= SetCellToOwned;
        }

        private void Start()
        {
            // Sets random value to each cell's cost for demonstration
            cellCost = Random.Range(1, 100);;
            
            baseMaterial = GetComponentInChildren<MeshRenderer>().material;
            baseMaterial.color = Game.GridManager.GetUnownedColor();
            Game.HUDManager.SetCellCostLabel(0);
        }

        private void Update()
        {
            if (isOwned && Game.SelectedCell != this)
                baseMaterial.color = Game.GridManager.GetOwnedColor();
        }

        private void OnMouseDown()
        {
            if (Game.SelectedCell == this) return;

            if (towerInCell != null && Game.GameManager.GetTowerTierUpgradeMode() && IsSelected(this))
            {
                DeselectOwnedCell(this);
                Game.HUDManager.SetTowerTierUpgradeButtonInteractable(false);
                return;
            }

            if (towerInCell != null && Game.GameManager.GetTowerTierUpgradeMode() && !IsSelected(this) && Game.TowerManager.GetSelectedTowers().Count + 1 < 4)
            {
                SelectOwnedCell(this);
                
                if (Game.TowerManager.GetSelectedTowers().Count == 4)
                    Game.HUDManager.SetTowerTierUpgradeButtonInteractable(true);
                else
                    Game.HUDManager.SetTowerTierUpgradeButtonInteractable(false);

                return;

            }
            
            if (isOwned)
                Game.Events.OnOwnedCellSelected.Invoke(this);
            else if (!isOwned)
                Game.Events.OnUnownedCellSelected.Invoke(this);
        }

        private bool IsSelected(CellComponent cellToCheck)
        {
            if (Game.TowerManager.GetSelectedTowers().Count == 0) return false;
            
            foreach (var selectedTower in Game.TowerManager.GetSelectedTowers())
            {
                if (selectedTower == cellToCheck.GetTowerInCell()) return true;
            }
            return false;
        }

        private void SelectUnownedCell(CellComponent selectedCell)
        {
            if (selectedCell != this || selectedCell.isOwned) return;
            
            Game.SelectedCell = this;
            baseMaterial.color = GetHighLightColor(baseMaterial.color);
            Game.HUDManager.SetCellCostLabel(cellCost);
        }

        private void DeselectUnownedCell(CellComponent selectedCell)
        {
            if (selectedCell == this || selectedCell.isOwned) return;
            
            baseMaterial.color = Game.GridManager.GetUnownedColor();
        }

        private void SelectOwnedCell(CellComponent selectedCell)
        {
            if (selectedCell != this || !selectedCell.isOwned) return;

            Game.SelectedCell = this;
            baseMaterial.color = GetHighLightColor(baseMaterial.color);
            Game.TowerManager.AddToSelectedTowers(Game.SelectedCell.GetTowerInCell());
            //Game.HUDManager.SetCellCostLabel(cellCost); -- SetTowerCostLabel eventually
        }
        
        private void DeselectOwnedCell(CellComponent selectedCell)
        {
            if (selectedCell == this || !selectedCell.isOwned || Game.SelectedCell == null) return;

            baseMaterial.color = Game.GridManager.GetUnownedColor();
            Game.TowerManager.RemoveFromSelectedTowers(Game.SelectedCell.GetTowerInCell());
        }

        public void SetGridPosition(Vector2 gridPosition)
        {
            this.gridPosition = gridPosition;
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

        private void SetCellToOwned(CellComponent cellToOwn)
        {
            // Subtract cell cost from total money cost (should be a GameManager method call)
            // Set cell isOwned bool to true
            // Set cell material to isOwned material

            if (cellToOwn == null || cellToOwn.isOwned) return;
            
            Game.Cash -= cellToOwn.cellCost;
            Game.HUDManager.SetCashLabelValue(cellToOwn);
            cellToOwn.isOwned = true;
            cellToOwn.baseMaterial.color = Game.GridManager.GetOwnedColor();
        }

        private Color GetHighLightColor(Color colorToHighlight)
        {
            return new Color(colorToHighlight.r + Game.GridManager.GetColorHighlightOffset().x, 
                            colorToHighlight.g + Game.GridManager.GetColorHighlightOffset().y, 
                            colorToHighlight.b + Game.GridManager.GetColorHighlightOffset().z);
        }
    }
}
