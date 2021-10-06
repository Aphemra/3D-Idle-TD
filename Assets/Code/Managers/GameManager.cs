using System;
using Cinemachine;
using Code.Components;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Managers
{
    public class GameManager : CoreManager
    {
        [SerializeField] private double startingCash;
        [SerializeField] private bool inBuyTowerMode;
        [SerializeField] private bool inBuyCellMode;
        [SerializeField] private bool inTowerTierUpgradeMode;

        [Title("Game.cs Debug Variable Values")]
        public GameState gameState;
        public double currentCash;
        public CellComponent selectedCell;
        public int location;
        public int wave;
        [Space] [Title("Forced Values")]
        public bool forceState;
        public GameState debugState;
        [Space] [Title("Toggle Values")]
        public bool canExplode;
        
        private void Awake()
        {
            if (Game.GameManager == null)
                Game.GameManager = this;
            
            InitializeGame();
            ChangeState(GameState.GridGeneration);
        }
        
        private void Start()
        {
            Game.Events.OnInfoUpdated.Invoke();
        }

        private void Update()
        {
            DebugGameVariables();
        }

        private void DebugGameVariables()
        {
            if (forceState)
                ChangeState(debugState);
            
            gameState = Game.GameState;
            currentCash = Game.Cash;
            selectedCell = Game.SelectedCell;
            location = Game.Location;
            wave = Game.Wave;
        }

        private void InitializeGame()
        {
            Game.Cash = startingCash;
        }
        
        #region Getters and Setters And Helper Methods

        public bool SetGameCash(double newCash)
        {
            if (newCash < 0) return false;
            
            Game.Cash = newCash;
            Game.Events.OnCashValueUpdated.Invoke();

            return true;
        }

        public void AddValueToGameCash(double valueToAdd)
        {
            Game.Cash += valueToAdd;
            Game.Events.OnCashValueUpdated.Invoke();
        }

        public bool SubtractValueFromGameCash(double valueToSubtract)
        {
            if ((Game.Cash - valueToSubtract) < 0) return false;

            Game.Cash -= valueToSubtract;
            Game.Events.OnCashValueUpdated.Invoke();

            return true;
        }
        
        public bool GetBuyTowerMode()
        {
            return inBuyTowerMode;
        }
        
        public bool GetBuyCellMode()
        {
            return inBuyCellMode;
        }
        
        public bool GetTowerTierUpgradeMode()
        {
            return inTowerTierUpgradeMode;
        }
        
        public void SetBuyTowerMode(bool modeState)
        {
            inBuyTowerMode = modeState;
        }
        
        public void SetBuyCellMode(bool modeState)
        {
            inBuyCellMode = modeState;
        }
        
        public void SetTowerTierUpgradeMode(bool modeState)
        {
            inTowerTierUpgradeMode = modeState;
        }

        #endregion

        public void ChangeState(GameState gameState)
        {
            Game.GameState = gameState;
        }
    }
}
