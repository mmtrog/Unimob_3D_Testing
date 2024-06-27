using UnityEngine;

namespace Game.Scripts.Objects.Tomato
{
    using System.Collections.Generic;
    using Game.Scripts.Character;
    using Game.Scripts.GameModule;

    public class Plant : MonoBehaviour
    {
        [SerializeField] private float timePerSpawn;
        
        [SerializeField] private List<Transform> slotList;

        private Queue<Fruit> fruitQueue = new();

        private float timeSpawnCounter;

        private bool ReachLimit => fruitQueue.Count >= 3;
        
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
            if(fruitQueue.Count >= 3) return false;

            var newFruit = PoolManager.Instance.Spawn(Entity.Fruit).GetComponent<Fruit>();

            fruitQueue.Enqueue(newFruit);
            
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
            if(ReachLimit) return;
            
            timeSpawnCounter -= Time.deltaTime;

            if (timeSpawnCounter <= 0)
            {
                if (OnFruit())
                {
                    timeSpawnCounter = timePerSpawn;  
                }
                else
                {
                    timeSpawnCounter = timePerSpawn + 0.5f;    
                }
                    
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.name != "Player") return;

            if (!other.TryGetComponent<Character>(out var character)) return;
            
            if(character.ReachLimitSlot || fruitQueue.Count == 0) return;
                
            var fruit = fruitQueue.Peek();
                
            if(!fruit.IsReady) return;
            
            character.CollectFruit(fruit);

            fruitQueue.Dequeue();
        }
    }
}
