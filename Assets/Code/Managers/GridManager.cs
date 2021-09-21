using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Code.Components;
using UnityEngine;

namespace Code.Managers
{
    public class GridManager : CoreManager
    {
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private Color isOwnedColor;
        [SerializeField] private Color isUnownedColor;
        [SerializeField] private Vector3 colorHighlightOffset;

        [SerializeField] private Transform gridParentContainer;
        [SerializeField] private new CinemachineVirtualCamera camera;
        
        [SerializeField] private Vector2 gridSize;

        [SerializeField] private List<Vector2> defaultOwnedCells;
        
        [SerializeField] private List<CellComponent> gridCells;

        private void Awake()
        {
            if (Game.GridManager == null)
                Game.GridManager = this;
        }

        private void Start()
        {
            gridCells = new List<CellComponent>();
            var cameraTransform = camera.transform;
            cameraTransform.position = new Vector3(gridSize.x / 2, gridSize.y / 2, cameraTransform.position.z);
            
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            for (var x = 0; x < gridSize.x; x++)
            {
                for (var y = 0; y < gridSize.y; y++)
                {
                    var cell = Instantiate(cellPrefab, new Vector3(x, y), Quaternion.identity, gridParentContainer).GetComponent<CellComponent>();
                    cell.SetGridPosition(new Vector2(x,y));
                    cell.name = "Cell (" + x + "," + y + ")";

                    foreach (var defaultOwnedCell in defaultOwnedCells)
                    {
                        if (defaultOwnedCell.Equals(cell.GetGridPosition()))
                        {
                            cell.SetCellOwned(true);
                        }
                    }
                    
                    gridCells.Add(cell);
                }
            }
        }

        #region Getters and Setters
        
        public Color GetOwnedColor()
        {
            return isOwnedColor;
        }

        public Color GetUnownedColor()
        {
            return isUnownedColor;
        }

        public Vector3 GetColorHighlightOffset()
        {
            return colorHighlightOffset;
        }
        
        #endregion
    }
}
