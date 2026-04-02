using UnityEngine;
using System.Collections.Generic;

public class GameObjectPooler : ObjectPooler
{
    public int amountToPool;
    public GameObject ObjectToPool;

    private void Start()
    {
        SetObjectToPool(ObjectToPool);
        FillObjectPool();
    }

    public override void FillObjectPool()
    {
        // Ensure the object pool is initialized
        if (_objectPool == null || _objectPool.PooledGameObjects == null)
        {
            if (CreateWaitingPool())
                _objectPool.PooledGameObjects = new List<GameObject>();
        }

        InitializeObjects(_objectPool, amountToPool, ObjectToPool);
    }

    public GameObject GetPooledGameObject(Vector3 position)
    {
        foreach (GameObject obj in _objectPool.PooledGameObjects)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                obj.transform.position = position;
                return obj;
            }
        }

        GameObject go = Instantiate(objectToPool, gameObject.transform);
        go.GetComponent <PoolableObject>().Initialize(_objectPool);
        _objectPool.PooledGameObjects.Add(go);
        go.SetActive(true);
        go.transform.position = position;
        return go;
    }
}