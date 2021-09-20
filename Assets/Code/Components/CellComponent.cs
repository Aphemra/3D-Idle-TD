using System;
using Code.Managers;
using TMPro;
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

        private MeshRenderer meshRenderer { get; set; }
        private Material baseMaterial { get; set; }

        private void OnEnable()
        {
            Game.Events.OnCellSelected += SelectCell;
            Game.Events.OnCellSelected += DeselectCell;

            Game.Events.OnCellPurchased += SetCellToOwned;
        }

        private void OnDisable()
        {
            Game.Events.OnCellSelected -= SelectCell;
            Game.Events.OnCellSelected -= DeselectCell;

            Game.Events.OnCellPurchased -= SetCellToOwned;
        }

        private void Start()
        {
            // Sets random value to each cell's cost for demonstration
            cellCost = Random.Range(1, 100);;
            
            meshRenderer = GetComponentInChildren<MeshRenderer>();
            baseMaterial = meshRenderer.material;
            isOwned = false;
            Game.HUDManager.SetCellCostLabel(0);
        }

        private void Update()
        {
            if (isOwned)
                meshRenderer.material = Game.GridManager.GetIsOwnedMaterial();
        }

        private void OnMouseDown()
        {
            if (isOwned) return;
            
            Game.Events.OnCellSelected.Invoke(this);
        }

        private void SelectCell(CellComponent selectedCell)
        {
            if (selectedCell != this || selectedCell.isOwned) return;
            
            Game.SelectedCell = this;
            meshRenderer.material = Game.GridManager.GetHighlightMaterial();
            Game.HUDManager.SetCellCostLabel(cellCost);
        }

        private void DeselectCell(CellComponent selectedCell)
        {
            if (selectedCell == this) return;

            meshRenderer.material = baseMaterial;
        }

        public void SetGridPosition(Vector2 gridPosition)
        {
            this.gridPosition = gridPosition;
        }

        private void SetCellToOwned(CellComponent cellToOwn)
        {
            // Subtract cell cost from total money cost (should be a GameManager method call)
            // Set cell isOwned bool to true
            // Set cell material to isOwned material

            if (cellToOwn.isOwned) return;
            
            Game.Cash -= cellToOwn.cellCost;
            Game.HUDManager.SetCashLabelValue(cellToOwn);
            cellToOwn.isOwned = true;
            cellToOwn.meshRenderer.material = Game.GridManager.GetIsOwnedMaterial();
        }
    }
}
