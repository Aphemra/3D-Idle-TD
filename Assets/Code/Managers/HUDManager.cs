using System;
using Code.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Managers
{
    public class HUDManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI cellSelectedCost;
        [SerializeField] private TextMeshProUGUI totalCashLabel;
        [SerializeField] private Button buyButton;

        private void OnEnable()
        {
            Game.Events.OnCellSelected += SetBuyButtonState;
        }

        private void OnDisable()
        {
            Game.Events.OnCellSelected -= SetBuyButtonState;
        }

        private void SetBuyButtonState(CellComponent selectedCell)
        {
            buyButton.interactable = true;
        }

        public void SetCashLabelValue(CellComponent selectedCell)
        {
            totalCashLabel.text = FormatTotalCashToString(Game.Cash);
        }

        private void Awake()
        {
            Game.Cash = 10000;
            totalCashLabel.text = FormatTotalCashToString(Game.Cash);
            
            if (Game.HUDManager == null)
                Game.HUDManager = this;
        }

        private void Start()
        {
            cellSelectedCost.text = "";
            buyButton.interactable = false;
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
                cellSelectedCost.text = "";
                return;
            }
            
            cellSelectedCost.text = "Cell Cost: " + FormatCostToString(currencyToFormat);
        }

        public void PurchaseCell()
        {
            Game.Events.OnCellPurchased(Game.SelectedCell);
        }
    }
}
