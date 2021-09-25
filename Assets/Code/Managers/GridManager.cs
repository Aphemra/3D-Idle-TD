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
        
        [SerializeField] private GameObject shotRangeArea;
        [SerializeField] private GameObject floorPlane;

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
            GenerateShotArea();
            GenerateBattleField();
            Game.GameManager.ChangeState(GameState.TowerBuyingMode);
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
                    
                    cell.SetCellCost(Random.Range(1, 100)); // Debug

                    foreach (var defaultOwnedCell in defaultOwnedCells)
                    {
                        if (defaultOwnedCell.Equals(cell.GetGridPosition()))
                        {
                            cell.SetCellOwned(true);
                        }
                    }
                    cell.SetSpawnColor();
                    gridCells.Add(cell);
                }
            }
        }

        private void GenerateShotArea()
        {
            shotRangeArea.transform.position = new Vector3(gridSize.x / 2, gridSize.y / 2, -1);
            shotRangeArea.transform.localScale = new Vector3(gridSize.x * 1.5f, gridSize.y * 1.5f, shotRangeArea.transform.localScale.z);
        }

        private void GenerateBattleField()
        {
            floorPlane.transform.localScale = new Vector3(gridSize.x * 2f, 1f, gridSize.y * 2f);
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

        public Vector2 GetGridSize()
        {
            return gridSize;
        }
        
        #endregion
    }
}
