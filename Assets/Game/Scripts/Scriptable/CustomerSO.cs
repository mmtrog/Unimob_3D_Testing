namespace Game.Scripts.Scriptable
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "CustomerSO", menuName = "Data/CustomerSO")]
    public class CustomerSO : ScriptableObject
    {
        public List<Material> materials;
    }
}