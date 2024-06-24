namespace Game.Scripts.Character
{
    using UnityEngine;

    public class Seller : Character
    {
        [SerializeField] private RectTransform handle;

        [SerializeField] private Rigidbody body;

        private void Update()
        {
            var handlePos = handle.localPosition;
            
            if (handlePos.magnitude <= 0.01f)
            {
                State = State.Idle;

                body.velocity = Vector3.zero;
            }
            else
            {
                State = State.Move;
                
                var targetAngle = Mathf.Atan2(handlePos.x, handlePos.y) * Mathf.Rad2Deg;
                
                var targetRotation = Quaternion.Euler(0, targetAngle, 0);

                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }
    }
}