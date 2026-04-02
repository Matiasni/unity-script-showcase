using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;
    // if this is true, the pool will try not to create a new waiting pool if it finds one with the same name.
    public bool MutualizeWaitingPools = false;
    // if this is true, all waiting and active objects will be regrouped under an empty game object. Otherwise they'll just be at top level in the hierarchy
    public bool NestWaitingPool = true;
    // if this is true, the waiting pool will be nested under this object
    public bool NestUnderThis = false;

    /// this object is just used to group the pooled objects
    protected GameObject _waitingPool = null;
    protected ObjectPool _objectPool;
    protected GameObject objectToPool;
    protected const int _initialPoolsListCapacity = 5;
    protected bool _onSceneLoadedRegistered = false;

    public static List<ObjectPool> _pools = new List<ObjectPool>(_initialPoolsListCapacity);

    // Adds a pooler to the static list if needed
    public static void AddPool(ObjectPool pool)
    {
        if (_pools == null)
        {
            _pools = new List<ObjectPool>(_initialPoolsListCapacity);
        }
        if (!_pools.Contains(pool))
        {
            _pools.Add(pool);
        }
    }

    // Removes a pooler from the static list
    public static void RemovePool(ObjectPool pool)
    {
        _pools?.Remove(pool);
    }

    // On awake we fill our object pool
    protected virtual void Awake()
    {
        Instance = this;
    }

    // Creates the waiting pool or tries to reuse one if there's already one available
    protected virtual bool CreateWaitingPool()
    {
        if (!MutualizeWaitingPools)
        {
            // we create a container that will hold all the instances we create
            _waitingPool = new GameObject(DetermineObjectPoolName());
            SceneManager.MoveGameObjectToScene(_waitingPool, this.gameObject.scene);
            _objectPool = _waitingPool.AddComponent<ObjectPool>();
            _objectPool.PooledGameObjects = new List<GameObject>();
            ApplyNesting();
            return true;
        }
        else
        {
            ObjectPool objectPool = ExistingPool(DetermineObjectPoolName());
            if (objectPool != null)
            {
                _objectPool = objectPool;
                _waitingPool = objectPool.gameObject;
                return false;
            }
            else
            {
                _waitingPool = new GameObject(DetermineObjectPoolName());
                SceneManager.MoveGameObjectToScene(_waitingPool, this.gameObject.scene);
                _objectPool = _waitingPool.AddComponent<ObjectPool>();
                _objectPool.PooledGameObjects = new List<GameObject>();
                ApplyNesting();
                AddPool(_objectPool);
                return true;
            }
        }
    }

    // Looks for an existing pooler for the same object, returns it if found, returns null otherwise
    public virtual ObjectPool ExistingPool(string poolName)
    {
        if (_pools == null)
        {
            _pools = new List<ObjectPool>(_initialPoolsListCapacity);
        }
        if (_pools.Count == 0)
        {
            var pools = FindObjectsByType<ObjectPool>(FindObjectsSortMode.None);
            if (pools.Length > 0)
            {
                _pools.AddRange(pools);
            }
        }
        foreach (ObjectPool pool in _pools)
        {
            if ((pool != null) && (pool.name == poolName))
            {
                return pool;
            }
        }
        return null;
    }

    // If needed, nests the waiting pool under this object
    protected virtual void ApplyNesting()
    {
        if (NestWaitingPool && NestUnderThis && (_waitingPool != null))
        {
            _waitingPool.transform.SetParent(this.transform);
        }
    }

    // Determines the name of the object pool.
    protected virtual string DetermineObjectPoolName()
    {
        return ("[ObjectPooler] " + this.name);
    }

    // Implement this method to fill the pool with objects
    public virtual void FillObjectPool()
    {
        return;
    }

    public virtual void InitializeObjects(ObjectPool pool, int amountToPool, GameObject objectToSpawn)
    {
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(objectToSpawn);
            obj.GetComponent<PoolableObject>().Initialize(pool);
            obj.gameObject.SetActive(false);
            _objectPool.PooledGameObjects.Add(obj.gameObject);
        }
    }

    // Implement this method to return a gameobject
    public virtual GameObject GetPooledGameObject()
    {
        return null;
    }

    // Implement this method to return a gameobject
    public virtual GameObject GetPooledGameObjectAtPosition(Vector3 position)
    {
        return null;
    }

    //Implement this method to set the prefab for the PooledObject
    public virtual void SetObjectToPool(GameObject objectPrefab)
    {
        if (objectPrefab.GetComponent<PoolableObject>())
        {
            objectToPool = objectPrefab;
        }
        else
            Debug.LogError("PMPoolableObject: Missing Component. This prefab isn't a poolable Object");
    }

    // Destroys the object pool
    public virtual void DestroyObjectPool()
    {
        if (_waitingPool != null)
        {
            Destroy(_waitingPool.gameObject);
        }
    }

    // On enable we register to the scene loaded hook
    protected virtual void OnEnable()
    {
        if (!_onSceneLoadedRegistered)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    // OnSceneLoaded we recreate 
    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if ((_objectPool == null) || (_waitingPool == null))
        {
            if (this != null && objectToPool != null)
            {
                FillObjectPool();
            }
        }
    }

    // On Destroy we remove ourselves from the list of poolers 
    private void OnDestroy()
    {
        if ((_objectPool != null) && NestUnderThis)
        {
            RemovePool(_objectPool);
        }

        if (_onSceneLoadedRegistered)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            _onSceneLoadedRegistered = false;
        }
    }
}