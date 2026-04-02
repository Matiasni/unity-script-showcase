using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public const int maxAmount = 300;
    public List<GameObject> PooledGameObjects;

    public bool HasReachedMaxAmount()
    {
        return PooledGameObjects.Count >= maxAmount;
    }
}