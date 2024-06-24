using UnityEngine;

namespace Game.Scripts.Camera
{
    using System;

    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform playerTrans;

        [SerializeField] private Transform cameraTrans;
        
        private Vector3 originalDirection;
        void Start()
        {
            originalDirection = cameraTrans.position - playerTrans.position;
        }

        private void Update()
        {
            cameraTrans.position = playerTrans.position + originalDirection;
        }
    }
}
