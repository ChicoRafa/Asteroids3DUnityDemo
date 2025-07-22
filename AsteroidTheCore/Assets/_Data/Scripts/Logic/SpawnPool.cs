using System.Collections.Generic;
using UnityEngine;

public class SpawnPool : MonoBehaviour {

    [System.Serializable]
    public class PrefabPool {
        public GameObject prefabGameObject;
        public int preloadedAmount;

        private HashSet<Transform> spawnedObjects = new HashSet<Transform>();
        private HashSet<Transform> despawnedObjects = new HashSet<Transform>();

        private bool isPreloaded = false;

        public PrefabPool(Transform prefab) {
            prefabGameObject = prefab.gameObject;
        }

        public Transform SpawnInstance(Vector3 position, Quaternion rotation, Vector3 scale, Transform parent = null) {
            Transform transformInstance;
            if (despawnedObjects.Count == 0) {
                transformInstance = SpawnNew(position, rotation, parent);
            } else {
                HashSet<Transform>.Enumerator despawnedEnumerator = this.despawnedObjects.GetEnumerator();
                despawnedEnumerator.MoveNext();
                transformInstance = despawnedEnumerator.Current;

                despawnedObjects.Remove(transformInstance);
                spawnedObjects.Add(transformInstance);

                transformInstance.parent = parent;
                if (parent) {
                    transformInstance.localPosition = position;
                    transformInstance.localRotation = rotation;
                } else {
                    transformInstance.SetPositionAndRotation(position, rotation);
                }
                transformInstance.localScale = scale;
            }

            transformInstance.gameObject.SetActive(true);
            return transformInstance;
        }

        public Transform SpawnNew(Vector3 position, Quaternion rotation, Transform parent = null) {
            GameObject instanceGameObject;
            instanceGameObject = GameObject.Instantiate(this.prefabGameObject, position, rotation, parent);
            if (parent) {
                instanceGameObject.transform.localPosition = position;
                instanceGameObject.transform.localRotation = rotation;
            }
            Transform inst = instanceGameObject.transform;

            // Start tracking the new instance
            this.spawnedObjects.Add(inst);

            return inst;
        }
        
        //carga los objetos y luego los desactiva
        public void DespawnInstance(Transform trans) {
            this.spawnedObjects.Remove(trans);
            this.despawnedObjects.Add(trans);
            trans.parent = null;
            trans.gameObject.SetActive(false);
        }

        public void PreloadInstances(Transform parent = null)
        {
            if (isPreloaded) return;
            isPreloaded = true;

            spawnedObjects = new HashSet<Transform>();
            despawnedObjects = new HashSet<Transform>();

            for (int i = 0; i < preloadedAmount; i++) {
                Transform trans = SpawnNew(Vector3.zero, Quaternion.identity, parent);
                DespawnInstance(trans);
            }
        }

    }


    public static SpawnPool Instance;

    [SerializeField]
    private List<PrefabPool> prefabPools = new List<PrefabPool>();
    public List<PrefabPool> PrefabPools {
        get {
            return prefabPools;
        }
    }

    private Dictionary<GameObject, PrefabPool> _prefabToPoolDictionary = new Dictionary<GameObject, PrefabPool>();
    private Dictionary<Transform, PrefabPool> _spawned = new Dictionary<Transform, PrefabPool>();

    private void Awake() {
        //Singleton
        if (!Instance) {
            Instance = this;
        } else if (Instance != null) {
            Destroy(this);
        }
        //this will make the object persist between scenes
        DontDestroyOnLoad(gameObject);

        //Initialization 
        for (int i = 0; i < prefabPools.Count; i++) {
            PrefabPool p = prefabPools[i];
            p.PreloadInstances(transform);
            if (!this._prefabToPoolDictionary.ContainsKey(p.prefabGameObject)) {
                this._prefabToPoolDictionary.Add(p.prefabGameObject, p);
            }
        }
    }

    public Transform Spawn(Transform prefab, Transform parent) {
        return Spawn(prefab, Vector3.zero, Quaternion.identity, Vector3.one, parent);
    }
    
    public Transform Spawn(Transform prefab, Vector3 position, Quaternion rotation)
    {
        return Spawn(prefab, position, rotation, Vector3.one);
    }
    public Transform Spawn(Transform prefab, Vector3 position, Quaternion rotation, Vector3 scale, Transform parent = null) {
        Transform instanceSpawn;
        PrefabPool prefabPool;
        if (_prefabToPoolDictionary.TryGetValue(prefab.gameObject, out prefabPool)) {
            instanceSpawn = prefabPool.SpawnInstance(position, rotation, scale, parent);
            this._spawned.Add(instanceSpawn, prefabPool);
            return instanceSpawn;
        }

        PrefabPool newPrefabPool = new PrefabPool(prefab);
        CreatePrefabPool(newPrefabPool);

        instanceSpawn = newPrefabPool.SpawnInstance(position, rotation, scale, parent);

        this._spawned.Add(instanceSpawn, newPrefabPool);

        return instanceSpawn;
    }

    public void CreatePrefabPool(PrefabPool prefabPool) {
        this._prefabToPoolDictionary.Add(prefabPool.prefabGameObject, prefabPool);
    }

    public void Despawn(Transform instance) {
        PrefabPool prefabPool;
        if (this._spawned.TryGetValue(instance, out prefabPool)) {
            prefabPool.DespawnInstance(instance);
        }

        _spawned.Remove(instance);

    }
}
