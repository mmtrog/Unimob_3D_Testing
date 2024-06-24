using UnityEngine;

namespace Game.Scripts.Objects.Tomato
{
    using System;
    using System.Collections.Generic;
    using Dasis.DesignPattern;
    using Game.Scripts.Character;

    public class Plant : MonoBehaviour
    {
        [SerializeField] private float timePerSpawn;
        
        [SerializeField] private List<Transform> slotList;

        private List<Fruit> fruitList = new();

        private float timeSpawnCounter;

        public Fruit ReadyFruit
        {
            get
            {
                foreach (var fruit in fruitList)
                {
                    if (fruit != null || fruit.IsReady) return fruit;
                }

                return null;
            }
        }
        
        private void Start()
        {
            for (int i = 0; i < 2; i++)
            {
                OnFruit();    
            }

            timeSpawnCounter = timePerSpawn;
        }

        public bool OnFruit()
        {
            if(fruitList.Count >= 3) return false;

            var newFruit = PoolManager.Instance.Spawn(Entity.Fruit).GetComponent<Fruit>();

            fruitList.Add(newFruit);
            
            foreach (var slot in slotList)
            {
                if (slot.childCount != 0) continue;
                
                newFruit.Regenerate(slot);
                
                break;
            }
            
            return true;
        }

        private void Update()
        {
            timeSpawnCounter -= Time.deltaTime;

            if (timeSpawnCounter <= 0)
            {
                if (OnFruit())
                {
                    timeSpawnCounter = timePerSpawn;  
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.name != "Player") return;

            if (!other.TryGetComponent<Character>(out var character)) return;
            
            if(character.ReachLimitSlot) return;
                
            var fruit = ReadyFruit;
            
            if(fruit == null) return;
                
            character.Collect(fruit);

            fruitList.Remove(fruit);
        }
    }
}
