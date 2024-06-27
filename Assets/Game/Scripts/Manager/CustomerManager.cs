namespace Game.Scripts.Character
{
    using System.Collections.Generic;
    using Game.Scripts.GameModule;
    using Game.Scripts.Manager;
    using UnityEngine;

    public class CustomerManager : Singleton<CustomerManager>
    {
        [SerializeField] private float timePerSpawn = 4;
        
        private List<Customer> customerList = new ();

        private float timeSpawnCounter = 0.5f;

        private void Update()
        {
            if (timeSpawnCounter <= 0)
            {
                SpawnCustomer();

                timeSpawnCounter = timePerSpawn;
            }
            else
            {
                timeSpawnCounter -= Time.deltaTime;
            }
        }

        private void SpawnCustomer()
        {
            if(CounterManager.Instance.IsFullQueue) return;
            
            var customer = PoolManager.Instance.Spawn(Entity.Customer).GetComponent<Customer>();
            
            customerList.Add(customer);

            if (CounterManager.Instance.GetQueue(customer, out var target))
            {
                customer.SetTransformTarget(target);    
            }
            
            customer.OnSpawn();
        }
    }
}
