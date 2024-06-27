namespace Game.Scripts.Location
{
    using System.Collections.Generic;
    using Game.Scripts.Character;
    using UnityEngine;

    public class Location : MonoBehaviour
    {
        [SerializeField] protected List<Transform> customerSlot;   
        
        protected Queue<Customer> customerQueue = new (); 
        
        public bool IsFullQueue
        {
            get
            {
                if (!gameObject.activeInHierarchy) return false;

                return customerQueue.Count >= customerSlot.Count;
            }
        }
        
        public bool TryGetEmptyQueue(Customer customer, out Transform queue)
        {
            if (IsFullQueue)
            {
                queue = null;
                
                return false;
            }

            customerQueue.Enqueue(customer);

            queue = customerSlot[customerQueue.Count - 1];
                
            return true;
        }
    }
    
}