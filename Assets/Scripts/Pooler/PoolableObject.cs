using UnityEngine;
using UnityEngine.Events;

public class PoolableObject : ObjectBounds
{
    [Header("Events")]
    public UnityEvent ExecuteOnEnable;
    public UnityEvent ExecuteOnDisable;

    public delegate void Events();
    public event Events OnSpawnComplete;

    [SerializeField] private ObjectPool _pooler;

    [Header("Poolable Object")]
    /// The life time, in seconds, of the object. If set to 0 it'll live forever, if set to any positive value it'll be set inactive after that time.
    public float LifeTime = 0f;

    /// Turns the instance inactive, in order to eventually reuse it.
    public virtual void DisableObject()
    {
        if (_pooler == null)
        {
            Destroy(this.gameObject);
            return;
        }

        if (_pooler.HasReachedMaxAmount())
        {
            _pooler.PooledGameObjects.Remove(this.gameObject);
            Destroy(this.gameObject);
            return;
        }
        else
        {
            gameObject.SetActive(false);
        }

    }


    /// When the objects get enabled (usually after having been pooled from an ObjectPooler, we initiate its death countdown.
    protected virtual void OnEnable()
    {
        Size = GetBounds().extents * 2;
        if (LifeTime > 0f)
        {
            Invoke("DisableObject", LifeTime);
        }
        ExecuteOnEnable?.Invoke();
    }

    public virtual void Initialize(ObjectPool pooler)
    {
        _pooler = pooler;
    }



    /// When the object gets disabled (maybe it got out of bounds), we cancel its programmed death
    protected virtual void OnDisable()
    {
        ExecuteOnDisable?.Invoke();
        CancelInvoke();
    }

    /// Triggers the on spawn complete event
    public virtual void TriggerOnSpawnComplete()
    {
        OnSpawnComplete?.Invoke();
    }
}