namespace Game.Scripts.Manager
{
    using System.Collections.Generic;
    using Game.Scripts.Character;
    using Game.Scripts.GameModule;
    using Game.Scripts.Location;
    using UnityEngine;

    public class CounterManager : Singleton<CounterManager>
    {
        [SerializeField] private List<CounterLocation> counterList;

        public bool IsFullQueue
        {
            get
            {
                foreach (var counter in counterList)
                {
                    if(counter.IsFullQueue) continue;
                    
                    return false;
                }

                return true;
            }
        }
        
        public bool GetQueue(Customer customer, out Transform queue)
        {  
            foreach (var counter in counterList)
            {
                if (!counter.TryGetEmptyQueue(customer, out var target)) continue;
                
                queue = target;
                    
                return true;
            }

            queue = null;
            
            return false;
        }
    }
}
