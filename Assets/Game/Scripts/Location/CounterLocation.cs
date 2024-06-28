namespace Game.Scripts.Location
{
    using System;
    using System.Collections.Generic;
    using DG.Tweening;
    using Game.Scripts.Character;
    using Game.Scripts.Manager;
    using Game.Scripts.Objects.Tomato;
    using UnityEngine;

    public class CounterLocation : Location
    {
        [SerializeField] private Transform groundTrans;
        
        [SerializeField] private List<Transform> fruitSlotList;
        
        private List<Fruit> fruitList = new();
        
        private bool playerIn;

        private bool isChecking;
        
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
            if (isChecking) return false;

            isChecking = true;
            
            if (customerQueue.Count > 0)
            {
                var customer = customerQueue.Peek();
                
                if(customer.LogicState == CustomerState.Buy && !customer.ReachLimitSlot)
                {
                    customer.CollectFruit(fruit);

                    isChecking = false;
                    
                    return true;
                }
            }

            if (fruitList.Count >= fruitSlotList.Count)
            {
                isChecking = false;
                
                return false;
            }

            fruitList.Add(fruit);
            
            foreach (var slot in fruitSlotList)
            {
                if (slot.childCount == 0)
                {
                    fruit.MoveToTarget(slot);
                    
                    isChecking = false;
                    
                    return true;
                }
            }
            
            fruitList.Remove(fruit);
            
            isChecking = false;
            
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

        private void CheckQueue()
        {
            if(customerQueue.Count == 0) return;
            
            var customer = customerQueue.Peek();

            if (!customer.ReachLimitSlot || CheckoutManager.Instance.IsFullQueue) return;
            
            customer.MoveToCheckOut();
                        
            OnCustomerMoveOut(customer);
        }

        private void Update()
        {
            CheckQueue();         
        }
        
        private void OnTriggerExit(Collider other)
        {
            if(!playerIn) return;
            
            if (other.gameObject.name != "Player") return;

            if (playerIn)
            {
                groundTrans.DOKill();
                
                groundTrans.DOScale(new Vector3(3f, 0.1f, 2f), 0.3f).SetEase(Ease.OutBack);
                
                playerIn = false;
            }   
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.isTrigger && other.gameObject.name == "Player")
            {
                if (!playerIn)
                {
                    groundTrans.DOKill();
                
                    groundTrans.DOScale(new Vector3(4f, 0.1f, 3f), 0.3f).SetEase(Ease.OutBack);
                
                    playerIn = true;
                }
            }
            
            if(isChecking) return;
            
            if (!other.isTrigger || other.gameObject.name != "Customer(Clone)") return;
            
            isChecking = true;

            if (!other.TryGetComponent<Customer>(out var customer) || customer.ReachLimitSlot)
            {
                isChecking = false;
                
                return;
            }
                
            var fruit = ReadyFruit;

            if (fruit == null)
            {
                isChecking = false;
                
                return;
            }

            customer.CollectFruit(fruit);

            fruitList.Remove(fruit);
            
            isChecking = false;
        }
    }
}
