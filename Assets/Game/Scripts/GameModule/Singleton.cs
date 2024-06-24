using UnityEngine;

namespace Dasis.DesignPattern
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _ins = null;

        public static T Instance
        {
            get
            {
                if (!_ins)
                {
                    _ins = FindObjectOfType<T>();
                }
                return _ins;
            }
        }

        protected void Awake()
        {
            if (!_ins)
            {
                _ins = FindObjectOfType<T>();
                OnInitialization();
            }
        }

        public virtual void OnDestroy()
        {
            _ins = null;
            OnExtinction();
        }

        public virtual void OnInitialization() { }
        public virtual void OnExtinction() { }
    }
}
