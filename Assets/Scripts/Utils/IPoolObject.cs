using UnityEngine;

public interface IPoolObject
{
    GameObject GameObject { get; }
    void Initialize<TPool>(ObjectPool<TPool> poolManager) where TPool : IPoolObject;
    void OnSpawn();
    void OnDespawn(Transform parent);
}
