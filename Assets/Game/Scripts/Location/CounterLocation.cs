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
        
        public void CollectFruit(Fruit fruit)
        {
            if (customerQueue.Count > 0)
            {
                var customer = customerQueue.Peek();
                
                if (customer.ReachLimitSlot)
                {
                    customer.MoveToCheckOut();
                    
                    customerQueue.Dequeue();    
                }
                else
                {
                    customer.CollectFruit(fruit);

                    if (customer.ReachLimitSlot)
                    {
                        customerQueue.Dequeue();
                    }   
                }

                return;
            }

            if (fruitList.Count >= fruitSlotList.Count) return;
         
            Debug.Log("Counter: Place fruit");
            
            fruitList.Add(fruit);
            
            foreach (var slot in fruitSlotList)
            {
                if (slot.childCount == 0)
                {
                    fruit.Collect(slot);
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.isTrigger && other.gameObject.name == "Customer (Clone)")
            {
                if (!other.TryGetComponent<Customer>(out var customer)) return;

                var fruit = ReadyFruit;

                if (fruit == null) return;

                customer.CollectFruit(fruit);

                fruitList.Remove(fruit);
            }  
        }
    }
}
