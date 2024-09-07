using UnityEngine;
using UnityEngine.Pool;

public class BestObjectPool<T> where T : MonoBehaviour
{
    private ObjectPool<T> _objectPool;

    public BestObjectPool(T prefab, int capacity = 10, int maxcapacity = 500)
    {
        _objectPool = new ObjectPool<T>
        (
            createFunc: () =>
            {
                var instance = Object.Instantiate(prefab);
                instance.gameObject.SetActive(false);
                return instance;
            },
            actionOnGet: obj => obj.gameObject.SetActive(true),
            actionOnRelease: obj => obj.gameObject.SetActive(false),
            actionOnDestroy: Object.Destroy,
            defaultCapacity: capacity,
            maxSize: maxcapacity,
            collectionCheck: false
        );
        
        var poolObjects = new T[capacity];
        for (var i = 0; i < capacity; i++)
        {
            poolObjects[i] = _objectPool.Get();
        }
        for (var i = 0; i < capacity; i++)
        {
            _objectPool.Release(poolObjects[i]);
        }
    }

    public T Get() => _objectPool.Get();
    
    public void Release(T obj) => _objectPool.Release(obj);
}