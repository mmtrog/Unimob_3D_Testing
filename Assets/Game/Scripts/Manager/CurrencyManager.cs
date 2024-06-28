namespace Game.Scripts.Manager
{
    using System;
    using Game.Scripts.GameModule;

    public class CurrencyManager : Singleton<CurrencyManager>
    {
        private int amount;

        public Action<int> OnCashChange;

        public bool TryUseCash(int subtract)
        {
            if (subtract <= amount)
            {
                amount -= subtract;
                
                OnCashChange.Invoke(amount);
                
                return true;
            }

            return false;
        }
        
        public void AddCash(int increase)
        {
            amount += increase;
            
            OnCashChange.Invoke(amount);
        }
    }
}
