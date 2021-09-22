using System;
using Cinemachine;
using UnityEngine;

namespace Code.Managers
{
    public class GameManager : CoreManager
    {
        [SerializeField] private double startingCash;
        [SerializeField] private bool inBuyTowerMode;
        [SerializeField] private bool inBuyCellMode;
        [SerializeField] private float scrollScale;
        [SerializeField] private Vector2 minMaxFOVZoom;
        
        [SerializeField] private CinemachineVirtualCamera topDownCamera;
        [SerializeField] private CinemachineVirtualCamera threeDimensionalCamera;
        
        private void Awake()
        {
            Game.Cash = startingCash;
            
            if (Game.GameManager == null)
                Game.GameManager = this;
        }

        private void Start()
        {
            Game.HUDManager.SetCashLabelValue(Game.SelectedCell);
        }

        private void Update()
        {
            if (Input.mouseScrollDelta.y != 0)
                topDownCamera.m_Lens.FieldOfView = Mathf.Clamp(topDownCamera.m_Lens.FieldOfView - (Input.mouseScrollDelta.y * scrollScale), minMaxFOVZoom.x, minMaxFOVZoom.y);
        }

        public void SwitchCameras()
        {
            if (topDownCamera.Priority > threeDimensionalCamera.Priority)
                threeDimensionalCamera.Priority += 10;
            else if (topDownCamera.Priority < threeDimensionalCamera.Priority)
                threeDimensionalCamera.Priority -= 10;
        }

        #region Getters and Setters

        public bool GetBuyTowerMode()
        {
            return inBuyTowerMode;
        }
        
        public bool GetBuyCellMode()
        {
            return inBuyCellMode;
        }
        
        public void SetBuyTowerMode(bool modeState)
        {
            inBuyTowerMode = modeState;
        }
        
        public void SetBuyCellMode(bool modeState)
        {
            inBuyCellMode = modeState;
        }
        
        #endregion
    }
}
