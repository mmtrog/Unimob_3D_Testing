using UnityEngine;

namespace Game.Scripts.Currency
{
    using DG.Tweening;
    using Game.Scripts.Manager;
    using TMPro;

    public class CashDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI cashAmountTMP;

        private int amount;
        
        private void Awake()
        {
            CurrencyManager.Instance.OnCashChange += UpdateAmount;
        }

        private void UpdateAmount(int amount)
        {
            var current = this.amount;
            
            this.amount = amount;

            DOVirtual.Int(current, amount, 0.3f, value =>
            {
                cashAmountTMP.text = value < 10 ? $"{value:0}" : $"{value:00}";
            });
        }
    }
}
