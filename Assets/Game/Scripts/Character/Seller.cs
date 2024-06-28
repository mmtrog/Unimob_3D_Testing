namespace Game.Scripts.Character
{
    using Game.Scripts.Location;
    using UnityEngine;

    public class Seller : Character
    {
        [SerializeField] private RectTransform handle;
        
        private void Update()
        {
            var handlePos = handle.localPosition;
            
            if (handlePos.magnitude <= 0.01f)
            {
                State = fruitList.Count > 0 ? State.CarryIdle : State.Idle;

                body.velocity = Vector3.zero;
            }
            else
            {
                State = fruitList.Count > 0 ? State.CarryMove : State.Move;
                
                // var targetAngle = Mathf.Atan2(handlePos.x, handlePos.y) * Mathf.Rad2Deg;
                //
                // var targetRotation = Quaternion.Euler(0, targetAngle, 0);
                //
                // transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 8f);
                
                Rotate(handlePos);
            }
        }
        
        protected override void Rotate(Vector3 direction)
        {
            var targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            
            var targetRotation = Quaternion.Euler(0, targetAngle, 0);
            
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 8f);
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (other.isTrigger && other.gameObject.name == "CounterLocation")
            {
                if (!other.TryGetComponent<CounterLocation>(out var counter)) return;

                if (counter.ReachLimitSlot) return;

                var fruit = ReadyFruit;

                if (fruit == null) return;

                if (counter.CollectFruit(fruit))
                {
                    fruitList.Remove(fruit);   
                }
            }  
        }
    }
}