using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Code.Components;
using UnityEngine;
using Random = UnityEngine.Random;

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
        [SerializeField] private List<EnemySpawnerComponent> enemySpawners;

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
            GenerateEnemySpawners();
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

        public CellComponent SpawnCell(int sizeOfCell, Vector2 spawnPositionOnGrid)
        {
            float calculatedCellScale = Mathf.Pow(2, (sizeOfCell - 1)); // Will find scale value of cell
            float calculatedCellPosition = calculatedCellScale / 2;
            
            var cell = Instantiate(cellPrefab, spawnPositionOnGrid, Quaternion.identity, gridParentContainer).GetComponent<CellComponent>();
            cell.SetGridPosition(spawnPositionOnGrid);
            cell.name = "Cell of Size " + sizeOfCell + " (" + spawnPositionOnGrid.x / sizeOfCell + "," + spawnPositionOnGrid.y / sizeOfCell + ")";
            cell.ReadjustCell(calculatedCellScale, calculatedCellPosition);
            cell.SetCellOwned(true);
            cell.SetSpawnColor();
            gridCells.Add(cell);

            return cell;
        }

        private void GenerateShotArea()
        {
            shotRangeArea.transform.position = new Vector3(gridSize.x / 2, gridSize.y / 2, -1);
            shotRangeArea.transform.localScale = new Vector3(gridSize.x * 1.8f, gridSize.y * 1.8f, shotRangeArea.transform.localScale.z);
        }

        private void GenerateBattleField()
        {
            floorPlane.transform.localScale = new Vector3(gridSize.x * 2f, 1f, gridSize.y * 2f);
        }

        private void GenerateEnemySpawners()
        {
            foreach (var spawner in enemySpawners)
            {
                spawner.gameObject.SetActive(true);
                var trans = spawner.transform;

                switch (spawner.GetLocation())
                {
                    case SpawnerLocation.North:
                        trans.position = new Vector3(gridSize.x / 2, gridSize.y * 2.5f, -1.5f);
                        trans.localScale = new Vector3(gridSize.x * 4 + 1, trans.localScale.y, 3);
                        break;
                    case SpawnerLocation.South:
                        trans.position = new Vector3(gridSize.x / 2, (gridSize.y * -2.5f) + gridSize.y, -1.5f);
                        trans.localScale = new Vector3(gridSize.x * 4 + 1, trans.localScale.y, 3);
                        break;
                    case SpawnerLocation.East:
                        trans.position = new Vector3(gridSize.x * 2.5f, gridSize.y / 2, -1.5f);
                        trans.localScale = new Vector3(trans.localScale.x, gridSize.y * 4 + 1, 3);
                        break;
                    case SpawnerLocation.West:
                        trans.position = new Vector3((gridSize.x * -2.5f) + gridSize.x, gridSize.y / 2, -1.5f);
                        trans.localScale = new Vector3(trans.localScale.x, gridSize.y * 4 + 1, 3);
                        break;
                }
                spawner.GenerateSpawnPoints();
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

        public Vector2 GetGridSize()
        {
            return gridSize;
        }

        public List<CellComponent> GetGridCellsList()
        {
            return gridCells;
        }

        public void DestroyCell(CellComponent cell)
        {
            gridCells.Remove(cell);
            Destroy(cell.gameObject);
        }
        
        #endregion
    }
}
