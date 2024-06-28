namespace Game.Scripts.Location
{
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using Game.Scripts.Character;
    using Game.Scripts.GameModule;
    using Game.Scripts.Objects;
    using UnityEngine;

    public class CheckoutLocation : Location
    {
        [SerializeField] private Transform groundTrans;
        
        private bool playerIn;
        
        private Box box;

        private Customer customer;

        private Stack<Cash> cashStack = new();

        public bool Processing { set; get; }
        
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
            if (!other.isTrigger || other.gameObject.name != "Player") return;
            
            if (!playerIn)
            {
                groundTrans.DOKill();
                
                groundTrans.DOScale(new Vector3(4f, 0.1f, 2.5f), 0.3f).SetEase(Ease.OutBack);
                
                playerIn = true;
            }

            if (cashStack.Count > 0)
            {
                if (cashStack.Peek().IsReady)
                {
                    cashStack.Pop().MoveToTarget(other.transform);   
                }
            }
            
            if (customerQueue.Count <= 0) return;

            customer = customerQueue.Peek();
            
            //if(customer.LogicState != CustomerState.CheckOut) return;
            
            if (Processing)
            {
                if(box == null) return;
                
                var fruit = customer.ReadyFruit;

                if (fruit != null)
                {
                    customer.RemoveFruit(fruit);
                    
                    box.CollectFruit(fruit);
                }
                else
                {
                    box.Boxed();
                }
            }
            else
            {
                Processing = true;

                box = PoolManager.Instance.Spawn(Entity.Box).GetComponent<Box>();

                box.Owner = customer;

                box.Active();
                
                box.OnBoxed += OnCheckoutDone;

                customer.OnMoveOut += NextCustomer;
            }
        }

        private void OnCheckoutDone()
        {
            for (var index = 0; index < customerSlot.Count; index++)
            {
                var slot = customerSlot[index];
                        
                if (slot.customer != customer) continue;

                slot.customer = null;

                customerSlot[index] = slot;
            }
            
            customer = customerQueue.Dequeue();

            var position     = customer.transform.position;
            
            var spawnCashPos = new Vector3(position.x, 0.5f, position.z);
            
            SpawnCash(spawnCashPos, customer.LimitFruit * 5);
            
            customer.CollectBox(box);

            box = null;
        }

        private async void SpawnCash(Vector3 spawnPos, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var cash = PoolManager.Instance.Spawn(Entity.Cash).GetComponent<Cash>();
                
                cashStack.Push(cash);

                var index = cashStack.Count - 1;

                var temp1 = index / 12;
                
                var temp2 = (index % 12) / 6;
                
                var temp3 = index % 6;
                
                cash.OnSpawn(spawnPos, new Vector3( 0.215f * temp3, 0.082f * temp1, - 0.41f * temp2));

                await UniTask.Delay(100);
            }
        }
        
        private async void NextCustomer()
        {
            await UniTask.Delay(500);

            var i = 0;
            
            for (; i < customerSlot.Count; i++)
            {
                var slot = customerSlot[i];

                slot.customer = null;

                customerSlot[i] = slot;
            }
            
            i = 0;
            
            foreach (var customer in customerQueue)
            {
                var slot = customerSlot[i];

                slot.customer = customer;

                customerSlot[i] = slot;

                i++;
            }
            
            i = 0;
            
            foreach (var customer in customerQueue)
            {
                customer.SetPositionTarget(customerSlot[i].transform.position);

                i++;
            }
            
            await UniTask.Delay(1000);
            
            Processing = false;
        }
    }
}
