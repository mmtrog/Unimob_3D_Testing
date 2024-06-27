using UnityEngine;

namespace Game.Scripts.Manager
{
    using Game.Scripts.Character;
    using Game.Scripts.GameModule;
    using Game.Scripts.Location;

    public class CheckoutManager : Singleton<CheckoutManager>
    {
        [SerializeField] 
        private CheckoutLocation checkout;
        
        public bool IsFullQueue => checkout.IsFullQueue;
        
        public bool GetQueue(Customer customer, out Transform queue)
        {
            return checkout.TryGetEmptyQueue(customer, out queue);
        }
    }
}
