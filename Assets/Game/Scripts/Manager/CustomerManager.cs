namespace Game.Scripts.Character
{
    using System;
    using System.Collections.Generic;
    using Game.Scripts.GameModule;
    using Game.Scripts.Manager;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class CustomerManager : Singleton<CustomerManager>
    {
        private float timePerSpawn;
        
        private List<Customer> customerList = new ();

        private float timeSpawnCounter = 3f;

        private void Start()
        {
            Application.targetFrameRate = 60;

            timePerSpawn = Random.Range(3.5f, 5f);
        }

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
            
            timePerSpawn = Random.Range(3.5f, 5f);
            
            var customer = PoolManager.Instance.Spawn(Entity.Customer).GetComponent<Customer>();
            
            customerList.Add(customer);

            customer.OnSpawn();
            
            if (CounterManager.Instance.GetQueue(customer, out var target))
            {
                customer.SetPositionTarget(target.position); 
                
                customer.SetRotationTarget(target.parent.transform.position);
            }
        }
    }
}
