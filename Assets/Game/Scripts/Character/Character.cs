using UnityEngine;

namespace Game.Scripts.Character
{
    using System.Collections.Generic;
    using Game.Scripts.Objects.Tomato;

    public enum State
    {
        Idle = 0,
        Move = 1,
        CarryMove = 2,
        CarryIdle = 3
    }
    
    public class Character : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        [SerializeField] protected Rigidbody body;
        
        [SerializeField] protected List<Transform> slotList;

        protected List<Fruit> fruitList = new ();

        protected int limitFruit = 3;
        
        private State state;

        public bool ReachLimitSlot => fruitList.Count >= limitFruit;
        
        public State State
        {
            get => state;
            set
            {
                if (state == value) return;
                
                state = value;    
                OnChangeState();
            }
        }
        
        public Fruit ReadyFruit
        {
            get
            {
                for (var index = fruitList.Count - 1; index >= 0; index--)
                {
                    var fruit = fruitList[index];
                    if (fruit != null && fruit.IsReady) return fruit;
                }

                return null;
            }
        }
        
        private void OnChangeState()
        {
            switch (state)
            {
                case State.Idle:
                    animator.SetBool("IsMove", false);
                    animator.SetBool("IsCarryMove", false);
                    animator.SetBool("IsEmpty", true);
                    break;
                case State.Move:
                    animator.SetBool("IsMove", true);
                    animator.SetBool("IsCarryMove", false);
                    animator.SetBool("IsEmpty", true);
                    break;
                case State.CarryMove:
                    animator.SetBool("IsMove", true);
                    animator.SetBool("IsCarryMove", true);
                    animator.SetBool("IsEmpty", false);
                    break;
                case State.CarryIdle:
                    animator.SetBool("IsMove", false);
                    animator.SetBool("IsCarryMove", false);
                    animator.SetBool("IsEmpty", false);
                    break;
            }
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
        
        public void GiveFruit(Fruit fruit)
        {
            if(fruitList.Count >= 3) return;
            
            fruitList.Add(fruit);
            
            foreach (var slot in slotList)
            {
                if (slot.childCount == 0)
                {
                    fruit.Collect(slot);
                }
            }
        }
    }
}
