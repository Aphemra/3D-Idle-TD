using System;
using Code.Components;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Managers
{
    public class HUDManager : CoreManager
    {
        [Title("General Menu Variables")]
        [SerializeField] private TextMeshProUGUI totalCashLabel;
        [SerializeField] private Button changeModeButton;
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

        /*
         *  1. Start on Buy Towers screen
         *  2. Selecting OWNED cell in Buy Tower mode will allow for tower purchases
         *  3. Create buy Cell mode
         *  4. In Buy Cell mode, towers disappear until Buy Cell mode is exited
         */

        private void OnEnable()
        {
            Game.Events.OnUnownedCellSelected += SetBuyCellButtonActive;
            Game.Events.OnOwnedCellSelected += SetBuyTowerButtonActive;
        }

        private void OnDisable()
        {
            Game.Events.OnUnownedCellSelected -= SetBuyCellButtonActive;
            Game.Events.OnOwnedCellSelected -= SetBuyTowerButtonActive;
        }

        private void SetBuyCellButtonActive(CellComponent selectedCell)
        {
            buyTowerButton.interactable = false;
            buyCellButton.interactable = true;
        }
        
        private void SetBuyTowerButtonActive(CellComponent selectedCell)
        {
            buyCellButton.interactable = false;
            buyTowerButton.interactable = true;
        }

        public void SetCashLabelValue(CellComponent selectedCell)
        {
            if (selectedCell == null)
            {
                totalCashLabel.text = FormatTotalCashToString(Game.Cash);
                return;
            }
            
            totalCashLabel.text = FormatTotalCashToString(Game.Cash);
        }

        private void Awake()
        {
            if (Game.HUDManager == null)
                Game.HUDManager = this;
        }

        private void Start()
        {
            cellCostLabel.text = "";
            buyCellButton.interactable = false;
        }

        private string FormatCostToString(double currencyToFormat)
        {
            return "$" + currencyToFormat;
        }
        
        private string FormatTotalCashToString(double currencyToFormat)
        {
            return "Total Cash: $" + currencyToFormat;
        }

        public void SetCellCostLabel(double currencyToFormat)
        {
            if (currencyToFormat == 0)
            {
                cellCostLabel.text = "";
                return;
            }
            
            cellCostLabel.text = "Cell Cost: " + FormatCostToString(currencyToFormat);
        }

        public void PurchaseCell()
        {
            Game.Events.OnCellPurchased.Invoke(Game.SelectedCell);
        }

        public void PurchaseTower()
        {
            Game.Events.OnTowerPurchased.Invoke(Game.SelectedCell);
        }

        public void SwitchModes()
        {
            Game.GameManager.SetBuyCellMode(!Game.GameManager.GetBuyCellMode());
            Game.GameManager.SetBuyTowerMode(!Game.GameManager.GetBuyTowerMode());
            
            buyCellsCanvas.SetActive(!buyCellsCanvas.activeSelf);
            buyTowersCanvas.SetActive(!buyTowersCanvas.activeSelf);
        }
    }
}
