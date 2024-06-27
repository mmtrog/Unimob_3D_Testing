namespace Game.Scripts.GameModule
{
    using System.Collections.Generic;
    using UnityEngine;

    public class PoolManager : Singleton<PoolManager>
    {
        [SerializeField]
        private List<PoolPrefab> prefabs;

        private readonly List<Pool> pools = new();

        public override void OnInitialization()
        {
            pools.Clear();
            for (int i = 0; i < System.Enum.GetValues(typeof(Entity)).Length; i++)
            {
                Transform parent = prefabs[i].parent != null ? prefabs[i].parent : transform;
                pools.Add(new Pool(prefabs[i].gameObject, parent));
            }
        }

        public GameObject Spawn(Entity entity)
        {
            GameObject gameObject = pools[(int)entity].Spawn();
            return gameObject;
        }
    }

    [System.Serializable]
    public class PoolPrefab
    {
        public GameObject gameObject;
        public Transform parent;
    }

    public class Pool
    {
        private readonly List<GameObject> _pooledObjects   = new();
        private readonly Transform         _parent;
        private readonly GameObject        _prefab;

        public int PooledCount => _pooledObjects.Count;

        public Pool(GameObject prefab, Transform parent)
        {
            _prefab = prefab;
            _parent = parent;
        }

        public GameObject Spawn()
        {
            if (PooledCount > 0)
            {
                foreach (var pooledObject in _pooledObjects)
                {
                    if (!pooledObject.activeInHierarchy)
                    {
                        pooledObject.transform.SetParent(_parent);   
                        
                        pooledObject.SetActive(true);

                        return pooledObject;
                    }
                }

                return CreateNew();
            }
            
            return CreateNew();
        }

        private GameObject CreateNew()
        {
            var gameObject = Object.Instantiate(_prefab, _parent);
            _pooledObjects.Add(gameObject);
            gameObject.SetActive(true);
            return gameObject;
        }
    }
}
