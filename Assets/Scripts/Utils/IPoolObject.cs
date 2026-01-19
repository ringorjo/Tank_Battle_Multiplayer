using UnityEngine;

public interface IPoolObject
{
    GameObject GameObject { get; }
    void Initialize(IResettablePool pool);
    void OnSpawn();
    void OnDespawn(Transform parent);
}

public interface IResettablePool
{
    void ReturnObjectToPool(IPoolObject o);
}
