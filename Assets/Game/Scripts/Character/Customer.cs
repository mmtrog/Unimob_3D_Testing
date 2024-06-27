namespace Game.Scripts.Character
{
    using System;
    using Cysharp.Threading.Tasks;
    using Game.Scripts.Manager;
    using Game.Scripts.Objects;
    using Game.Scripts.Objects.Tomato;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public enum CustomerState
    {
        Free = 0,
        Buy = 1,
        CheckOut = 2,
        Recycle = 3,
    }
    
    public class Customer : Character
    {
        private Vector3 targetPos;

        private Vector3 targetRot;
        
        private bool isReachTarget;

        private CustomerState logicState;
        public  CustomerState LogicState => logicState;

        public int LimitFruit => limitFruit;
        
        public void OnSpawn()
        {
            OnMoveOut = null;

            isReachTarget = true;
            
            logicState = CustomerState.Free;
            
            limitFruit = Random.Range(1, 4);

            transform.localPosition = Vector3.zero;
            
            gameObject.SetActive(true);
            
            SetIdle();
        }

        public void SetPositionTarget(Vector3 targetPos)
        {
            isReachTarget = false;
            
            this.targetPos = targetPos;
        }
        
        public void SetRotationTarget(Vector3 targetRot)
        {
            this.targetRot = targetRot;
        }
        
        public void SetIdle()
        {
            State = State.Idle;
        }
        
        private void Update()
        {
            var direction = targetPos - transform.position;
            
            if (direction.magnitude <= 0.3f || isReachTarget)
            {
                if (!isReachTarget)
                {
                    State = fruitList.Count > 0 ? State.CarryIdle : State.Idle;

                    body.velocity = Vector3.zero;

                    isReachTarget = true;
                    
                    if ((int) logicState < 2)
                    {
                        logicState = (CustomerState) Enum.GetValues(typeof(CustomerState)).GetValue((int) logicState + 1);
                    }
                    else
                    {
                        if (logicState == CustomerState.Recycle)
                        {
                            Recycle();
                        }
                    }
                }
                else
                {
                    var rotation = targetRot - transform.position;
                    
                    Rotate(rotation); 
                }
            }
            else
            {
                State = fruitList.Count > 0 ? State.CarryMove : State.Move;
                
                Rotate(direction); 
            }
        }
        
        public override void CollectFruit(Fruit fruit)
        {
            if(fruitList.Count >= limitFruit) return;
           
            fruitList.Add(fruit);

            foreach (var slot in slotList)
            {
                if (slot.childCount == 0)
                {
                    fruit.MoveToTarget(slot);
                    break;
                }
            }

            if (fruitList.Count == limitFruit)
            {
                MoveToCheckOut();
            }
        }
        
        public async void CollectBox(Box box)
        {
            box.MoveToTarget(slotList[0]);

            await UniTask.Delay(400);
            
            MoveOut();
        }

        public Action OnMoveOut;
        
        private void MoveOut()
        {
            SetPositionTarget(new Vector3(-16f, 0, 3f));

            logicState = CustomerState.Recycle;
            
            OnMoveOut?.Invoke();
        }

        public void RemoveFruit(Fruit fruit)
        {
            fruitList.Remove(fruit);  
        }

        public void MoveToCheckOut()
        {
            CheckoutManager.Instance.GetQueue(this, out var queue);
                
            SetPositionTarget(queue.position);  
            
            SetRotationTarget(queue.parent.transform.position);
        }

        private void Recycle()
        {
            for (int i = 0; i < slotList[0].childCount; i++)
            {
                if (slotList[0].GetChild(0).TryGetComponent<Box>(out var box))
                {
                    box.Recycle();
                    
                    continue;
                }
                
                if (slotList[0].GetChild(0).TryGetComponent<Fruit>(out var fruit))
                {
                    fruit.Recycle();
                }
            }
            
            gameObject.SetActive(false);   
        }
    }
}
