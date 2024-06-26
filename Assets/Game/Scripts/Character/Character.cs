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

        [SerializeField] private ParticleSystem smokeFx;
        
        [SerializeField] protected Rigidbody body;
        
        [SerializeField] protected List<Transform> slotList;
        
        [SerializeField]
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
                    smokeFx.Stop();
                    break;
                case State.Move:
                    animator.SetBool("IsMove", true);
                    animator.SetBool("IsCarryMove", false);
                    animator.SetBool("IsEmpty", true);
                    smokeFx.Play();
                    break;
                case State.CarryMove:
                    animator.SetBool("IsMove", true);
                    animator.SetBool("IsCarryMove", true);
                    animator.SetBool("IsEmpty", false);
                    smokeFx.Play();
                    break;
                case State.CarryIdle:
                    animator.SetBool("IsMove", false);
                    animator.SetBool("IsCarryMove", false);
                    animator.SetBool("IsEmpty", false);
                    smokeFx.Stop();
                    break;
            }
        }

        public virtual void CollectFruit(Fruit fruit)
        {
            if(fruitList.Count >= limitFruit || !fruit.IsReady) return;
            
            fruitList.Add(fruit);
            
            foreach (var slot in slotList)
            {
                if (slot.childCount == 0)
                {
                    fruit.MoveToTarget(slot);
                    break;
                }
            }
        }
        
        // public void GiveFruit(Fruit fruit)
        // {
        //     if(fruitList.Count >= 3) return;
        //     
        //     fruitList.Add(fruit);
        //     
        //     foreach (var slot in slotList)
        //     {
        //         if (slot.childCount == 0)
        //         {
        //             fruit.MoveToTarget(slot);
        //         }
        //     }
        // }

        protected virtual void Rotate(Vector3 direction)
        {
            var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                
            var targetRotation = Quaternion.Euler(0, targetAngle, 0);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 8f);
        }
    }
}
