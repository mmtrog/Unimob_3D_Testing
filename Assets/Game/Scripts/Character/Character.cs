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

        [SerializeField] protected List<Transform> slotList;
        
        protected List<Fruit> fruitList = new ();

        private State state;

        public bool ReachLimitSlot => fruitList.Count >= 3;
        
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

        public void Collect(Fruit fruit)
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
