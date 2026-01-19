using UnityEngine;

public abstract class BulletBase : MonoBehaviour, IPoolObject
{
    public GameObject GameObject => gameObject;
    [SerializeField]
    protected Rigidbody2D _rb;
    protected ProjectileContext _projectileInfo;
    private IResettablePool _pool;
    public void Initialize(IResettablePool pool) => _pool = pool;
    public void OnDespawn(Transform parent)
    {
        transform.SetParent(parent);
        _rb.linearVelocity = Vector2.zero;
    }

    public void OnSpawn()
    {
       
        transform.SetParent(null);
        _rb.linearVelocity = transform.up * _projectileInfo.BulletSpeed;
        Invoke(nameof(OnDestroyBullet), _projectileInfo.Lifetime);
    }

    public void InjectBulletInfo(ProjectileContext projectileInfo)
    {
        _projectileInfo = projectileInfo;
    }

    protected void OnDestroyBullet()
    {
        _pool.ReturnObjectToPool(this);
        CancelInvoke();
        Debug.Log("Recycle Bullet");
    }


}

