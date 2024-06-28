using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Character
{
    public class CustomerInfoDisplay : MonoBehaviour
    {
        [SerializeField] private List<GameObject> statusLabels;

        [SerializeField] private TextMeshPro fruitRequiredTMP;

        private Customer customer;

        private int limitFruit;
        
        private void Start()
        {
            customer = transform.parent.GetComponent<Customer>();

            limitFruit = customer.LimitFruit;
            
            customer.OnCollectFruit += UpdateFruitAmount;
        }

        public void UpdateFruitAmount(int amount)
        {
            fruitRequiredTMP.text = $"{amount}/{limitFruit}";
        }
        
        public void UpdateStatus(CustomerState state)
        {
            switch (state)
            {
                  
            }
        }
    }
}
