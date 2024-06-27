namespace Game.Scripts.Location
{
    using System;
    using System.Collections.Generic;
    using Game.Scripts.Character;
    using UnityEngine;

    public class Location : MonoBehaviour
    {
        [SerializeField] protected List<QueueSlot> customerSlot = new();   
        
        protected Queue<Customer> customerQueue = new (); 
        
        public bool IsFullQueue
        {
            get
            {
                if (!gameObject.activeInHierarchy) return true;

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

            for (var index = 0; index < customerSlot.Count; index++)
            {
                var slot = customerSlot[index];
                
                if (slot.customer != null) continue;

                customerQueue.Enqueue(customer);
                
                queue = slot.transform;

                slot.customer = customer;

                customerSlot[index] = slot;
                
                return true;
            }
            
            queue = null;
                
            return false;
        }
    }

    [Serializable]
    public struct QueueSlot
    {
        public Transform transform;

        public Customer customer;
    }
}