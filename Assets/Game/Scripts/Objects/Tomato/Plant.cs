using UnityEngine;

namespace Game.Scripts.Objects.Tomato
{
    using System.Collections.Generic;
    using DG.Tweening;
    using Game.Scripts.Character;
    using Game.Scripts.GameModule;

    public class Plant : MonoBehaviour
    {
        [SerializeField] private float timePerSpawn;

        [SerializeField] private Transform plantTrans;
        
        [SerializeField] private List<Transform> slotList;

        private Sequence zoomSequence;
        
        private Queue<Fruit> fruitQueue = new();

        private float timeSpawnCounter;

        private bool ReachLimit => fruitQueue.Count >= 3;
        
        private void OnEnable()
        {
            for (int i = 0; i < 2; i++)
            {
                OnFruit();    
            }

            zoomSequence = DOTween.Sequence();

            zoomSequence.Pause();

            zoomSequence.SetAutoKill(false);

            zoomSequence.Append(plantTrans.DOScale(new Vector3(220f, 220f, 220f), 0.3f).SetEase(Ease.OutBack));
            
            zoomSequence.Append(plantTrans.DOScale(new Vector3(200f, 200f, 200f), 0.2f).SetEase(Ease.Linear));
            
            timeSpawnCounter = timePerSpawn;
        }

        public bool OnFruit()
        {
            if(fruitQueue.Count >= 3)
            {
                Debug.Log($"Plant: queueCount{fruitQueue.Count}");
                
                return false;
            }

            var newFruit = PoolManager.Instance.Spawn(Entity.Fruit).GetComponent<Fruit>();

            foreach (var slot in slotList)
            {
                if (slot.childCount != 0) continue;
                
                zoomSequence.Restart();
                
                newFruit.Regenerate(slot);
                
                break;
            }
            
            fruitQueue.Enqueue(newFruit);
            
            timeSpawnCounter += timePerSpawn / 2;  
            
            Debug.Log($"Plant: OnFruit");
            
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

            if (!other.TryGetComponent<Seller>(out var player)) return;
            
            if(player.ReachLimitSlot || fruitQueue.Count == 0) return;
                
            var fruit = fruitQueue.Peek();
                
            if(!fruit.IsReady) return;
            
            player.CollectFruit(fruit);

            fruitQueue.Dequeue();
        }
    }
}
