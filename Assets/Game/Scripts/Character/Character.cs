using UnityEngine;

namespace Game.Scripts.Character
{
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

        private State state;
        
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
    }
}
