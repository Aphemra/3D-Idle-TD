using System;
using System.Collections.Generic;
using Cinemachine;
using Code.Components;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Managers
{
    public class GridManager : SerializedMonoBehaviour
    {
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private Material highlightMaterial;
        [SerializeField] private Material isOwnedMaterial;

        [SerializeField] private Transform gridParentContainer;
        [SerializeField] private new CinemachineVirtualCamera camera;
        
        [SerializeField] private Vector2 gridSize;
    
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
            for (var x = 0; x < gridSize.x; x++)
            {
                for (var y = 0; y < gridSize.y; y++)
                {
                    var cell = Instantiate(cellPrefab, new Vector3(x, y), Quaternion.identity, gridParentContainer).GetComponent<CellComponent>();
                    cell.SetGridPosition(new Vector2(x,y));
                    cell.name = "Cell (" + x + "," + y + ")";
                    gridCells.Add(cell);
                }
            }
        }

        public Material GetHighlightMaterial()
        {
            return highlightMaterial;
        }

        public Material GetIsOwnedMaterial()
        {
            return isOwnedMaterial;
        }
    }
}
