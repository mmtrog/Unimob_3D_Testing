using UnityEngine;

namespace Game.Scripts.Currency
{
    using System;
    using DG.Tweening;
    using Game.Scripts.Manager;
    using TMPro;

    public class CashDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI cashAmountTMP;

        private int currentAmount;
        
        private void Awake()
        {
            CurrencyManager.Instance.OnCashChange += UpdateAmount;
        }

        private void UpdateAmount(int amount)
        {
            DOVirtual.Int(currentAmount, amount, 0.3f, value =>
            {
                cashAmountTMP.text = currentAmount < 10 ? $"{currentAmount:0}" : $"{currentAmount:00}";
            }).OnComplete(() =>
            {
                currentAmount = amount;
            });
        }

        private void OnDestroy()
        {
            CurrencyManager.Instance.OnCashChange -= UpdateAmount; 
        }
    }
}
