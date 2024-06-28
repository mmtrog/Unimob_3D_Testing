using UnityEngine;

namespace Game.Scripts.Objects
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using DG.Tweening;
    using Game.Scripts.Character;
    using Game.Scripts.GameModule;
    using Game.Scripts.Objects.Tomato;

    public class Box : MonoBehaviour
    {
        [SerializeField] private Transform modelTrans;

        [SerializeField] private Animator animator;

        [SerializeField] private List<Transform> slotList;

        private List<Fruit> fruitList = new ();

        public Customer Owner { get; set; }
        
        public bool IsReady { get; set; }
        
        public void Active()
        {
            IsReady = false;

            OnBoxed = null;
            
            transform.localPosition = Vector3.zero;
            
            modelTrans.localScale = Vector3.zero;

            animator.SetBool("Close", false);
            
            gameObject.SetActive(true);

            modelTrans.DOScale(new Vector3(80, 60f, 80), 0.35f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                IsReady = true;
            });
        }
        
        public void CollectFruit(Fruit fruit)
        {
            //if(fruitList.Count >= 4) return;
            
            fruitList.Add(fruit);
            
            foreach (var slot in slotList)
            {
                if (slot.childCount == 0)
                {
                    fruit.MoveToTarget(slot);
                    
                    return;
                }
            }

            fruit.MoveToTarget(slotList[^1]);
        }

        public void MoveToTarget(Transform parent)
        {
            IsReady = false;

            transform.SetParent(parent);
            
            transform.DOKill();
            
            transform.DOLocalMove(new Vector3(0, -0.25f, 0.2f), 0.3f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                transform.DOLocalRotate(Vector3.zero, 0.2f).SetEase(Ease.Linear);
                
                IsReady = true;
            });
        }
        
        public void Boxed()
        {
            IsReady = false;

            animator.SetBool("Close", true);  
        }

        public void OnBoxClose()
        {
            OnBoxed?.Invoke();
        }

        public void Recycle()
        {
            foreach (var fruit in fruitList)
            {
                fruit.Recycle();
            }

            fruitList.Clear();
            
            transform.SetParent(null);
            
            gameObject.SetActive(false);
        }

        public Action OnBoxed;
    }
}
