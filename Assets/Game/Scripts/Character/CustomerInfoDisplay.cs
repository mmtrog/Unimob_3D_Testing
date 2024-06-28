using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Character
{
    using Camera = UnityEngine.Camera;

    public class CustomerInfoDisplay : MonoBehaviour
    {
        [SerializeField] private List<GameObject> statusLabels;

        [SerializeField] private TextMeshPro fruitRequiredTMP;

        [SerializeField] private Customer customer;
        
        private int limitFruit;

        private Transform cameraTrans;
        
        private void Start()
        {
            cameraTrans = Camera.main.transform;
            
            customer.OnCollectFruit += UpdateFruitAmount;
        }

        public void SetUp(int limitFruit)
        {
            this.limitFruit = limitFruit;
            
            fruitRequiredTMP.text = $"0/{limitFruit}";
        }

        public void UpdateFruitAmount(int amount)
        {
            fruitRequiredTMP.text = $"{amount}/{limitFruit}";
        }
        
        public void UpdateStatus(int state)
        {
            var i = 0;
            for (; i < 3; i++)
            {
                statusLabels[i].SetActive(i == state);
            }
        }

        private void Update()
        {
            transform.LookAt(cameraTrans);
        }
    }
}
