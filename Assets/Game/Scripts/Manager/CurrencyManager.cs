namespace Game.Scripts.Manager
{
    using System;
    using Game.Scripts.GameModule;

    public class CurrencyManager : Singleton<CurrencyManager>
    {
        private int amount;

        public int CashAmount => amount;

        public Action<int> OnCashChange;
        
        public void AddCash(int amount)
        {
            this.amount += amount;
            
            OnCashChange.Invoke(this.amount);
        }
    }
}
