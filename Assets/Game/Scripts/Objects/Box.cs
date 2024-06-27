using UnityEngine;

namespace Game.Scripts.Objects
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class Box : MonoBehaviour
    {
        [SerializeField] private Transform modelTrans;

        [SerializeField] private Animator animator;

        [SerializeField] private List<Transform> slotList;

        private bool ready;
        
        private void OnEnable()
        {
            ready = false;
            
            modelTrans.localScale = Vector3.zero;

            animator.SetBool("Close", false);
            
            
        }
        
        public virtual void CollectFruit(Fruit fruit)
        {
            if(fruitList.Count >= 3) return;
            
            fruitList.Add(fruit);
            
            foreach (var slot in slotList)
            {
                if (slot.childCount == 0)
                {
                    fruit.Collect(slot);
                    break;
                }
            }
        }
    }
}
