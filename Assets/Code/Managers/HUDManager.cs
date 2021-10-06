using System;
using System.Runtime.CompilerServices;
using Code.Components;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Code.Managers
{
    public class HUDManager : CoreManager
    {
        [Title("General Menu Variables")]
        [SerializeField] private TextMeshProUGUI totalCashLabel;
        [SerializeField] private Button changeModeButton;
        [SerializeField] private TextMeshProUGUI notificationLabel;
        [SerializeField] private TextMeshProUGUI waveLabel;
        [SerializeField] private TextMeshProUGUI locationLabel;
        [SerializeField] private Toggle autoToggle;
        [Space]
        
        [Title("Buy Cells Menu Variables")]
        [SerializeField] private GameObject buyCellsCanvas;
        [SerializeField] private TextMeshProUGUI cellCostLabel;
        [SerializeField] private Button buyCellButton;
        [Space]
        
        [Title("Buy Towers Menu Variables")]
        [SerializeField] private GameObject buyTowersCanvas;
        [SerializeField] private TextMeshProUGUI towerCostLabel;
        [SerializeField] private Button buyTowerButton;
        [Space]
        
        [Title("Tower Tier Upgrade Menu Variables")]
        [SerializeField] private GameObject towerTierUpgradeCanvas;
        [SerializeField] private Button towerTierUpgradeButton;

        /*
         *  1. Start on Buy Towers screen
         *  2. Selecting OWNED cell in Buy Tower mode will allow for tower purchases
         *  3. Create buy Cell mode
         *  4. In Buy Cell mode, towers disappear until Buy Cell mode is exited
         */

        private void OnEnable()
        {
            Game.Events.OnInfoUpdated += UpdateAllHUDElements;
            Game.Events.OnCashValueUpdated += SetCashLabelValue;
        }

        private void OnDisable()
        {
            Game.Events.OnInfoUpdated -= UpdateAllHUDElements;
            Game.Events.OnCashValueUpdated -= SetCashLabelValue;
        }

        private void UpdateAllHUDElements()
        {
            // Any HUD update methods get called here.
            SetCashLabelValue();
            SetWaveAndLocationLabel();

            if (Game.SelectedCell == null) return;

            if (!Game.SelectedCell.GetCellOwned())
                SetCellCostLabel(Game.SelectedCell.GetCellCost());
            else
                SetCellCostLabel(0);
            
            if (Game.SelectedCell.GetCellOwned() && Game.SelectedCell.GetTowerInCell() == null)
                SetTowerCostLabel(Game.SelectedCell.GetCellCost());
            else
                SetTowerCostLabel(0);
        }

        private void SetWaveAndLocationLabel()
        {
            locationLabel.text = "Location: " + Game.Location;
            waveLabel.text = "Wave: " + Game.Wave + "/10"; // Second number should also be a variable eventually
        }

        public void SetAutoToggle(bool state)
        {
            autoToggle.isOn = state;
        }
        
        #region Already Refactored

        public void SetCashLabelValue()
        {
            totalCashLabel.text = FormatValueToString(Game.Cash);
        }
        
        private string FormatValueToString(double currencyToFormat)
        {
            // Work this out later
            return "$" + currencyToFormat;
        }
        
        #endregion

        private void Awake()
        {
            if (Game.HUDManager == null)
                Game.HUDManager = this;
        }

        private void Start()
        {
            cellCostLabel.text = "";
            towerCostLabel.text = "";
            buyCellButton.interactable = false;
            buyTowerButton.interactable = false;
        }
        
        public void SetCellCostLabel(double currencyToFormat)
        {
            if (currencyToFormat == 0)
            {
                cellCostLabel.text = "";
                return;
            }
            
            cellCostLabel.text = "Cell Cost: " + FormatValueToString(currencyToFormat);
        }
        
        public void SetTowerCostLabel(double currencyToFormat)
        {
            if (currencyToFormat == 0)
            {
                towerCostLabel.text = "";
                return;
            }
            
            towerCostLabel.text = "Tower Cost: " + FormatValueToString(currencyToFormat);
        }

        public void SetNotificationLabel(string text)
        {
            notificationLabel.text = text;
        }

        public void PurchaseCell()
        {
            Game.Events.OnCellPurchased.Invoke(Game.SelectedCell);
        }

        public void PurchaseTower(int towerSizeIndex)
        {
            Game.Events.OnTowerPurchased.Invoke(Game.SelectedCell, towerSizeIndex);
        }

        public void SwitchModes(int newMode)
        {
            switch (newMode)
            {
                case 0:
                    Game.GameManager.SetBuyCellMode(true);
                    Game.GameManager.SetBuyTowerMode(false);
                    Game.GameManager.SetTowerTierUpgradeMode(false);
                    buyCellsCanvas.SetActive(true);
                    buyTowersCanvas.SetActive(false);
                    towerTierUpgradeCanvas.SetActive(false);
                    notificationLabel.text = "";
                    Game.GameManager.ChangeState(GameState.CellBuyingMode);
                    Game.Events.OnModeSwitched.Invoke();
                    break;
                case 1:
                    Game.GameManager.SetBuyCellMode(false);
                    Game.GameManager.SetBuyTowerMode(true);
                    Game.GameManager.SetTowerTierUpgradeMode(false);
                    buyCellsCanvas.SetActive(false);
                    buyTowersCanvas.SetActive(true);
                    towerTierUpgradeCanvas.SetActive(false);
                    notificationLabel.text = "";
                    Game.GameManager.ChangeState(GameState.TowerBuyingMode);
                    Game.Events.OnModeSwitched.Invoke();
                    break;
                case 2:
                    Game.GameManager.SetBuyCellMode(false);
                    Game.GameManager.SetBuyTowerMode(false);
                    Game.GameManager.SetTowerTierUpgradeMode(true);
                    buyCellsCanvas.SetActive(false);
                    buyTowersCanvas.SetActive(false);
                    towerTierUpgradeCanvas.SetActive(true);
                    notificationLabel.text = "";
                    Game.GameManager.ChangeState(GameState.TowerTierMode);
                    Game.Events.OnModeSwitched.Invoke();
                    break;
            }
            
            Game.SelectedCell = null;
        }
    }
}
