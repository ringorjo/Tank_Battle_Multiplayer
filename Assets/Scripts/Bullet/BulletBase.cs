using UnityEngine;

public abstract class BulletBase : MonoBehaviour, IPoolObject
{
    public GameObject GameObject => gameObject;
    [SerializeField]
    protected Rigidbody2D _rb;
    [SerializeField]
    protected ProjectileData _projectileData;
    protected ObjectPool<BulletBase> _objectPool;

    public void Initialize<TPool>(ObjectPool<TPool> poolManager) where TPool : IPoolObject
    {
        _objectPool = poolManager as ObjectPool<BulletBase>;
    }

    public void OnDespawn(Transform parent)
    {
        transform.SetParent(parent);
        _rb.linearVelocity = Vector2.zero;
    }

    public  void OnSpawn()
    {
        transform.SetParent(null);
        _rb.linearVelocity = transform.up * _projectileData.Speed;
        Invoke(nameof(OnDestroyBullet), _projectileData.LifeTime);
    }

    protected void OnDestroyBullet()
    {
        _objectPool.RecycleObject(this);
        CancelInvoke();
        Debug.Log("Recycle Bullet");
    }
}
    
