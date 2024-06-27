namespace Game.Scripts.Character
{
    using Game.Scripts.Manager;
    using Game.Scripts.Objects.Tomato;
    using UnityEngine;

    public class Customer : Character
    {
        public Transform Slot { get; set; }

        public int LimitFruit => limitFruit;

        
        
        private Vector3 targetPos;
        
        public void OnSpawn()
        {
            limitFruit = Random.Range(1, 5);

            transform.localPosition = Vector3.zero;
            
            gameObject.SetActive(true);
            
            SetIdle();
        }

        public void SetTarget(Vector3 targetPos)
        {
            this.targetPos = targetPos;
        }
        
        public void SetTransformTarget(Transform targetTrans)
        {
            Slot = targetTrans;

            targetPos = targetTrans.position;
        }
        
        public void SetIdle()
        {
            State = State.Idle;
        }
        
        private void Update()
        {
            var direction = targetPos - transform.position;
            
            if (direction.magnitude <= 0.3f)
            {
                State = fruitList.Count > 0 ? State.CarryIdle : State.Idle;

                body.velocity = Vector3.zero;
            }
            else
            {
                State = fruitList.Count > 0 ? State.CarryMove : State.Move;
                
                var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                
                var targetRotation = Quaternion.Euler(0, targetAngle, 0);

                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
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
                    fruit.Collect(slot);
                    break;
                }
            }

            if (fruitList.Count == limitFruit)
            {
                MoveToCheckOut();
            }
        }

        public void MoveToCheckOut()
        {
            CheckoutManager.Instance.GetQueue(this, out var queue);
                
            SetTransformTarget(queue);          
        }
    }
}
