namespace Game.Scripts.Location
{
    using System;
    using System.Collections.Generic;
    using Game.Scripts.Character;
    using Game.Scripts.Objects.Tomato;
    using UnityEngine;

    public class CounterLocation : Location
    {
        [SerializeField] private List<Transform> fruitSlotList;
        
        private List<Fruit> fruitList = new();
        
        public bool ReachLimitSlot => fruitList.Count >= fruitSlotList.Count;
        
        private Fruit ReadyFruit
        {
            get
            {
                foreach (var fruit in fruitList)
                {
                    if (fruit != null && fruit.IsReady) return fruit;
                }

                return null;
            }
        }
        
        public bool CollectFruit(Fruit fruit)
        {
            if (customerQueue.Count > 0)
            {
                var customer = customerQueue.Peek();
                
                if(customer.LogicState == CustomerState.Buy)
                {
                    if (customer.ReachLimitSlot)
                    {
                        customer.MoveToCheckOut();
                    
                        OnCustomerMoveOut(customer); 
                        
                        return false;
                    }

                    customer.CollectFruit(fruit);

                    if (customer.ReachLimitSlot)
                    {
                        OnCustomerMoveOut(customer);
                    }

                    return true;
                }
            }

            if (fruitList.Count >= fruitSlotList.Count) return false;

            fruitList.Add(fruit);
            
            foreach (var slot in fruitSlotList)
            {
                if (slot.childCount == 0)
                {
                    fruit.MoveToTarget(slot);
                    
                    Debug.Log("Counter: Place fruit");
                    
                    return true;
                }
            }
            
            fruitList.Remove(fruit);
            
            return false;
        }

        private void OnCustomerMoveOut(Customer customer)
        {
            if(customerQueue.Count == 0) return;
            
            for (var index = 0; index < customerSlot.Count; index++)
            {
                var slot = customerSlot[index];
                        
                if (slot.customer != customer) continue;

                slot.customer = null;

                customerSlot[index] = slot;
            }

            customerQueue.Dequeue();      
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.isTrigger && other.gameObject.name == "Customer(Clone)")
            {
                if (!other.TryGetComponent<Customer>(out var customer)) return;

                var fruit = ReadyFruit;

                if (fruit == null || customer.ReachLimitSlot) return;

                customer.CollectFruit(fruit);

                fruitList.Remove(fruit);
                
                if (customer.ReachLimitSlot)
                {
                    OnCustomerMoveOut(customer);
                }
            }  
        }
    }
}
